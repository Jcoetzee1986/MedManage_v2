using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Tariff;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// API controller for MainClient tariff configuration
/// </summary>
[ApiController]
[Route("api/main-client-tariffs")]
[Authorize]
public class MainClientTariffController : ControllerBase
{
    private readonly IMainClientTariffService _service;
    private readonly ILogger<MainClientTariffController> _logger;

    public MainClientTariffController(IMainClientTariffService service, ILogger<MainClientTariffController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get tariff configurations for a main client
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MainClientTariffDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMainClientId([FromQuery] int mainClientId)
    {
        try
        {
            var tariffs = await _service.GetByMainClientIdAsync(mainClientId);
            return Ok(ApiResponse<IEnumerable<MainClientTariffDto>>.SuccessResponse(tariffs));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tariff configs for main client {MainClientId}", mainClientId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving tariff configurations"));
        }
    }

    /// <summary>
    /// Create a new tariff configuration for a main client
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MainClientTariffDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateMainClientTariffDto dto)
    {
        try
        {
            var tariff = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByMainClientId), new { mainClientId = tariff.MainClientId },
                ApiResponse<MainClientTariffDto>.SuccessResponse(tariff));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tariff config for main client {MainClientId}", dto.MainClientId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the tariff configuration"));
        }
    }

    /// <summary>
    /// Delete a tariff configuration for a main client
    /// </summary>
    [HttpDelete("{mainClientId}/{tariffNameId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int mainClientId, int tariffNameId)
    {
        try
        {
            var result = await _service.DeleteAsync(mainClientId, tariffNameId);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Tariff configuration not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Tariff configuration deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tariff config {MainClientId}/{TariffNameId}", mainClientId, tariffNameId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the tariff configuration"));
        }
    }
}
