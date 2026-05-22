using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Member;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IValidator<CreateMemberRequest> _createValidator;
    private readonly IValidator<UpdateMemberRequest> _updateValidator;

    public MembersController(
        IMemberService memberService,
        IValidator<CreateMemberRequest> createValidator,
        IValidator<UpdateMemberRequest> updateValidator)
    {
        _memberService = memberService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a member by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetByIdAsync(id, cancellationToken);
        
        if (member == null)
        {
            return NotFound(ApiResponse<MemberDto>.ErrorResponse($"Member with ID {id} not found"));
        }

        return Ok(ApiResponse<MemberDto>.SuccessResponse(member));
    }

    /// <summary>
    /// Search members with optional filters and pagination
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MemberDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] MemberSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _memberService.SearchAsync(request, cancellationToken);
        return Ok(ApiResponse<PagedResult<MemberDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new member
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMemberRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<MemberDto>.ErrorResponse("Validation failed", errors));
        }

        var member = await _memberService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById), 
            new { id = member.MemberId }, 
            ApiResponse<MemberDto>.SuccessResponse(member, "Member created successfully"));
    }

    /// <summary>
    /// Update an existing member
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        if (id != request.MemberId)
        {
            return BadRequest(ApiResponse<MemberDto>.ErrorResponse("ID mismatch between route and body"));
        }

        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<MemberDto>.ErrorResponse("Validation failed", errors));
        }

        try
        {
            var member = await _memberService.UpdateAsync(request, cancellationToken);
            return Ok(ApiResponse<MemberDto>.SuccessResponse(member, "Member updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<MemberDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Soft delete a member
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _memberService.DeleteAsync(id, cancellationToken);
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Check if a member exists
    /// </summary>
    [HttpHead("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken)
    {
        var exists = await _memberService.ExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }
}
