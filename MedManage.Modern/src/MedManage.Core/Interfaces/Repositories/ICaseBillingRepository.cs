using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseBillingRepository : IRepository<CaseBilling>
{
    Task<IEnumerable<CaseBilling>> GetByCaseIdAsync(int caseId);
    Task<IEnumerable<CaseBilling>> GetNotLinkedToCaseAsync();
    Task<IEnumerable<CaseBilling>> GetByServiceProviderIdAsync(int serviceProviderId);
    Task<IEnumerable<string>> GetAccountNumbersByServiceProviderIdAsync(int serviceProviderId);
    Task<IEnumerable<CaseBilling>> GetAfterAutoCompleteAsync(string searchCriteria);
    Task<CaseBilling?> GetSummaryAsync(int caseBillingId);
    Task<bool> CheckDuplicatesAsync(int caseId, string invoiceNumber);
    Task LinkToCaseAsync(int caseBillingId, int caseId);
    Task UpdateToPaidAsync(int caseBillingId, string remittanceNumber);
    Task UpdateRemittanceNumberAsync(int caseBillingId, string remittanceNumber);
}
