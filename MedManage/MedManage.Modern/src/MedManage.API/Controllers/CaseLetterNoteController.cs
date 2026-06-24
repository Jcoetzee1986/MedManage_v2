using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseLetterNote;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/letter-notes")]
[Authorize]
public class CaseLetterNoteController : ControllerBase
{
    private readonly ICaseLetterNoteService _caseLetterNoteService;
    private readonly ILogger<CaseLetterNoteController> _logger;

    public CaseLetterNoteController(ICaseLetterNoteService caseLetterNoteService, ILogger<CaseLetterNoteController> logger)
    {
        _caseLetterNoteService = caseLetterNoteService;
        _logger = logger;
    }

    /// <summary>
    /// Get the letter note for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CaseLetterNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseLetterNoteDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        try
        {
            var caseLetterNote = await _caseLetterNoteService.GetByCaseIdAsync(caseId);
            if (caseLetterNote == null)
                return NotFound(ApiResponse<CaseLetterNoteDto>.ErrorResponse($"Letter note for case {caseId} not found"));

            return Ok(ApiResponse<CaseLetterNoteDto>.SuccessResponse(caseLetterNote));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLetterNoteDto>.ErrorResponse("An error occurred while retrieving the letter note"));
        }
    }

    /// <summary>
    /// Create a letter note for a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseLetterNoteDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseLetterNoteDto dto)
    {
        try
        {
            dto.CaseId = caseId;
            var caseLetterNote = await _caseLetterNoteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByCaseId), new { caseId }, ApiResponse<CaseLetterNoteDto>.SuccessResponse(caseLetterNote));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLetterNoteDto>.ErrorResponse("An error occurred while creating the letter note"));
        }
    }

    /// <summary>
    /// Update the letter note for a case
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<CaseLetterNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseLetterNoteDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, [FromBody] UpdateCaseLetterNoteDto dto)
    {
        try
        {
            var caseLetterNote = await _caseLetterNoteService.UpdateAsync(caseId, dto);
            return Ok(ApiResponse<CaseLetterNoteDto>.SuccessResponse(caseLetterNote));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseLetterNoteDto>.ErrorResponse($"Letter note for case {caseId} not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLetterNoteDto>.ErrorResponse("An error occurred while updating the letter note"));
        }
    }

    /// <summary>
    /// Delete the letter note for a case
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId)
    {
        try
        {
            var result = await _caseLetterNoteService.DeleteAsync(caseId);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"Letter note for case {caseId} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Letter note deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the letter note"));
        }
    }
}
