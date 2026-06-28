using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Report;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/report")]
[Authorize]
public class ReportGenerationController : ControllerBase
{
    private readonly IReportGenerationService _service;

    public ReportGenerationController(IReportGenerationService service)
    {
        _service = service;
    }

    [HttpPost("cases-between-dates")]
    public async Task<IActionResult> CasesBetweenDates([FromBody] CasesBetweenDatesRequest request, CancellationToken ct)
    {
        var result = await _service.GenerateCasesBetweenDatesAsync(request, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpPost("billing-summary")]
    public async Task<IActionResult> BillingSummary([FromBody] BillingSummaryRequest request, CancellationToken ct)
    {
        var result = await _service.GenerateBillingSummaryAsync(request, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpPost("tariff-detail")]
    public async Task<IActionResult> TariffDetail([FromBody] CaseTariffDetailRequest request, CancellationToken ct)
    {
        var result = await _service.GenerateCaseTariffDetailAsync(request, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpPost("my-cases")]
    public async Task<IActionResult> MyCases([FromBody] MyCasesRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
        var result = await _service.GenerateMyCasesAsync(request, userId, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpPost("case-comments")]
    public async Task<IActionResult> CaseComments([FromBody] CaseCommentsExportRequest request, CancellationToken ct)
    {
        var result = await _service.GenerateCaseCommentsExportAsync(request, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpPost("wip-extract")]
    public async Task<IActionResult> WipExtract([FromBody] WipExtractRequest request, CancellationToken ct)
    {
        var result = await _service.GenerateWipExtractAsync(request, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpPost("linked-cases")]
    public async Task<IActionResult> LinkedCases([FromBody] LinkedCasesRequest request, CancellationToken ct)
    {
        var result = await _service.GenerateLinkedCasesAsync(request, ct);
        return File(result.Content, result.ContentType, result.FileName);
    }
}
