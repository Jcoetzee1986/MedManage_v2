using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class EpisodeCaseRepository : Repository<EpisodeCase>, IEpisodeCaseRepository
{
    public EpisodeCaseRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EpisodeCase>> GetByEpisodeIdAsync(int episodeId)
    {
        // Note: Navigation properties not scaffolded - query joins manually if needed
        return await _dbSet
            .Where(x => x.EpisodeId == episodeId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
