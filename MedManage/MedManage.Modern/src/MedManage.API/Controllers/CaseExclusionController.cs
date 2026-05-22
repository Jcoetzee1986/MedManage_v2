using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseExclusion;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseExclusionController : ControllerBase
{
    private readonly ICaseExclusionService _service;

    public CaseExclusionController(ICaseExclusionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var exclusions = await _service.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(exclusions));
    }

    [HttpGet("case/{caseId}/exclusion/{exclusionId}")]
    public async Task<IActionResult> GetById(int caseId, int exclusionId)
    {
        var exclusion = await _service.GetByIdAsync(caseId, exclusionId);
        if (exclusion == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Case exclusion not found"));

        return Ok(ApiResponse<object>.SuccessResponse(exclusion));
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var exclusions = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(exclusions));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCaseExclusionDto dto)
    {
        var exclusion = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), 
            new { caseId = exclusion.CaseId, exclusionId = exclusion.ExclusionId },
            ApiResponse<object>.SuccessResponse(exclusion));
    }

    [HttpPut("case/{caseId}/exclusion/{exclusionId}")]
    public async Task<IActionResult> Update(int caseId, int exclusionId, [FromBody] UpdateCaseExclusionDto dto)
    {
        try
        {
            var exclusion = await _service.UpdateAsync(caseId, exclusionId, dto);
            return Ok(ApiResponse<object>.SuccessResponse(exclusion));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("case/{caseId}/exclusion/{exclusionId}")]
    public async Task<IActionResult> Delete(int caseId, int exclusionId)
    {
        var result = await _service.DeleteAsync(caseId, exclusionId);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Case exclusion not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }
}
