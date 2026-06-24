using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.MemberMedicalAidProduct;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/members/{memberId}/medical-aid-products")]
[Authorize]
public class MemberMedicalAidProductController : ControllerBase
{
    private readonly IMemberMedicalAidProductService _service;
    private readonly ILogger<MemberMedicalAidProductController> _logger;

    public MemberMedicalAidProductController(IMemberMedicalAidProductService service, ILogger<MemberMedicalAidProductController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get medical aid product history for a member
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemberMedicalAidProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMemberId(int memberId, CancellationToken cancellationToken)
    {
        var items = await _service.GetByMemberIdAsync(memberId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MemberMedicalAidProductDto>>.SuccessResponse(items));
    }

    /// <summary>
    /// Get a specific medical aid product history record
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberMedicalAidProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberMedicalAidProductDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int memberId, int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
        {
            return NotFound(ApiResponse<MemberMedicalAidProductDto>.ErrorResponse($"Medical aid product record with ID {id} not found"));
        }
        return Ok(ApiResponse<MemberMedicalAidProductDto>.SuccessResponse(item));
    }

    /// <summary>
    /// Create a new medical aid product history record for a member
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MemberMedicalAidProductDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(int memberId, [FromBody] CreateMemberMedicalAidProductDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(memberId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { memberId, id = created.MedAidProductIdMemberId }, ApiResponse<MemberMedicalAidProductDto>.SuccessResponse(created));
    }

    /// <summary>
    /// Update a medical aid product history record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MemberMedicalAidProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberMedicalAidProductDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int memberId, int id, [FromBody] UpdateMemberMedicalAidProductDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<MemberMedicalAidProductDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<MemberMedicalAidProductDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete a medical aid product history record (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int memberId, int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"Medical aid product record with ID {id} not found"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Medical aid product record deleted successfully"));
    }
}
