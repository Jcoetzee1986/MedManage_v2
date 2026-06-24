using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseTariff;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/tariffs")]
[Authorize]
public class CaseTariffController : ControllerBase
{
    private readonly ICaseTariffService _caseTariffService;
    private readonly ILogger<CaseTariffController> _logger;

    public CaseTariffController(ICaseTariffService caseTariffService, ILogger<CaseTariffController> logger)
    {
        _caseTariffService = caseTariffService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseTariffDto>>>> GetByCaseId(int caseId)
    {
        try
        {
            var tariffs = await _caseTariffService.GetByCaseIdAsync(caseId);
            return Ok(ApiResponse<IEnumerable<CaseTariffDto>>.SuccessResponse(tariffs));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tariffs for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseTariffDto>>.ErrorResponse("An error occurred while retrieving case tariffs"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CaseTariffDto>>> GetById(int caseId, long id)
    {
        try
        {
            var tariff = await _caseTariffService.GetByIdAsync(caseId, id);
            if (tariff == null)
                return NotFound(ApiResponse<CaseTariffDto>.ErrorResponse($"CaseTariff with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<CaseTariffDto>.SuccessResponse(tariff));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving case tariff {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseTariffDto>.ErrorResponse("An error occurred while retrieving the case tariff"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CaseTariffDto>>> Create(int caseId, [FromBody] CreateCaseTariffRequest request)
    {
        try
        {
            var tariff = await _caseTariffService.CreateAsync(caseId, request);
            return CreatedAtAction(nameof(GetById),
                new { caseId, id = tariff.CaseIdTariffId },
                ApiResponse<CaseTariffDto>.SuccessResponse(tariff));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating case tariff for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseTariffDto>.ErrorResponse("An error occurred while creating the case tariff"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CaseTariffDto>>> Update(int caseId, long id, [FromBody] UpdateCaseTariffRequest request)
    {
        try
        {
            var tariff = await _caseTariffService.UpdateAsync(caseId, id, request);
            return Ok(ApiResponse<CaseTariffDto>.SuccessResponse(tariff));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseTariffDto>.ErrorResponse($"CaseTariff with ID {id} not found for case {caseId}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating case tariff {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseTariffDto>.ErrorResponse("An error occurred while updating the case tariff"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int caseId, long id)
    {
        try
        {
            var result = await _caseTariffService.DeleteAsync(caseId, id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"CaseTariff with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Case tariff deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting case tariff {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the case tariff"));
        }
    }
}
