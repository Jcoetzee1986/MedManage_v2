using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.MedicalAid;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalAidController : ControllerBase
{
    private readonly IMedicalAidService _service;

    public MedicalAidController(IMedicalAidService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidDto>>.SuccessResponse(items));
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var items = await _service.GetActiveAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<MedicalAidDto>.ErrorResponse($"Medical aid with ID {id} not found"));
        return Ok(ApiResponse<MedicalAidDto>.SuccessResponse(item));
    }

    [HttpGet("{id}/with-details")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithDetails(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdWithDetailsAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<MedicalAidDto>.ErrorResponse($"Medical aid with ID {id} not found"));
        return Ok(ApiResponse<MedicalAidDto>.SuccessResponse(item));
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] MedicalAidSearchFilters filters, CancellationToken cancellationToken)
    {
        var items = await _service.SearchAsync(filters, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidDto>>.SuccessResponse(items));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMedicalAidDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MedicalAidDto>.ErrorResponse("Invalid medical aid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.MedicalAidId }, ApiResponse<MedicalAidDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalAidDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.MedicalAidId)
            return BadRequest(ApiResponse<MedicalAidDto>.ErrorResponse("ID mismatch"));

        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MedicalAidDto>.ErrorResponse("Invalid medical aid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var updated = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<MedicalAidDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<MedicalAidDto>.ErrorResponse($"Medical aid with ID {id} not found"));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Medical aid with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
