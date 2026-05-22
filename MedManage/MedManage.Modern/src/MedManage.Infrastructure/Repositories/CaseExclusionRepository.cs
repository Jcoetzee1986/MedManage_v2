using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseExclusionRepository : Repository<CaseExclusion>, ICaseExclusionRepository
{
    public CaseExclusionRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseExclusion>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Include(x => x.Exclusion)
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .ToListAsync();
    }
}
