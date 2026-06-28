using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Report;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate a case letter report for a specific case.
    /// </summary>
    [HttpPost("case-letter")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateCaseLetter([FromBody] CaseLetterReportRequest request, CancellationToken cancellationToken)
    {
        if (request.CaseId <= 0)
            return BadRequest(ApiResponse<object>.ErrorResponse("CaseId is required and must be positive."));

        try
        {
            var result = await _reportService.GenerateCaseLetterAsync(request, cancellationToken);
            return File(result.Content, result.ContentType, result.FileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to generate case letter for CaseId: {CaseId}", request.CaseId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("Report generation failed. Please try again or contact support."));
        }
    }

    /// <summary>
    /// Generate a report of cases between specified dates.
    /// </summary>
    [HttpPost("cases-between-dates")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateCasesBetweenDates([FromBody] CasesBetweenDatesReportRequest request, CancellationToken cancellationToken)
    {
        if (request.DateFrom == default || request.DateTo == default)
            return BadRequest(ApiResponse<object>.ErrorResponse("DateFrom and DateTo are required."));

        if (request.DateFrom > request.DateTo)
            return BadRequest(ApiResponse<object>.ErrorResponse("DateFrom must be before DateTo."));

        try
        {
            var result = await _reportService.GenerateCasesBetweenDatesAsync(request, cancellationToken);
            return File(result.Content, result.ContentType, result.FileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to generate cases between dates report");
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("Report generation failed. Please try again or contact support."));
        }
    }

    /// <summary>
    /// Generate a WIP (Work In Progress) extract report.
    /// </summary>
    [HttpPost("wip-extract")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateWipExtract([FromBody] WipExtractReportRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _reportService.GenerateWipExtractAsync(request, cancellationToken);
            return File(result.Content, result.ContentType, result.FileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to generate WIP extract report");
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("Report generation failed. Please try again or contact support."));
        }
    }

    /// <summary>
    /// Generate a billing summary report.
    /// </summary>
    [HttpPost("billing-summary")]
    [Authorize(Roles = "System Administrator,Billing Auditing")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateBillingSummary([FromBody] BillingSummaryReportRequest request, CancellationToken cancellationToken)
    {
        if (request.DateFrom == default || request.DateTo == default)
            return BadRequest(ApiResponse<object>.ErrorResponse("DateFrom and DateTo are required."));

        if (request.DateFrom > request.DateTo)
            return BadRequest(ApiResponse<object>.ErrorResponse("DateFrom must be before DateTo."));

        try
        {
            var result = await _reportService.GenerateBillingSummaryAsync(request, cancellationToken);
            return File(result.Content, result.ContentType, result.FileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to generate billing summary report");
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("Report generation failed. Please try again or contact support."));
        }
    }
}
