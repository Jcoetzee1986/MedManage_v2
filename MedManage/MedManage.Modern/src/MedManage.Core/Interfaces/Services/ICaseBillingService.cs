using System.Collections.Generic;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.Common;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseBillingService
{
    Task<IEnumerable<CaseBillingDto>> GetAllAsync();
    Task<CaseBillingDto?> GetByIdAsync(int id);
    Task<IEnumerable<CaseBillingDto>> GetByCaseIdAsync(int caseId);
    Task<CaseBillingDto> CreateAsync(CreateCaseBillingDto dto);
    Task<CaseBillingDto> UpdateAsync(int id, UpdateCaseBillingDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResult<CaseBillingDto>> SearchAsync(BillingSearchRequest request, CancellationToken cancellationToken = default);
    Task<DuplicateAccountCheckResult> CheckDuplicateAccountAsync(string accountNumber, int? excludeBillingId = null);
    Task<BillingSummaryDto> GetBillingSummaryAsync(int caseId);

    // Payments and Remittance
    Task<BulkPaymentResult> BulkMarkAsPaidAsync(BulkPaymentRequest request, CancellationToken cancellationToken = default);
    Task<RemittanceUpdateResult> UpdateRemittanceAsync(RemittanceUpdateRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<CaseBillingDto>> GetByRemittanceAsync(string remittanceNumber, CancellationToken cancellationToken = default);
    Task<BulkPaymentResult> ImportStatusUpdatesAsync(List<BillingStatusImportItem> items, CancellationToken cancellationToken = default);
}
