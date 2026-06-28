using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedManage.Core.Configuration;
using MedManage.Core.DTOs.Report;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services;

/// <summary>
/// Report generation service that communicates with jsreport server
/// </summary>
public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;
    private readonly JsReportSettings _settings;
    private readonly ILogger<ReportService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public ReportService(
        HttpClient httpClient,
        IOptions<JsReportSettings> settings,
        ILogger<ReportService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<ReportResult> GenerateCaseLetterAsync(CaseLetterReportRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating case letter report for CaseId: {CaseId}", request.CaseId);

        var recipe = GetRecipe(request.Format);
        var payload = new JsReportRenderRequest
        {
            Template = new JsReportTemplate
            {
                Name = "case-letter",
                Recipe = recipe
            },
            Data = new
            {
                caseId = request.CaseId
            }
        };

        var result = await RenderReportAsync(payload, cancellationToken);
        result.FileName = $"CaseLetter_{request.CaseId}.{GetFileExtension(request.Format)}";
        result.ContentType = GetContentType(request.Format);

        return result;
    }

    public async Task<ReportResult> GenerateCasesBetweenDatesAsync(CasesBetweenDatesReportRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating cases between dates report: {DateFrom} to {DateTo}", request.DateFrom, request.DateTo);

        var recipe = GetRecipe(request.Format);
        var payload = new JsReportRenderRequest
        {
            Template = new JsReportTemplate
            {
                Name = "cases-between-dates",
                Recipe = recipe
            },
            Data = new
            {
                dateFrom = request.DateFrom,
                dateTo = request.DateTo,
                serviceProviderId = request.ServiceProviderId,
                caseStatusId = request.CaseStatusId
            }
        };

        var result = await RenderReportAsync(payload, cancellationToken);
        result.FileName = $"Cases_{request.DateFrom:yyyyMMdd}_{request.DateTo:yyyyMMdd}.{GetFileExtension(request.Format)}";
        result.ContentType = GetContentType(request.Format);

        return result;
    }

    public async Task<ReportResult> GenerateWipExtractAsync(WipExtractReportRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating WIP extract report");

        var recipe = GetRecipe(request.Format);
        var payload = new JsReportRenderRequest
        {
            Template = new JsReportTemplate
            {
                Name = "wip-extract",
                Recipe = recipe
            },
            Data = new
            {
                serviceProviderId = request.ServiceProviderId,
                mainClientId = request.MainClientId,
                asAtDate = request.AsAtDate ?? DateTime.UtcNow
            }
        };

        var result = await RenderReportAsync(payload, cancellationToken);
        var asAt = request.AsAtDate ?? DateTime.UtcNow;
        result.FileName = $"WIP_Extract_{asAt:yyyyMMdd}.{GetFileExtension(request.Format)}";
        result.ContentType = GetContentType(request.Format);

        return result;
    }

    public async Task<ReportResult> GenerateBillingSummaryAsync(BillingSummaryReportRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating billing summary report: {DateFrom} to {DateTo}", request.DateFrom, request.DateTo);

        var recipe = GetRecipe(request.Format);
        var payload = new JsReportRenderRequest
        {
            Template = new JsReportTemplate
            {
                Name = "billing-summary",
                Recipe = recipe
            },
            Data = new
            {
                dateFrom = request.DateFrom,
                dateTo = request.DateTo,
                serviceProviderId = request.ServiceProviderId,
                billingStatusId = request.BillingStatusId
            }
        };

        var result = await RenderReportAsync(payload, cancellationToken);
        result.FileName = $"BillingSummary_{request.DateFrom:yyyyMMdd}_{request.DateTo:yyyyMMdd}.{GetFileExtension(request.Format)}";
        result.ContentType = GetContentType(request.Format);

        return result;
    }

    /// <summary>
    /// Sends a render request to the jsreport API and returns the report bytes
    /// </summary>
    private async Task<ReportResult> RenderReportAsync(JsReportRenderRequest request, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(request, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/report", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("jsreport render failed with status {StatusCode}: {Error}",
                response.StatusCode, errorBody);
            throw new InvalidOperationException(
                $"Report generation failed. jsreport returned {(int)response.StatusCode}: {errorBody}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        return new ReportResult
        {
            Content = bytes
        };
    }

    private static string GetRecipe(ReportFormat format) => format switch
    {
        ReportFormat.Pdf => "chrome-pdf",
        ReportFormat.Excel => "html-to-xlsx",
        _ => "chrome-pdf"
    };

    private static string GetContentType(ReportFormat format) => format switch
    {
        ReportFormat.Pdf => "application/pdf",
        ReportFormat.Excel => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        _ => "application/pdf"
    };

    private static string GetFileExtension(ReportFormat format) => format switch
    {
        ReportFormat.Pdf => "pdf",
        ReportFormat.Excel => "xlsx",
        _ => "pdf"
    };

    #region jsreport API Models

    private class JsReportRenderRequest
    {
        public JsReportTemplate Template { get; set; } = new();
        public object? Data { get; set; }
    }

    private class JsReportTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Recipe { get; set; } = "chrome-pdf";
    }

    #endregion
}
