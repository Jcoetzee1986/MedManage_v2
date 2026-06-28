using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseNappi;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/nappi")]
[Authorize]
public class CaseNappiController : ControllerBase
{
    private readonly ICaseNappiService _caseNappiService;
    private readonly ILogger<CaseNappiController> _logger;

    public CaseNappiController(ICaseNappiService caseNappiService, ILogger<CaseNappiController> logger)
    {
        _caseNappiService = caseNappiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all NAPPI codes for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseNappiDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _caseNappiService.GetByCaseIdAsync(caseId, cancellationToken);
            return Ok(ApiResponse<IEnumerable<CaseNappiDto>>.SuccessResponse(items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving NAPPI codes for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseNappiDto>>.ErrorResponse("An error occurred while retrieving NAPPI codes"));
        }
    }

    /// <summary>
    /// Get a specific NAPPI code assignment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseNappiDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseNappiDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _caseNappiService.GetByIdAsync(caseId, id, cancellationToken);
            if (item == null)
                return NotFound(ApiResponse<CaseNappiDto>.ErrorResponse($"NAPPI code with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<CaseNappiDto>.SuccessResponse(item));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving NAPPI code {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseNappiDto>.ErrorResponse("An error occurred while retrieving the NAPPI code"));
        }
    }

    /// <summary>
    /// Add a NAPPI code to a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseNappiDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseNappiDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseNappiDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _caseNappiService.CreateAsync(caseId, dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { caseId, id = created.CaseIdNappiId }, ApiResponse<CaseNappiDto>.SuccessResponse(created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CaseNappiDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating NAPPI code for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseNappiDto>.ErrorResponse("An error occurred while creating the NAPPI code"));
        }
    }

    /// <summary>
    /// Update a NAPPI code assignment on a case
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseNappiDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseNappiDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, int id, [FromBody] UpdateCaseNappiDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _caseNappiService.UpdateAsync(caseId, id, dto, cancellationToken);
            return Ok(ApiResponse<CaseNappiDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseNappiDto>.ErrorResponse($"NAPPI code with ID {id} not found for case {caseId}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating NAPPI code {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseNappiDto>.ErrorResponse("An error occurred while updating the NAPPI code"));
        }
    }

    /// <summary>
    /// Remove a NAPPI code from a case (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _caseNappiService.DeleteAsync(caseId, id, cancellationToken);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"NAPPI code with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "NAPPI code removed from case successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting NAPPI code {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the NAPPI code"));
        }
    }
}
