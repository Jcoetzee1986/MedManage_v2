using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CaseBillingController : ControllerBase
{
    private readonly ICaseBillingService _service;

    public CaseBillingController(ICaseBillingService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all billing records.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseBillingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var billings = await _service.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(billings));
    }

    /// <summary>
    /// Get a billing record by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseBillingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var billing = await _service.GetByIdAsync(id);
        if (billing == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Case billing not found"));

        return Ok(ApiResponse<object>.SuccessResponse(billing));
    }

    /// <summary>
    /// Get all billing records for a specific case.
    /// </summary>
    [HttpGet("case/{caseId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseBillingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var billings = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(billings));
    }

    /// <summary>
    /// Create a new billing record.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseBillingDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCaseBillingDto dto)
    {
        var billing = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = billing.CaseBillingId },
            ApiResponse<object>.SuccessResponse(billing));
    }

    /// <summary>
    /// Update an existing billing record.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseBillingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseBillingDto dto)
    {
        try
        {
            var billing = await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<object>.SuccessResponse(billing));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Soft-delete a billing record.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,BillingOfficer")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Case billing not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }

    /// <summary>
    /// Search billing records with filters: provider, dates, status, paid, remittance.
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CaseBillingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] BillingSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.SearchAsync(request, cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// Check for duplicate account numbers across billing records.
    /// </summary>
    [HttpPost("check-duplicate")]
    [ProducesResponseType(typeof(ApiResponse<DuplicateAccountCheckResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckDuplicate([FromBody] CheckDuplicateAccountRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.AccountNumber))
            return BadRequest(ApiResponse<object>.ErrorResponse("AccountNumber is required"));

        var result = await _service.CheckDuplicateAccountAsync(request.AccountNumber, request.ExcludeBillingId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// Get billing summary (totals, counts) for a specific case.
    /// </summary>
    [HttpGet("summary/{caseId}")]
    [ProducesResponseType(typeof(ApiResponse<BillingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBillingSummary(int caseId)
    {
        var summary = await _service.GetBillingSummaryAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(summary));
    }

    #region Payments and Remittance

    /// <summary>
    /// Mark one or more billing records as paid in bulk.
    /// </summary>
    [HttpPost("bulk-payment")]
    [Authorize(Roles = "Admin,BillingOfficer")]
    [ProducesResponseType(typeof(ApiResponse<BulkPaymentResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BulkPayment([FromBody] BulkPaymentRequest request, CancellationToken cancellationToken)
    {
        if (request.BillingIds == null || request.BillingIds.Count == 0)
            return BadRequest(ApiResponse<object>.ErrorResponse("At least one billing ID is required."));

        var result = await _service.BulkMarkAsPaidAsync(request, cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// Update remittance number on one or more billing records.
    /// </summary>
    [HttpPut("remittance")]
    [ProducesResponseType(typeof(ApiResponse<RemittanceUpdateResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRemittance([FromBody] RemittanceUpdateRequest request, CancellationToken cancellationToken)
    {
        if (request.BillingIds == null || request.BillingIds.Count == 0)
            return BadRequest(ApiResponse<object>.ErrorResponse("At least one billing ID is required."));

        if (string.IsNullOrWhiteSpace(request.RemittanceNumber))
            return BadRequest(ApiResponse<object>.ErrorResponse("RemittanceNumber is required."));

        var result = await _service.UpdateRemittanceAsync(request, cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// Get all billing records with a specific remittance number.
    /// </summary>
    [HttpGet("remittance/{remittanceNumber}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseBillingDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByRemittance(string remittanceNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(remittanceNumber))
            return BadRequest(ApiResponse<object>.ErrorResponse("Remittance number is required."));

        var billings = await _service.GetByRemittanceAsync(remittanceNumber, cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(billings));
    }

    #endregion
}
