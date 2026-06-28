using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.CaseNote;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// Case Notes API — CRUD with 8 interim amount categories
/// (Hospital, Radiology, Dialysis, Specialist, Physio, Transport, Accommodation, Script)
/// </summary>
[ApiController]
[Route("api/cases/{caseId}/notes")]
[Authorize]
public class CaseNoteController : ControllerBase
{
    private readonly ICaseNoteService _service;
    private readonly ILogger<CaseNoteController> _logger;

    public CaseNoteController(ICaseNoteService service, ILogger<CaseNoteController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all notes for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseNoteDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _service.GetByCaseIdAsync(caseId, includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<CaseNoteDto>>.SuccessResponse(notes));
    }

    /// <summary>
    /// Get a specific note by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var note = await _service.GetByIdAsync(id, cancellationToken);

        if (note == null || note.CaseId != caseId)
        {
            return NotFound(ApiResponse<CaseNoteDto>.ErrorResponse($"CaseNote with ID {id} not found for case {caseId}"));
        }

        return Ok(ApiResponse<CaseNoteDto>.SuccessResponse(note));
    }

    /// <summary>
    /// Create a new note for a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CaseNoteDto>.ErrorResponse("Invalid model state",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        // Ensure the CaseId from route is used
        dto.CaseId = caseId;

        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { caseId, id = created.CaseNoteId }, ApiResponse<CaseNoteDto>.SuccessResponse(created));
    }

    /// <summary>
    /// Update an existing note
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<CaseNoteDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int caseId, int id, [FromBody] UpdateCaseNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CaseNoteDto>.ErrorResponse("Invalid model state",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<CaseNoteDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseNoteDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a note (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"CaseNote with ID {id} not found"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "CaseNote deleted successfully"));
    }
}
