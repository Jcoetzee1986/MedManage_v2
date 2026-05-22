using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChecklistTemplateController : ControllerBase
{
    private readonly IChecklistTemplateService _service;

    public ChecklistTemplateController(IChecklistTemplateService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ChecklistTemplateDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ChecklistTemplateDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ChecklistTemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ChecklistTemplateDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<ChecklistTemplateDto>.ErrorResponse($"Checklist template with ID {id} not found"));
        return Ok(ApiResponse<ChecklistTemplateDto>.SuccessResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ChecklistTemplateDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateChecklistTemplateDto dto, CancellationToken cancellationToken)
    {
        var item = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.ChecklistTemplateId }, ApiResponse<ChecklistTemplateDto>.SuccessResponse(item));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ChecklistTemplateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateChecklistTemplateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.ChecklistTemplateId)
            return BadRequest(ApiResponse<ChecklistTemplateDto>.ErrorResponse("ID mismatch"));
        
        try
        {
            var item = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<ChecklistTemplateDto>.SuccessResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<ChecklistTemplateDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Checklist template with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
