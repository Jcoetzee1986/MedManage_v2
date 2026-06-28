using System.Threading;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/discounts")]
[Authorize]
public class CaseDiscountController : ControllerBase
{
    private readonly ICaseDiscountService _service;

    public CaseDiscountController(ICaseDiscountService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all discounts for a specific case.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseDiscountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, CancellationToken cancellationToken)
    {
        var discounts = await _service.GetByCaseIdAsync(caseId, cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(discounts));
    }

    /// <summary>
    /// Create a discount for a case.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseDiscountDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseDiscountDto dto, CancellationToken cancellationToken)
    {
        var discount = await _service.CreateAsync(caseId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetByCaseId), new { caseId },
            ApiResponse<object>.SuccessResponse(discount));
    }

    /// <summary>
    /// Delete a discount from a case by discount value.
    /// Since CaseDiscount is keyless (CaseID + Discount combination), we delete by discount value.
    /// </summary>
    [HttpDelete("{discount}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, decimal discount, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(caseId, discount, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse($"Discount {discount} not found for case {caseId}"));

        return Ok(ApiResponse<object>.SuccessResponse(null!, "Discount deleted successfully"));
    }
}
