using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseCptRepository : Repository<CaseCpt>, ICaseCptRepository
{
    public CaseCptRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseCpt>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Include(x => x.Cpt)
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
