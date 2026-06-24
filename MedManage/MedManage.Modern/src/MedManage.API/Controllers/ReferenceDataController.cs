using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// Generic base controller for reference data (lookup table) CRUD operations.
/// Concrete controllers inherit this and only need to specify the DTO types and route.
/// </summary>
/// <typeparam name="TDto">The read DTO type</typeparam>
/// <typeparam name="TCreateDto">The create request DTO type</typeparam>
/// <typeparam name="TUpdateDto">The update request DTO type</typeparam>
[ApiController]
[Authorize]
public abstract class ReferenceDataController<TDto, TCreateDto, TUpdateDto> : ControllerBase
{
    private readonly IReferenceDataService<TDto, TCreateDto, TUpdateDto> _service;
    private readonly string _entityDisplayName;

    protected ReferenceDataController(
        IReferenceDataService<TDto, TCreateDto, TUpdateDto> service,
        string entityDisplayName)
    {
        _service = service;
        _entityDisplayName = entityDisplayName;
    }

    /// <summary>
    /// Get all items
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<TDto>>.SuccessResponse(items));
    }

    /// <summary>
    /// Get item by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<TDto>.ErrorResponse($"{_entityDisplayName} with ID {id} not found"));
        return Ok(ApiResponse<TDto>.SuccessResponse(item));
    }

    /// <summary>
    /// Create a new item
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] TCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<TDto>.ErrorResponse("Invalid request", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = GetIdFromDto(item) }, 
            ApiResponse<TDto>.SuccessResponse(item, $"{_entityDisplayName} created successfully"));
    }

    /// <summary>
    /// Update an existing item
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] TUpdateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<TDto>.ErrorResponse("Invalid request",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var item = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<TDto>.SuccessResponse(item, $"{_entityDisplayName} updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<TDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete an item (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"{_entityDisplayName} with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, $"{_entityDisplayName} deleted successfully"));
    }

    /// <summary>
    /// Extract the ID from a DTO for the CreatedAtAction response.
    /// Override in concrete controllers if the ID property name differs.
    /// </summary>
    protected virtual int GetIdFromDto(TDto dto)
    {
        // Use reflection as a fallback; concrete controllers can override for performance
        var idProp = typeof(TDto).GetProperties()
            .FirstOrDefault(p => p.Name.EndsWith("Id") && p.PropertyType == typeof(int));
        return idProp != null ? (int)idProp.GetValue(dto)! : 0;
    }
}
