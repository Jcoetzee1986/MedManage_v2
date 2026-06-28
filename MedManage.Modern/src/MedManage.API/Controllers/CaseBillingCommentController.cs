using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// API controller for billing record comments (sub-resource of CaseBilling)
/// </summary>
[ApiController]
[Route("api/casebilling/{billingId}/comments")]
[Authorize]
public class CaseBillingCommentController : ControllerBase
{
    private readonly ICaseBillingCommentService _service;
    private readonly ILogger<CaseBillingCommentController> _logger;

    public CaseBillingCommentController(ICaseBillingCommentService service, ILogger<CaseBillingCommentController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all comments for a billing record
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseBillingCommentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByBillingId(int billingId)
    {
        try
        {
            var comments = await _service.GetByBillingIdAsync(billingId);
            return Ok(ApiResponse<IEnumerable<CaseBillingCommentDto>>.SuccessResponse(comments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving comments for billing {BillingId}", billingId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving billing comments"));
        }
    }

    /// <summary>
    /// Add a comment to a billing record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseBillingCommentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(int billingId, [FromBody] CreateCaseBillingCommentDto dto)
    {
        try
        {
            var comment = await _service.CreateAsync(billingId, dto);
            return CreatedAtAction(nameof(GetByBillingId), new { billingId },
                ApiResponse<CaseBillingCommentDto>.SuccessResponse(comment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating billing comment for billing {BillingId}", billingId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the billing comment"));
        }
    }

    /// <summary>
    /// Delete a billing comment (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int billingId, int id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"Billing comment with ID {id} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Billing comment deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting billing comment {Id}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the billing comment"));
        }
    }
}
