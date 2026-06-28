using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseIcd;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/icd")]
[Authorize]
public class CaseIcdController : ControllerBase
{
    private readonly ICaseIcdService _caseIcdService;
    private readonly ILogger<CaseIcdController> _logger;

    public CaseIcdController(ICaseIcdService caseIcdService, ILogger<CaseIcdController> logger)
    {
        _caseIcdService = caseIcdService;
        _logger = logger;
    }

    /// <summary>
    /// Get all ICD codes for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseIcdDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _caseIcdService.GetByCaseIdAsync(caseId, cancellationToken);
            return Ok(ApiResponse<IEnumerable<CaseIcdDto>>.SuccessResponse(items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ICD codes for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseIcdDto>>.ErrorResponse("An error occurred while retrieving ICD codes"));
        }
    }

    /// <summary>
    /// Get a specific ICD code assignment by ID
    /// </summary>
    [HttpGet("{icdId}")]
    [ProducesResponseType(typeof(ApiResponse<CaseIcdDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseIcdDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int icdId, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _caseIcdService.GetByIdAsync(caseId, icdId, cancellationToken);
            if (item == null)
                return NotFound(ApiResponse<CaseIcdDto>.ErrorResponse($"ICD code with ID {icdId} not found for case {caseId}"));

            return Ok(ApiResponse<CaseIcdDto>.SuccessResponse(item));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ICD code {IcdId} for case {CaseId}", icdId, caseId);
            return StatusCode(500, ApiResponse<CaseIcdDto>.ErrorResponse("An error occurred while retrieving the ICD code"));
        }
    }

    /// <summary>
    /// Add an ICD code to a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseIcdDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseIcdDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseIcdDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _caseIcdService.CreateAsync(caseId, dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { caseId, icdId = created.Icdid }, ApiResponse<CaseIcdDto>.SuccessResponse(created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CaseIcdDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ICD code for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseIcdDto>.ErrorResponse("An error occurred while creating the ICD code"));
        }
    }

    /// <summary>
    /// Update an ICD code assignment on a case
    /// </summary>
    [HttpPut("{icdId}")]
    [ProducesResponseType(typeof(ApiResponse<CaseIcdDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseIcdDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, int icdId, [FromBody] UpdateCaseIcdDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _caseIcdService.UpdateAsync(caseId, icdId, dto, cancellationToken);
            return Ok(ApiResponse<CaseIcdDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseIcdDto>.ErrorResponse($"ICD code with ID {icdId} not found for case {caseId}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ICD code {IcdId} for case {CaseId}", icdId, caseId);
            return StatusCode(500, ApiResponse<CaseIcdDto>.ErrorResponse("An error occurred while updating the ICD code"));
        }
    }

    /// <summary>
    /// Remove an ICD code from a case (soft delete)
    /// </summary>
    [HttpDelete("{icdId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int icdId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _caseIcdService.DeleteAsync(caseId, icdId, cancellationToken);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"ICD code with ID {icdId} not found for case {caseId}"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "ICD code removed from case successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ICD code {IcdId} for case {CaseId}", icdId, caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the ICD code"));
        }
    }
}
