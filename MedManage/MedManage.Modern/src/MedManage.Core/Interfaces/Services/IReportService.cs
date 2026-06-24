using MedManage.Core.DTOs.Report;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for generating reports via jsreport
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generate a case letter report for a specific case
    /// </summary>
    Task<ReportResult> GenerateCaseLetterAsync(CaseLetterReportRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a report of cases between specified dates
    /// </summary>
    Task<ReportResult> GenerateCasesBetweenDatesAsync(CasesBetweenDatesReportRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a WIP (Work In Progress) extract report
    /// </summary>
    Task<ReportResult> GenerateWipExtractAsync(WipExtractReportRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a billing summary report
    /// </summary>
    Task<ReportResult> GenerateBillingSummaryAsync(BillingSummaryReportRequest request, CancellationToken cancellationToken = default);
}
