using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _service;

    public CountryController(ICountryService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CountryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<CountryDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CountryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CountryDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<CountryDto>.ErrorResponse($"Country with ID {id} not found"));
        return Ok(ApiResponse<CountryDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CountryDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCountryDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.CountryId }, ApiResponse<CountryDto>.SuccessResponse(item));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CountryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCountryDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.CountryId)
            return BadRequest(ApiResponse<CountryDto>.ErrorResponse("ID mismatch"));
        
        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<CountryDto>.SuccessResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CountryDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Country with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
