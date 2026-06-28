using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChronicIllnessController : ControllerBase
{
    private readonly IChronicIllnessService _service;

    public ChronicIllnessController(IChronicIllnessService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ChronicIllnessDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ChronicIllnessDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ChronicIllnessDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ChronicIllnessDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(double? id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<ChronicIllnessDto>.ErrorResponse($"Chronic illness with ID {id} not found"));
        return Ok(ApiResponse<ChronicIllnessDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ChronicIllnessDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateChronicIllnessDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.ChronicIllnessId }, ApiResponse<ChronicIllnessDto>.SuccessResponse(item));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ChronicIllnessDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateChronicIllnessDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.ChronicIllnessId)
            return BadRequest(ApiResponse<ChronicIllnessDto>.ErrorResponse("ID mismatch"));
        
        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<ChronicIllnessDto>.SuccessResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<ChronicIllnessDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(double? id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Chronic illness with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
