using MedManage.Core.DTOs.CaseChecklist;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

/// <summary>
/// Case Checklist API — CRUD from template (Checked, NotApplicable, Comments, Date)
/// </summary>
[ApiController]
[Route("api/cases/{caseId}/checklist")]
[Authorize]
public class CaseChecklistController : ControllerBase
{
    private readonly ICaseChecklistService _service;

    public CaseChecklistController(ICaseChecklistService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all checklist items for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseChecklistDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var checklists = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<IEnumerable<CaseChecklistDto>>.SuccessResponse(checklists));
    }

    /// <summary>
    /// Get a specific checklist item by template ID
    /// </summary>
    [HttpGet("{checklistTemplateId}")]
    [ProducesResponseType(typeof(ApiResponse<CaseChecklistDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseChecklistDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int checklistTemplateId)
    {
        var checklist = await _service.GetByIdAsync(caseId, checklistTemplateId);
        if (checklist == null)
            return NotFound(ApiResponse<CaseChecklistDto>.ErrorResponse($"Case checklist not found for case {caseId}, template {checklistTemplateId}"));

        return Ok(ApiResponse<CaseChecklistDto>.SuccessResponse(checklist));
    }

    /// <summary>
    /// Create a new checklist item for a case (from template)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseChecklistDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseChecklistDto dto)
    {
        // Ensure the CaseId from route is used
        dto.CaseId = caseId;

        var checklist = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { caseId = checklist.CaseId, checklistTemplateId = checklist.ChecklistTemplateId },
            ApiResponse<CaseChecklistDto>.SuccessResponse(checklist));
    }

    /// <summary>
    /// Update a checklist item (Checked, NotApplicable, Comments, Date)
    /// </summary>
    [HttpPut("{checklistTemplateId}")]
    [ProducesResponseType(typeof(ApiResponse<CaseChecklistDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseChecklistDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, int checklistTemplateId, [FromBody] UpdateCaseChecklistDto dto)
    {
        try
        {
            var checklist = await _service.UpdateAsync(caseId, checklistTemplateId, dto);
            return Ok(ApiResponse<CaseChecklistDto>.SuccessResponse(checklist));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseChecklistDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a checklist item (soft delete)
    /// </summary>
    [HttpDelete("{checklistTemplateId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int checklistTemplateId)
    {
        var result = await _service.DeleteAsync(caseId, checklistTemplateId);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Case checklist not found for case {caseId}, template {checklistTemplateId}"));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Case checklist item deleted successfully"));
    }
}
