using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseCpt;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/cpt")]
[Authorize]
public class CaseCptController : ControllerBase
{
    private readonly ICaseCptService _caseCptService;
    private readonly ILogger<CaseCptController> _logger;

    public CaseCptController(ICaseCptService caseCptService, ILogger<CaseCptController> logger)
    {
        _caseCptService = caseCptService;
        _logger = logger;
    }

    /// <summary>
    /// Get all CPT codes for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseCptDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _caseCptService.GetByCaseIdAsync(caseId, cancellationToken);
            return Ok(ApiResponse<IEnumerable<CaseCptDto>>.SuccessResponse(items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving CPT codes for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseCptDto>>.ErrorResponse("An error occurred while retrieving CPT codes"));
        }
    }

    /// <summary>
    /// Get a specific CPT code assignment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseCptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseCptDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _caseCptService.GetByIdAsync(caseId, id, cancellationToken);
            if (item == null)
                return NotFound(ApiResponse<CaseCptDto>.ErrorResponse($"CPT code with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<CaseCptDto>.SuccessResponse(item));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving CPT code {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseCptDto>.ErrorResponse("An error occurred while retrieving the CPT code"));
        }
    }

    /// <summary>
    /// Add a CPT code to a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseCptDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseCptDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseCptDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _caseCptService.CreateAsync(caseId, dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { caseId, id = created.CaseIdCptid }, ApiResponse<CaseCptDto>.SuccessResponse(created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CaseCptDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating CPT code for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseCptDto>.ErrorResponse("An error occurred while creating the CPT code"));
        }
    }

    /// <summary>
    /// Update a CPT code assignment on a case
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseCptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseCptDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, int id, [FromBody] UpdateCaseCptDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _caseCptService.UpdateAsync(caseId, id, dto, cancellationToken);
            return Ok(ApiResponse<CaseCptDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseCptDto>.ErrorResponse($"CPT code with ID {id} not found for case {caseId}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating CPT code {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseCptDto>.ErrorResponse("An error occurred while updating the CPT code"));
        }
    }

    /// <summary>
    /// Remove a CPT code from a case (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _caseCptService.DeleteAsync(caseId, id, cancellationToken);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"CPT code with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "CPT code removed from case successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting CPT code {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the CPT code"));
        }
    }
}
