using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseNappiCodeRepository : Repository<CaseNappiCode>, ICaseNappiCodeRepository
{
    public CaseNappiCodeRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseNappiCode>> GetByCaseIdAsync(int caseId)
    {
        // Note: NappiCode navigation not scaffolded
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
