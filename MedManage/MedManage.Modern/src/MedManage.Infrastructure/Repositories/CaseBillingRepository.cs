using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseBillingRepository : Repository<CaseBilling>, ICaseBillingRepository
{
    public CaseBillingRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseBilling>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(cb => cb.CaseId == caseId && cb.DateDeleted == null)
            .OrderByDescending(cb => cb.DateInserted)
            .ToListAsync();
    }

    public async Task<IEnumerable<CaseBilling>> GetNotLinkedToCaseAsync()
    {
        return await _dbSet
            .Where(cb => cb.CaseId == null && cb.DateDeleted == null)
            .OrderByDescending(cb => cb.DateInserted)
            .ToListAsync();
    }

    public async Task<IEnumerable<CaseBilling>> GetByServiceProviderIdAsync(int serviceProviderId)
    {
        return await _dbSet
            .Where(cb => cb.ServiceProviderId == serviceProviderId && cb.DateDeleted == null)
            .OrderByDescending(cb => cb.DateInserted)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetAccountNumbersByServiceProviderIdAsync(int serviceProviderId)
    {
        return await _dbSet
            .Where(cb => cb.ServiceProviderId == serviceProviderId 
                      && cb.AccountNumber != null 
                      && cb.DateDeleted == null)
            .Select(cb => cb.AccountNumber)
            .Distinct()
            .OrderBy(an => an)
            .ToListAsync();
    }

    public async Task<IEnumerable<CaseBilling>> GetAfterAutoCompleteAsync(string searchCriteria)
    {
        // Note: ServiceProvider and Case navigations not scaffolded
        return await _dbSet
            .Where(cb => (cb.AccountNumber != null && cb.AccountNumber.Contains(searchCriteria))
                      || (cb.InvoiceNumber != null && cb.InvoiceNumber.Contains(searchCriteria))
                      && cb.DateDeleted == null)
            .Take(20)
            .ToListAsync();
    }

    public async Task<CaseBilling?> GetSummaryAsync(int caseBillingId)
    {
        // Note: Case and ServiceProvider navigations not scaffolded
        return await _dbSet
            .FirstOrDefaultAsync(cb => cb.CaseBillingId == caseBillingId && cb.DateDeleted == null);
    }

    public async Task<bool> CheckDuplicatesAsync(int caseId, string invoiceNumber)
    {
        return await _dbSet
            .AnyAsync(cb => cb.CaseId == caseId 
                         && cb.InvoiceNumber == invoiceNumber 
                         && cb.DateDeleted == null);
    }

    public async Task LinkToCaseAsync(int caseBillingId, int caseId)
    {
        var caseBilling = await _dbSet.FindAsync(caseBillingId);
        if (caseBilling != null)
        {
            caseBilling.CaseId = caseId;
            caseBilling.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateToPaidAsync(int caseBillingId, string remittanceNumber)
    {
        var caseBilling = await _dbSet.FindAsync(caseBillingId);
        if (caseBilling != null)
        {
            caseBilling.Paid = true;
            caseBilling.Remittance = remittanceNumber;
            caseBilling.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateRemittanceNumberAsync(int caseBillingId, string remittanceNumber)
    {
        var caseBilling = await _dbSet.FindAsync(caseBillingId);
        if (caseBilling != null)
        {
            caseBilling.Remittance = remittanceNumber;
            caseBilling.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
