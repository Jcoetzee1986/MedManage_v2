using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.MemberNote;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/members/{memberId}/notes")]
[Authorize]
public class MemberNoteController : ControllerBase
{
    private readonly IMemberNoteService _service;
    private readonly ILogger<MemberNoteController> _logger;

    public MemberNoteController(IMemberNoteService service, ILogger<MemberNoteController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all notes for a member
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemberNoteDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMemberId(int memberId, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _service.GetByMemberIdAsync(memberId, includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MemberNoteDto>>.SuccessResponse(notes));
    }

    /// <summary>
    /// Get a specific member note by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberNoteDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int memberId, int id, CancellationToken cancellationToken = default)
    {
        var note = await _service.GetByIdAsync(id, cancellationToken);
        
        if (note == null)
        {
            return NotFound(ApiResponse<MemberNoteDto>.ErrorResponse($"MemberNote with ID {id} not found"));
        }
        
        return Ok(ApiResponse<MemberNoteDto>.SuccessResponse(note));
    }

    /// <summary>
    /// Create a new note for a member
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MemberNoteDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MemberNoteDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int memberId, [FromBody] CreateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MemberNoteDto>.ErrorResponse("Invalid model state", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        // Ensure the note is associated with the correct member from the route
        dto.MemberId = memberId;
        
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { memberId, id = created.MemberNoteId }, ApiResponse<MemberNoteDto>.SuccessResponse(created));
    }

    /// <summary>
    /// Update a member note
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberNoteDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int memberId, int id, [FromBody] UpdateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MemberNoteDto>.ErrorResponse("Invalid model state", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }
        
        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<MemberNoteDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<MemberNoteDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a member note (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int memberId, int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"MemberNote with ID {id} not found"));
        }
        
        return Ok(ApiResponse<bool>.SuccessResponse(true, "MemberNote deleted successfully"));
    }
}
