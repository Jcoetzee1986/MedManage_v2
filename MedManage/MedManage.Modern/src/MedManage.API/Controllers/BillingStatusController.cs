using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillingStatusController : ControllerBase
{
    private readonly IBillingStatusService _service;

    public BillingStatusController(IBillingStatusService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BillingStatusDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<BillingStatusDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BillingStatusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BillingStatusDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<BillingStatusDto>.ErrorResponse($"Billing status with ID {id} not found"));
        return Ok(ApiResponse<BillingStatusDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BillingStatusDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBillingStatusDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.BillingStatusId }, ApiResponse<BillingStatusDto>.SuccessResponse(item));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BillingStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBillingStatusDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.BillingStatusId)
            return BadRequest(ApiResponse<BillingStatusDto>.ErrorResponse("ID mismatch"));
        
        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<BillingStatusDto>.SuccessResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<BillingStatusDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Billing status with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
