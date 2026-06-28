using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseChecklistRepository : Repository<CaseChecklist>, ICaseChecklistRepository
{
    public CaseChecklistRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseChecklist>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
