using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class TitleRepository : Repository<Title>, ITitleRepository
{
    public TitleRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Title>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.Title1)
            .ToListAsync();
    }
}
