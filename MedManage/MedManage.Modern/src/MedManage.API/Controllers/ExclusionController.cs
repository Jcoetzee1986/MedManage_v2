using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExclusionController : ControllerBase
{
    private readonly IExclusionService _service;

    public ExclusionController(IExclusionService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExclusionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ExclusionDto>>.SuccessResponse(items));
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExclusionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var items = await _service.GetActiveAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<ExclusionDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<ExclusionDto>.ErrorResponse($"Exclusion with ID {id} not found"));
        return Ok(ApiResponse<ExclusionDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateExclusionDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ExclusionDto>.ErrorResponse("Invalid exclusion data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.ExclusionId }, ApiResponse<ExclusionDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ExclusionDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExclusionDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.ExclusionId)
            return BadRequest(ApiResponse<ExclusionDto>.ErrorResponse("ID mismatch"));

        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ExclusionDto>.ErrorResponse("Invalid exclusion data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var updated = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<ExclusionDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<ExclusionDto>.ErrorResponse($"Exclusion with ID {id} not found"));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Exclusion with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
