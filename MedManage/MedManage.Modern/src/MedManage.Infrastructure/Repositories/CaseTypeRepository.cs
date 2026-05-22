using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseTypeRepository : Repository<CaseType>, ICaseTypeRepository
{
    public CaseTypeRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseType>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.CaseType1)
            .ToListAsync();
    }

    public async Task<IEnumerable<CaseType>> GetForFiltersAsync()
    {
        return await GetActiveAsync();
    }
}
