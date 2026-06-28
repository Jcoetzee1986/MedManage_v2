using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class BillingStatusRepository : Repository<BillingStatus>, IBillingStatusRepository
{
    public BillingStatusRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BillingStatus>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.BillingStatus1)
            .ToListAsync();
    }
}
