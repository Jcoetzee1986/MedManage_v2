using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseTariffRepository : Repository<CaseTariff>, ICaseTariffRepository
{
    public CaseTariffRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseTariff>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }

    public async Task<IEnumerable<CaseTariff>> GetForReportAsync(DateTime dateFrom, DateTime dateTo)
    {
        return await _dbSet
            .Where(x => x.DateInserted >= dateFrom && x.DateInserted <= dateTo && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
