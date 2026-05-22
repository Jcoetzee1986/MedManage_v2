using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberStatusController : ControllerBase
{
    private readonly IMemberStatusService _service;

    public MemberStatusController(IMemberStatusService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all member statuses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemberStatusDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MemberStatusDto>>.SuccessResponse(items));
    }

    /// <summary>
    /// Get a member status by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        
        if (item == null)
        {
            return NotFound(ApiResponse<MemberStatusDto>.ErrorResponse($"Member status with ID {id} not found"));
        }

        return Ok(ApiResponse<MemberStatusDto>.SuccessResponse(item));
    }

    /// <summary>
    /// Create a new member status
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMemberStatusDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MemberStatusDto>.ErrorResponse("Invalid request", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.MemberStatusId }, ApiResponse<MemberStatusDto>.SuccessResponse(item, "Member status created successfully"));
    }

    /// <summary>
    /// Update an existing member status
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<MemberStatusDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberStatusDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.MemberStatusId)
        {
            return BadRequest(ApiResponse<MemberStatusDto>.ErrorResponse("ID in URL does not match ID in request body"));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MemberStatusDto>.ErrorResponse("Invalid request", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<MemberStatusDto>.SuccessResponse(item, "Member status updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<MemberStatusDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a member status (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"Member status with ID {id} not found"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Member status deleted successfully"));
    }
}
