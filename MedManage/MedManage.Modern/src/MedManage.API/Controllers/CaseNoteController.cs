using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.CaseNote;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseNoteController : ControllerBase
{
    private readonly ICaseNoteService _service;
    private readonly ILogger<CaseNoteController> _logger;

    public CaseNoteController(ICaseNoteService service, ILogger<CaseNoteController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<CaseNoteDto>>.SuccessResponse(notes));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var note = await _service.GetByIdAsync(id, cancellationToken);
        
        if (note == null)
        {
            return NotFound(ApiResponse<CaseNoteDto>.ErrorResponse($"CaseNote with ID {id} not found"));
        }
        
        return Ok(ApiResponse<CaseNoteDto>.SuccessResponse(note));
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(int caseId, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _service.GetByCaseIdAsync(caseId, includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<CaseNoteDto>>.SuccessResponse(notes));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCaseNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CaseNoteDto>.ErrorResponse("Invalid model state", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }
        
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.CaseNoteId }, ApiResponse<CaseNoteDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CaseNoteDto>.ErrorResponse("Invalid model state", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }
        
        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<CaseNoteDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseNoteDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"CaseNote with ID {id} not found"));
        }
        
        return Ok(ApiResponse<bool>.SuccessResponse(true, "CaseNote deleted successfully"));
    }
}
