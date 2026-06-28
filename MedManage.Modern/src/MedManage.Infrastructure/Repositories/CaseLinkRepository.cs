using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseLinkRepository : Repository<CaseLink>, ICaseLinkRepository
{
    public CaseLinkRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseLink>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => (x.ParentCase == caseId || x.ChildCase == caseId) && x.DateDeleted == null)
            .ToListAsync();
    }
}
