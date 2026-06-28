using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseComment;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

/// <summary>
/// Case Comments API — simple CRUD (text, user, date)
/// </summary>
[ApiController]
[Route("api/cases/{caseId}/comments")]
[Authorize]
public class CaseCommentController : ControllerBase
{
    private readonly ICaseCommentService _caseCommentService;
    private readonly ILogger<CaseCommentController> _logger;

    public CaseCommentController(ICaseCommentService caseCommentService, ILogger<CaseCommentController> logger)
    {
        _caseCommentService = caseCommentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all comments for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseCommentDto>>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Get a specific comment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseCommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseCommentDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CaseCommentDto>>> GetById(int caseId, int id)
    {
        try
        {
            var caseComment = await _caseCommentService.GetByIdAsync(id);
            if (caseComment == null || caseComment.CaseId != caseId)
                return NotFound(ApiResponse<CaseCommentDto>.ErrorResponse($"CaseComment with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<CaseCommentDto>.SuccessResponse(caseComment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving case comment {Id}", id);
            return StatusCode(500, ApiResponse<CaseCommentDto>.ErrorResponse("An error occurred while retrieving the case comment"));
        }
    }

    /// <summary>
    /// Create a new comment for a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseCommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseCommentDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CaseCommentDto>>> Create(int caseId, [FromBody] CreateCaseCommentDto dto)
    {
        try
        {
            // Ensure the CaseId from route is used
            dto.CaseId = caseId;

            var caseComment = await _caseCommentService.CreateAsync(dto);
            return Ok(ApiResponse<CaseCommentDto>.SuccessResponse(caseComment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating case comment");
            return StatusCode(500, ApiResponse<CaseCommentDto>.ErrorResponse("An error occurred while creating the case comment"));
        }
    }

    /// <summary>
    /// Update an existing comment
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseCommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseCommentDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CaseCommentDto>>> Update(int caseId, int id, [FromBody] UpdateCaseCommentDto dto)
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

    /// <summary>
    /// Delete a comment (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int caseId, int id)
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
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the case comment"));
        }
    }
}
