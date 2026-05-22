using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MarritalStatusRepository : Repository<MarritalStatus>, IMarritalStatusRepository
{
    public MarritalStatusRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MarritalStatus>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.MarritalStatus1)
            .ToListAsync();
    }
}
