using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseStatusRepository : Repository<CaseStatus>, ICaseStatusRepository
{
    public CaseStatusRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseStatus>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.CaseStatus1)
            .ToListAsync();
    }

    public async Task<CaseStatus?> GetByDescriptionAsync(string description)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.CaseStatus1 == description && x.DateDeleted == null);
    }
}
