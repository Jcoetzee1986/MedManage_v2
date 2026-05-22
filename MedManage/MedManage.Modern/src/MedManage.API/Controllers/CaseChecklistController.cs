using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseChecklist;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseChecklistController : ControllerBase
{
    private readonly ICaseChecklistService _service;

    public CaseChecklistController(ICaseChecklistService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var checklists = await _service.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(checklists));
    }

    [HttpGet("case/{caseId}/template/{checklistTemplateId}")]
    public async Task<IActionResult> GetById(int caseId, int checklistTemplateId)
    {
        var checklist = await _service.GetByIdAsync(caseId, checklistTemplateId);
        if (checklist == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Case checklist not found"));

        return Ok(ApiResponse<object>.SuccessResponse(checklist));
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var checklists = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(checklists));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCaseChecklistDto dto)
    {
        var checklist = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), 
            new { caseId = checklist.CaseId, checklistTemplateId = checklist.ChecklistTemplateId },
            ApiResponse<object>.SuccessResponse(checklist));
    }

    [HttpPut("case/{caseId}/template/{checklistTemplateId}")]
    public async Task<IActionResult> Update(int caseId, int checklistTemplateId, [FromBody] UpdateCaseChecklistDto dto)
    {
        try
        {
            var checklist = await _service.UpdateAsync(caseId, checklistTemplateId, dto);
            return Ok(ApiResponse<object>.SuccessResponse(checklist));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("case/{caseId}/template/{checklistTemplateId}")]
    public async Task<IActionResult> Delete(int caseId, int checklistTemplateId)
    {
        var result = await _service.DeleteAsync(caseId, checklistTemplateId);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Case checklist not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }
}
