using MedManage.Core.DTOs.CaseExclusion;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

/// <summary>
/// Case Exclusions API — CRUD (ExclusionID, Comment)
/// </summary>
[ApiController]
[Route("api/cases/{caseId}/exclusions")]
[Authorize]
public class CaseExclusionController : ControllerBase
{
    private readonly ICaseExclusionService _service;

    public CaseExclusionController(ICaseExclusionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all exclusions for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseExclusionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var exclusions = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<IEnumerable<CaseExclusionDto>>.SuccessResponse(exclusions));
    }

    /// <summary>
    /// Get a specific exclusion by ExclusionID within a case
    /// </summary>
    [HttpGet("{exclusionId}")]
    [ProducesResponseType(typeof(ApiResponse<CaseExclusionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseExclusionDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int exclusionId)
    {
        var exclusion = await _service.GetByIdAsync(caseId, exclusionId);
        if (exclusion == null)
            return NotFound(ApiResponse<CaseExclusionDto>.ErrorResponse($"Case exclusion not found for case {caseId}, exclusion {exclusionId}"));

        return Ok(ApiResponse<CaseExclusionDto>.SuccessResponse(exclusion));
    }

    /// <summary>
    /// Add an exclusion to a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseExclusionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseExclusionDto dto)
    {
        // Ensure the CaseId from route is used
        dto.CaseId = caseId;

        var exclusion = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { caseId = exclusion.CaseId, exclusionId = exclusion.ExclusionId },
            ApiResponse<CaseExclusionDto>.SuccessResponse(exclusion));
    }

    /// <summary>
    /// Update an exclusion comment
    /// </summary>
    [HttpPut("{exclusionId}")]
    [ProducesResponseType(typeof(ApiResponse<CaseExclusionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseExclusionDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, int exclusionId, [FromBody] UpdateCaseExclusionDto dto)
    {
        try
        {
            var exclusion = await _service.UpdateAsync(caseId, exclusionId, dto);
            return Ok(ApiResponse<CaseExclusionDto>.SuccessResponse(exclusion));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseExclusionDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Remove an exclusion from a case (soft delete)
    /// </summary>
    [HttpDelete("{exclusionId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int exclusionId)
    {
        var result = await _service.DeleteAsync(caseId, exclusionId);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Case exclusion not found for case {caseId}, exclusion {exclusionId}"));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Case exclusion deleted successfully"));
    }
}
