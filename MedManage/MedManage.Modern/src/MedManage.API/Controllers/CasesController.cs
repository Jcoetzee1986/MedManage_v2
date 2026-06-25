using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CasesController : ControllerBase
{
    private readonly ICaseService _caseService;
    private readonly ICaseCopyService _caseCopyService;
    private readonly ICaseWorkflowService _caseWorkflowService;
    private readonly ICaseBusinessRuleService _businessRuleService;
    private readonly IValidator<CreateCaseRequest> _createValidator;
    private readonly IValidator<UpdateCaseRequest> _updateValidator;

    public CasesController(
        ICaseService caseService,
        ICaseCopyService caseCopyService,
        ICaseWorkflowService caseWorkflowService,
        ICaseBusinessRuleService businessRuleService,
        IValidator<CreateCaseRequest> createValidator,
        IValidator<UpdateCaseRequest> updateValidator)
    {
        _caseService = caseService;
        _caseCopyService = caseCopyService;
        _caseWorkflowService = caseWorkflowService;
        _businessRuleService = businessRuleService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a case by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var caseDto = await _caseService.GetByIdAsync(id, cancellationToken);
        
        if (caseDto == null)
        {
            return NotFound(ApiResponse<CaseDto>.ErrorResponse($"Case with ID {id} not found"));
        }

        return Ok(ApiResponse<CaseDto>.SuccessResponse(caseDto));
    }

    /// <summary>
    /// Get cases where the current user is the creator or has them locked
    /// </summary>
    [HttpGet("my-cases")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyCases(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var cases = await _caseService.GetMyCasesAsync(userId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<CaseDto>>.SuccessResponse(cases));
    }

    /// <summary>
    /// Search cases with optional filters and pagination.
    /// Supports 14+ filter parameters including auth number, member, dates, practice, status, ICD, CPT.
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] CaseSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _caseService.SearchAsync(request, cancellationToken);
        return Ok(ApiResponse<PagedResult<CaseDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCaseRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse("Validation failed", errors));
        }

        // Business rule validation (cross-entity checks)
        var businessRuleResult = await _businessRuleService.ValidateCreateAsync(request, cancellationToken);
        if (!businessRuleResult.IsValid)
        {
            var errors = businessRuleResult.Errors.Select(e => $"[{e.Rule}] {e.Message}").ToList();
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse("Business rule validation failed", errors));
        }

        // Apply auth number prefix if member is specified
        if (request.MemberId.HasValue && !string.IsNullOrWhiteSpace(request.AuthNumber))
        {
            var prefixResult = await _businessRuleService.GenerateAuthNumberWithPrefixAsync(
                request.MemberId.Value, request.AuthNumber, cancellationToken);
            request.AuthNumber = prefixResult.GeneratedAuthNumber;
        }

        var caseDto = await _caseService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById), 
            new { id = caseDto.CaseId }, 
            ApiResponse<CaseDto>.SuccessResponse(caseDto, "Case created successfully"));
    }

    /// <summary>
    /// Update an existing case
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseRequest request, CancellationToken cancellationToken)
    {
        if (id != request.CaseId)
        {
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse("ID mismatch between route and body"));
        }

        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse("Validation failed", errors));
        }

        // Business rule validation (cross-entity checks)
        var businessRuleResult = await _businessRuleService.ValidateUpdateAsync(request, cancellationToken);
        if (!businessRuleResult.IsValid)
        {
            var errors = businessRuleResult.Errors.Select(e => $"[{e.Rule}] {e.Message}").ToList();
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse("Business rule validation failed", errors));
        }

        // Apply auth number prefix if member is specified
        if (request.MemberId.HasValue && !string.IsNullOrWhiteSpace(request.AuthNumber))
        {
            var prefixResult = await _businessRuleService.GenerateAuthNumberWithPrefixAsync(
                request.MemberId.Value, request.AuthNumber, cancellationToken);
            request.AuthNumber = prefixResult.GeneratedAuthNumber;
        }

        try
        {
            var caseDto = await _caseService.UpdateAsync(request, cancellationToken);
            return Ok(ApiResponse<CaseDto>.SuccessResponse(caseDto, "Case updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Soft delete a case
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,CaseManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _caseService.DeleteAsync(id, cancellationToken);
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Check for duplicate cases.
    /// A duplicate is defined as: same member + same provider + same admission date.
    /// </summary>
    [HttpPost("check-duplicate")]
    [ProducesResponseType(typeof(ApiResponse<DuplicateCheckResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<DuplicateCheckResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckDuplicate([FromBody] DuplicateCheckRequest request, CancellationToken cancellationToken)
    {
        if (request.MemberId <= 0)
        {
            return BadRequest(ApiResponse<DuplicateCheckResult>.ErrorResponse("MemberId is required"));
        }

        var result = await _caseService.CheckDuplicateAsync(request, cancellationToken);
        return Ok(ApiResponse<DuplicateCheckResult>.SuccessResponse(result));
    }

    /// <summary>
    /// Check if a case exists
    /// </summary>
    [HttpHead("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken)
    {
        var exists = await _caseService.ExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }

    /// <summary>
    /// Check whether a member is eligible for case creation.
    /// Returns member status, AllowServices flag, and any warnings (suspended, deceased, exhausted).
    /// </summary>
    [HttpGet("member-status/{memberId}")]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusCheckResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckMemberStatus(int memberId, CancellationToken cancellationToken)
    {
        var result = await _businessRuleService.CheckMemberAllowServicesAsync(memberId, cancellationToken);
        return Ok(ApiResponse<MemberStatusCheckResult>.SuccessResponse(result));
    }

    /// <summary>
    /// Generate or validate an auth number with the correct medical aid prefix for a member.
    /// </summary>
    [HttpPost("auth-number-prefix")]
    [ProducesResponseType(typeof(ApiResponse<AuthNumberPrefixResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateAuthNumberPrefix([FromBody] AuthNumberPrefixRequest request, CancellationToken cancellationToken)
    {
        if (request.MemberId <= 0)
        {
            return BadRequest(ApiResponse<AuthNumberPrefixResult>.ErrorResponse("MemberId is required"));
        }

        var result = await _businessRuleService.GenerateAuthNumberWithPrefixAsync(
            request.MemberId, request.AuthNumber, cancellationToken);
        return Ok(ApiResponse<AuthNumberPrefixResult>.SuccessResponse(result));
    }

    /// <summary>
    /// Deep copy a case and all its linked sub-entities (CPT, ICD, tariffs, facility types, etc.)
    /// </summary>
    [HttpPost("{id}/copy")]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CopyCase(int id, [FromBody] CaseCopyRequest? request, CancellationToken cancellationToken)
    {
        try
        {
            var copiedCase = await _caseCopyService.CopyAsync(id, request, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { id = copiedCase.CaseId },
                ApiResponse<CaseDto>.SuccessResponse(copiedCase, "Case copied successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Transition a case to a new status (e.g., Booking → Case).
    /// Validates the transition is allowed and applies side effects.
    /// </summary>
    [HttpPost("{id}/transition")]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TransitionStatus(int id, [FromBody] CaseStatusTransitionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updatedCase = await _caseWorkflowService.TransitionStatusAsync(id, request, cancellationToken);
            return Ok(ApiResponse<CaseDto>.SuccessResponse(updatedCase, "Case status transitioned successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseDto>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse(ex.Message));
        }
    }
}
