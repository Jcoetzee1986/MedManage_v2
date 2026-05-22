using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseIcdRepository : Repository<CaseIcd>, ICaseIcdRepository
{
    public CaseIcdRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseIcd>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Include(x => x.Icd)
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
