using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.MemberNote;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberNoteController : ControllerBase
{
    private readonly IMemberNoteService _service;
    private readonly ILogger<MemberNoteController> _logger;

    public MemberNoteController(IMemberNoteService service, ILogger<MemberNoteController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MemberNoteDto>>.SuccessResponse(notes));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var note = await _service.GetByIdAsync(id, cancellationToken);
        
        if (note == null)
        {
            return NotFound(ApiResponse<MemberNoteDto>.ErrorResponse($"MemberNote with ID {id} not found"));
        }
        
        return Ok(ApiResponse<MemberNoteDto>.SuccessResponse(note));
    }

    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetByMemberId(int memberId, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _service.GetByMemberIdAsync(memberId, includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MemberNoteDto>>.SuccessResponse(notes));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MemberNoteDto>.ErrorResponse("Invalid model state", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }
        
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.MemberNoteId }, ApiResponse<MemberNoteDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<MemberNoteDto>.ErrorResponse("Invalid model state", 
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }
        
        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<MemberNoteDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<MemberNoteDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"MemberNote with ID {id} not found"));
        }
        
        return Ok(ApiResponse<bool>.SuccessResponse(true, "MemberNote deleted successfully"));
    }
}
