using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class SessionUserCaseRepository : Repository<SessionUserCase>, ISessionUserCaseRepository
{
    public SessionUserCaseRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SessionUserCase>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .ToListAsync();
    }
}
