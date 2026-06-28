using System.Threading.Tasks;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Tariff;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TariffController : ControllerBase
{
    private readonly ITariffService _tariffService;

    public TariffController(ITariffService tariffService)
    {
        _tariffService = tariffService;
    }

    // --- Tariff Search (simple text search — no rate context) ---

    [HttpGet("search")]
    public async Task<IActionResult> SearchTariffs([FromQuery] string? q, [FromQuery] string? code, [FromQuery] string? description)
    {
        var query = q ?? code ?? "";
        if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(description))
            return Ok(ApiResponse<IEnumerable<BaseTariffDto>>.SuccessResponse(Enumerable.Empty<BaseTariffDto>()));

        var results = await _tariffService.SearchBaseTariffsAsync(code ?? q, description);
        return Ok(ApiResponse<IEnumerable<BaseTariffDto>>.SuccessResponse(results));
    }

    // --- Tariff Search with case context (returns rates via SP) ---

    [HttpGet("search-for-case/{caseId}")]
    public async Task<IActionResult> SearchTariffsForCase(int caseId, [FromQuery] string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 2)
            return Ok(ApiResponse<IEnumerable<object>>.SuccessResponse(Enumerable.Empty<object>()));

        var results = await _tariffService.LookupTariffForCaseAsync(caseId, code);
        return Ok(ApiResponse<IEnumerable<object>>.SuccessResponse(results));
    }

    // --- Tariff Lookup (wraps SP) ---

    [HttpPost("lookup")]
    public async Task<IActionResult> LookupTariff([FromBody] TariffLookupRequest request)
    {
        var result = await _tariffService.LookupTariffAsync(request);
        if (result == null)
            return NotFound(ApiResponse<object>.ErrorResponse("No tariff found for the given code, provider, and date"));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    // --- Case Tariff Calculation (wraps fn_sc_TotalTariffPerCase) ---

    [HttpGet("calculate/{caseId}")]
    public async Task<IActionResult> CalculateCaseTariff(int caseId)
    {
        var results = await _tariffService.CalculateCaseTariffAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(results));
    }

    // --- Base Tariff Management (CRUD) ---

    [HttpGet("base")]
    public async Task<IActionResult> GetAllBaseTariffs()
    {
        var tariffs = await _tariffService.GetAllBaseTariffsAsync();
        return Ok(ApiResponse<object>.SuccessResponse(tariffs));
    }

    [HttpGet("base/{id}")]
    public async Task<IActionResult> GetBaseTariffById(string id)
    {
        var tariff = await _tariffService.GetBaseTariffByIdAsync(id);
        if (tariff == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Base tariff not found"));

        return Ok(ApiResponse<object>.SuccessResponse(tariff));
    }

    [HttpPost("base")]
    public async Task<IActionResult> CreateBaseTariff([FromBody] CreateBaseTariffDto dto)
    {
        var tariff = await _tariffService.CreateBaseTariffAsync(dto);
        return CreatedAtAction(nameof(GetBaseTariffById), new { id = tariff.BaseTariffId },
            ApiResponse<object>.SuccessResponse(tariff));
    }

    [HttpPut("base/{id}")]
    public async Task<IActionResult> UpdateBaseTariff(string id, [FromBody] UpdateBaseTariffDto dto)
    {
        try
        {
            var tariff = await _tariffService.UpdateBaseTariffAsync(id, dto);
            return Ok(ApiResponse<object>.SuccessResponse(tariff));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("base/{id}")]
    public async Task<IActionResult> DeleteBaseTariff(string id)
    {
        var result = await _tariffService.DeleteBaseTariffAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Base tariff not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }

    // --- Tariff Rate/Period Management (CRUD) ---

    [HttpGet("rates")]
    public async Task<IActionResult> GetAllTariffRates()
    {
        var rates = await _tariffService.GetAllTariffRatesAsync();
        return Ok(ApiResponse<object>.SuccessResponse(rates));
    }

    [HttpGet("rates/by-base-tariff/{baseTariffId}")]
    public async Task<IActionResult> GetTariffRatesByBaseTariff(string baseTariffId)
    {
        var rates = await _tariffService.GetTariffRatesByBaseTariffIdAsync(baseTariffId);
        return Ok(ApiResponse<object>.SuccessResponse(rates));
    }

    [HttpGet("rates/{id}")]
    public async Task<IActionResult> GetTariffRateById(int id)
    {
        var rate = await _tariffService.GetTariffRateByIdAsync(id);
        if (rate == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Tariff rate not found"));

        return Ok(ApiResponse<object>.SuccessResponse(rate));
    }

    [HttpPost("rates")]
    public async Task<IActionResult> CreateTariffRate([FromBody] CreateTariffRateDto dto)
    {
        var rate = await _tariffService.CreateTariffRateAsync(dto);
        return CreatedAtAction(nameof(GetTariffRateById), new { id = rate.TariffId },
            ApiResponse<object>.SuccessResponse(rate));
    }

    [HttpPut("rates/{id}")]
    public async Task<IActionResult> UpdateTariffRate(int id, [FromBody] UpdateTariffRateDto dto)
    {
        try
        {
            var rate = await _tariffService.UpdateTariffRateAsync(id, dto);
            return Ok(ApiResponse<object>.SuccessResponse(rate));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("rates/{id}")]
    public async Task<IActionResult> DeleteTariffRate(int id)
    {
        var result = await _tariffService.DeleteTariffRateAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Tariff rate not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }

    // --- Tariff Name Management (CRUD) ---

    [HttpGet("names")]
    public async Task<IActionResult> GetAllTariffNames()
    {
        var names = await _tariffService.GetAllTariffNamesAsync();
        return Ok(ApiResponse<object>.SuccessResponse(names));
    }

    [HttpGet("names/{id}")]
    public async Task<IActionResult> GetTariffNameById(int id)
    {
        var name = await _tariffService.GetTariffNameByIdAsync(id);
        if (name == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Tariff name not found"));

        return Ok(ApiResponse<object>.SuccessResponse(name));
    }

    [HttpPost("names")]
    public async Task<IActionResult> CreateTariffName([FromBody] CreateTariffNameDto dto)
    {
        var name = await _tariffService.CreateTariffNameAsync(dto);
        return CreatedAtAction(nameof(GetTariffNameById), new { id = name.TariffNameId },
            ApiResponse<object>.SuccessResponse(name));
    }

    [HttpPut("names/{id}")]
    public async Task<IActionResult> UpdateTariffName(int id, [FromBody] UpdateTariffNameDto dto)
    {
        try
        {
            var name = await _tariffService.UpdateTariffNameAsync(id, dto);
            return Ok(ApiResponse<object>.SuccessResponse(name));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("names/{id}")]
    public async Task<IActionResult> DeleteTariffName(int id)
    {
        var result = await _tariffService.DeleteTariffNameAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Tariff name not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }
}
