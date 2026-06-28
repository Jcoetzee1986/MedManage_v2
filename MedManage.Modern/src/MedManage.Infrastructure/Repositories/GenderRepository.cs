using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class GenderRepository : Repository<Gender>, IGenderRepository
{
    public GenderRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Gender>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.GenderDescription)
            .ToListAsync();
    }
}
