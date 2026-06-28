using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class RaceRepository : Repository<Race>, IRaceRepository
{
    public RaceRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Race>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.Race1)
            .ToListAsync();
    }
}
