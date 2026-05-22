using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ExclusionRepository : Repository<Exclusion>, IExclusionRepository
{
    public ExclusionRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Exclusion>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.ExclusionDescription)
            .ToListAsync();
    }
}
