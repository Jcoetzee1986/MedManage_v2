using MedManage.Core.DTOs.Report;

namespace MedManage.Core.Interfaces.Services;

public interface IReportGenerationService
{
    Task<ReportOutput> GenerateCasesBetweenDatesAsync(CasesBetweenDatesRequest request, CancellationToken cancellationToken = default);
    Task<ReportOutput> GenerateBillingSummaryAsync(BillingSummaryRequest request, CancellationToken cancellationToken = default);
    Task<ReportOutput> GenerateCaseTariffDetailAsync(CaseTariffDetailRequest request, CancellationToken cancellationToken = default);
    Task<ReportOutput> GenerateMyCasesAsync(MyCasesRequest request, string userId, CancellationToken cancellationToken = default);
    Task<ReportOutput> GenerateCaseCommentsExportAsync(CaseCommentsExportRequest request, CancellationToken cancellationToken = default);
    Task<ReportOutput> GenerateWipExtractAsync(WipExtractRequest request, CancellationToken cancellationToken = default);
    Task<ReportOutput> GenerateLinkedCasesAsync(LinkedCasesRequest request, CancellationToken cancellationToken = default);
}
