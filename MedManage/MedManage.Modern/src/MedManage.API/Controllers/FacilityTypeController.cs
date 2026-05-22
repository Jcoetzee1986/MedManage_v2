using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacilityTypeController : ControllerBase
{
    private readonly IFacilityTypeService _service;

    public FacilityTypeController(IFacilityTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FacilityTypeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<FacilityTypeDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FacilityTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FacilityTypeDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<FacilityTypeDto>.ErrorResponse($"Facility type with ID {id} not found"));
        return Ok(ApiResponse<FacilityTypeDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FacilityTypeDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateFacilityTypeDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.FacilityTypeId }, ApiResponse<FacilityTypeDto>.SuccessResponse(item));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FacilityTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFacilityTypeDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.FacilityTypeId)
            return BadRequest(ApiResponse<FacilityTypeDto>.ErrorResponse("ID mismatch"));
        
        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<FacilityTypeDto>.SuccessResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<FacilityTypeDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Facility type with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
