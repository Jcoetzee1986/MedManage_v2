using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialityController : ControllerBase
{
    private readonly ISpecialityService _service;

    public SpecialityController(ISpecialityService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<SpecialityDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<SpecialityDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<SpecialityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SpecialityDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<SpecialityDto>.ErrorResponse($"Speciality with ID {id} not found"));
        return Ok(ApiResponse<SpecialityDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SpecialityDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSpecialityDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.SpecialityId }, ApiResponse<SpecialityDto>.SuccessResponse(item));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<SpecialityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialityDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.SpecialityId)
            return BadRequest(ApiResponse<SpecialityDto>.ErrorResponse("ID mismatch"));
        
        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<SpecialityDto>.SuccessResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<SpecialityDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Speciality with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
