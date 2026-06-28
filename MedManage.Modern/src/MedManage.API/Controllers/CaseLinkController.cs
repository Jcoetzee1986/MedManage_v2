using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseLink;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/links")]
[Authorize]
public class CaseLinkController : ControllerBase
{
    private readonly ICaseLinkService _caseLinkService;
    private readonly ILogger<CaseLinkController> _logger;

    public CaseLinkController(ICaseLinkService caseLinkService, ILogger<CaseLinkController> logger)
    {
        _caseLinkService = caseLinkService;
        _logger = logger;
    }

    /// <summary>
    /// Get all linked cases for a case (parent and child relationships)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LinkedCaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _caseLinkService.GetByCaseIdAsync(caseId, cancellationToken);
            return Ok(ApiResponse<IEnumerable<LinkedCaseDto>>.SuccessResponse(items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving links for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<LinkedCaseDto>>.ErrorResponse("An error occurred while retrieving case links"));
        }
    }

    /// <summary>
    /// Create a link between two cases (parent/child relationship)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseLinkDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseLinkDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseLinkDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _caseLinkService.CreateAsync(caseId, dto, cancellationToken);
            return CreatedAtAction(nameof(GetByCaseId), new { caseId }, ApiResponse<CaseLinkDto>.SuccessResponse(created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CaseLinkDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating link for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLinkDto>.ErrorResponse("An error occurred while creating the case link"));
        }
    }

    /// <summary>
    /// Remove a link between two cases
    /// </summary>
    [HttpDelete("{linkedCaseId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int linkedCaseId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _caseLinkService.DeleteAsync(caseId, linkedCaseId, cancellationToken);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"Link between case {caseId} and case {linkedCaseId} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Case link removed successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting link between case {CaseId} and case {LinkedCaseId}", caseId, linkedCaseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the case link"));
        }
    }
}
