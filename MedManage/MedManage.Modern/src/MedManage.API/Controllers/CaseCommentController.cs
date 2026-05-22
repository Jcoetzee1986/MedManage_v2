using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseComment;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseCommentController : ControllerBase
{
    private readonly ICaseCommentService _caseCommentService;
    private readonly ILogger<CaseCommentController> _logger;

    public CaseCommentController(ICaseCommentService caseCommentService, ILogger<CaseCommentController> logger)
    {
        _caseCommentService = caseCommentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseCommentDto>>>> GetAll()
    {
        try
        {
            var caseComments = await _caseCommentService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CaseCommentDto>>.SuccessResponse(caseComments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all case comments");
            return StatusCode(500, ApiResponse<IEnumerable<CaseCommentDto>>.ErrorResponse("An error occurred while retrieving case comments"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CaseCommentDto>>> GetById(int id)
    {
        try
        {
            var caseComment = await _caseCommentService.GetByIdAsync(id);
            if (caseComment == null)
                return NotFound(ApiResponse<CaseCommentDto>.ErrorResponse($"CaseComment with ID {id} not found"));

            return Ok(ApiResponse<CaseCommentDto>.SuccessResponse(caseComment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving case comment {Id}", id);
            return StatusCode(500, ApiResponse<CaseCommentDto>.ErrorResponse("An error occurred while retrieving the case comment"));
        }
    }

    [HttpGet("case/{caseId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseCommentDto>>>> GetByCaseId(int caseId)
    {
        try
        {
            var caseComments = await _caseCommentService.GetByCaseIdAsync(caseId);
            return Ok(ApiResponse<IEnumerable<CaseCommentDto>>.SuccessResponse(caseComments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving comments for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseCommentDto>>.ErrorResponse("An error occurred while retrieving case comments"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CaseCommentDto>>> Create([FromBody] CreateCaseCommentDto dto)
    {
        try
        {
            var caseComment = await _caseCommentService.CreateAsync(dto);
            return Ok(ApiResponse<CaseCommentDto>.SuccessResponse(caseComment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating case comment");
            return StatusCode(500, ApiResponse<CaseCommentDto>.ErrorResponse("An error occurred while creating the case comment"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CaseCommentDto>>> Update(int id, [FromBody] UpdateCaseCommentDto dto)
    {
        try
        {
            var caseComment = await _caseCommentService.UpdateAsync(id, dto);
            return Ok(ApiResponse<CaseCommentDto>.SuccessResponse(caseComment));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseCommentDto>.ErrorResponse($"CaseComment with ID {id} not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating case comment {Id}", id);
            return StatusCode(500, ApiResponse<CaseCommentDto>.ErrorResponse("An error occurred while updating the case comment"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var result = await _caseCommentService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"CaseComment with ID {id} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "CaseComment deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting case comment {Id}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while dleting the case comment"));
        }
    }
}
