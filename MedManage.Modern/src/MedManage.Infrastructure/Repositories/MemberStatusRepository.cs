using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MemberStatusRepository : Repository<MemberStatus>, IMemberStatusRepository
{
    public MemberStatusRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MemberStatus>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.MemberStatus1)
            .ToListAsync();
    }
}
