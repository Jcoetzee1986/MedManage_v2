using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarritalStatusController : ControllerBase
{
    private readonly IMarritalStatusService _service;

    public MarritalStatusController(IMarritalStatusService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all marital statuses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MarritalStatusDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MarritalStatusDto>>.SuccessResponse(items));
    }

    /// <summary>
    /// Get a marital status by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        
        if (item == null)
        {
            return NotFound(ApiResponse<MarritalStatusDto>.ErrorResponse($"Marital status with ID {id} not found"));
        }

        return Ok(ApiResponse<MarritalStatusDto>.SuccessResponse(item));
    }

    /// <summary>
    /// Create a new marital status
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMarritalStatusDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MarritalStatusDto>.ErrorResponse("Invalid request", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.MarritalStatusId }, ApiResponse<MarritalStatusDto>.SuccessResponse(item, "Marital status created successfully"));
    }

    /// <summary>
    /// Update an existing marital status
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<MarritalStatusDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMarritalStatusDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.MarritalStatusId)
        {
            return BadRequest(ApiResponse<MarritalStatusDto>.ErrorResponse("ID in URL does not match ID in request body"));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MarritalStatusDto>.ErrorResponse("Invalid request", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<MarritalStatusDto>.SuccessResponse(item, "Marital status updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<MarritalStatusDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a marital status (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"Marital status with ID {id} not found"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Marital status deleted successfully"));
    }
}
