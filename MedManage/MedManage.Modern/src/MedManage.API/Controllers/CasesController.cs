using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasesController : ControllerBase
{
    private readonly ICaseService _caseService;
    private readonly IValidator<CreateCaseRequest> _createValidator;
    private readonly IValidator<UpdateCaseRequest> _updateValidator;

    public CasesController(
        ICaseService caseService,
        IValidator<CreateCaseRequest> createValidator,
        IValidator<UpdateCaseRequest> updateValidator)
    {
        _caseService = caseService;
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
    /// Search cases with optional filters and pagination
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
}
