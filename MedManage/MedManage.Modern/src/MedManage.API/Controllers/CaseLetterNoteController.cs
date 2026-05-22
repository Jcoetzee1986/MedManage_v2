using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseLetterNote;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseLetterNoteController : ControllerBase
{
    private readonly ICaseLetterNoteService _caseLetterNoteService;
    private readonly ILogger<CaseLetterNoteController> _logger;

    public CaseLetterNoteController(ICaseLetterNoteService caseLetterNoteService, ILogger<CaseLetterNoteController> logger)
    {
        _caseLetterNoteService = caseLetterNoteService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseLetterNoteDto>>>> GetAll()
    {
        try
        {
            var caseLetterNotes = await _caseLetterNoteService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CaseLetterNoteDto>>.SuccessResponse(caseLetterNotes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all case letter notes");
            return StatusCode(500, ApiResponse<IEnumerable<CaseLetterNoteDto>>.ErrorResponse("An error occurred while retrieving case letter notes"));
        }
    }

    [HttpGet("case/{caseId}")]
    public async Task<ActionResult<ApiResponse<CaseLetterNoteDto>>> GetByCaseId(int caseId)
    {
        try
        {
            var caseLetterNote = await _caseLetterNoteService.GetByCaseIdAsync(caseId);
            if (caseLetterNote == null)
                return NotFound(ApiResponse<CaseLetterNoteDto>.ErrorResponse($"CaseLetterNote for Case ID {caseId} not found"));

            return Ok(ApiResponse<CaseLetterNoteDto>.SuccessResponse(caseLetterNote));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving case letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLetterNoteDto>.ErrorResponse("An error occurred while retrieving the case letter note"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CaseLetterNoteDto>>> Create([FromBody] CreateCaseLetterNoteDto dto)
    {
        try
        {
            var caseLetterNote = await _caseLetterNoteService.CreateAsync(dto);
            return Ok(ApiResponse<CaseLetterNoteDto>.SuccessResponse(caseLetterNote));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating case letter note");
            return StatusCode(500, ApiResponse<CaseLetterNoteDto>.ErrorResponse("An error occurred while creating the case letter note"));
        }
    }

    [HttpPut("case/{caseId}")]
    public async Task<ActionResult<ApiResponse<CaseLetterNoteDto>>> Update(int caseId, [FromBody] UpdateCaseLetterNoteDto dto)
    {
        try
        {
            var caseLetterNote = await _caseLetterNoteService.UpdateAsync(caseId, dto);
            return Ok(ApiResponse<CaseLetterNoteDto>.SuccessResponse(caseLetterNote));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseLetterNoteDto>.ErrorResponse($"CaseLetterNote for Case ID {caseId} not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating case letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLetterNoteDto>.ErrorResponse("An error occurred while updating the case letter note"));
        }
    }

    [HttpDelete("case/{caseId}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int caseId)
    {
        try
        {
            var result = await _caseLetterNoteService.DeleteAsync(caseId);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"CaseLetterNote for Case ID {caseId} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "CaseLetterNote deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting case letter note for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the case letter note"));
        }
    }
}
