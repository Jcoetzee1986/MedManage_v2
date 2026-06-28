using MedManage.Core.DTOs.CaseFacilityType;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/facility-types")]
[Authorize]
public class CaseFacilityTypeController : ControllerBase
{
    private readonly ICaseFacilityTypeService _service;

    public CaseFacilityTypeController(ICaseFacilityTypeService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all facility types for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseFacilityTypeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var facilityTypes = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<IEnumerable<CaseFacilityTypeDto>>.SuccessResponse(facilityTypes));
    }

    /// <summary>
    /// Get a specific facility type record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseFacilityTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseFacilityTypeDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int id)
    {
        var facilityType = await _service.GetByIdAsync(id);
        if (facilityType == null || facilityType.CaseId != caseId)
            return NotFound(ApiResponse<CaseFacilityTypeDto>.ErrorResponse("Case facility type not found"));

        return Ok(ApiResponse<CaseFacilityTypeDto>.SuccessResponse(facilityType));
    }

    /// <summary>
    /// Add a facility type to a case
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseFacilityTypeDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CaseFacilityTypeDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseFacilityTypeRequest request)
    {
        if (request.CaseId != caseId)
            request.CaseId = caseId;

        var facilityType = await _service.CreateAsync(request);
        return CreatedAtAction(
            nameof(GetById),
            new { caseId, id = facilityType.CaseIdFacilityTypeId },
            ApiResponse<CaseFacilityTypeDto>.SuccessResponse(facilityType, "Case facility type created successfully"));
    }

    /// <summary>
    /// Update a facility type record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseFacilityTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseFacilityTypeDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int caseId, int id, [FromBody] UpdateCaseFacilityTypeRequest request)
    {
        try
        {
            var facilityType = await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<CaseFacilityTypeDto>.SuccessResponse(facilityType, "Case facility type updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseFacilityTypeDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a facility type record (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Case facility type not found"));

        return NoContent();
    }
}
