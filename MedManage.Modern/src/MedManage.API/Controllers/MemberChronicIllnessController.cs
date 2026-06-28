using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.MemberChronicIllness;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/members/{memberId}/chronic-illness")]
[Authorize]
public class MemberChronicIllnessController : ControllerBase
{
    private readonly IMemberChronicIllnessService _service;
    private readonly ILogger<MemberChronicIllnessController> _logger;

    public MemberChronicIllnessController(IMemberChronicIllnessService service, ILogger<MemberChronicIllnessController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all chronic illnesses for a member
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemberChronicIllnessDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMemberId(int memberId, CancellationToken cancellationToken)
    {
        var items = await _service.GetByMemberIdAsync(memberId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MemberChronicIllnessDto>>.SuccessResponse(items));
    }

    /// <summary>
    /// Assign a chronic illness to a member
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MemberChronicIllnessDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MemberChronicIllnessDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int memberId, [FromBody] CreateMemberChronicIllnessDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(memberId, dto, cancellationToken);
            return CreatedAtAction(nameof(GetByMemberId), new { memberId }, ApiResponse<MemberChronicIllnessDto>.SuccessResponse(created));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<MemberChronicIllnessDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Remove a chronic illness from a member (soft delete)
    /// </summary>
    [HttpDelete("{chronicIllnessId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int memberId, int chronicIllnessId, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(memberId, chronicIllnessId, cancellationToken);

        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse($"Chronic illness assignment not found for member {memberId}"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Chronic illness removed from member successfully"));
    }
}
