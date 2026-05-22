using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
{
    public EpisodeRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<Episode?> GetByIdWithCasesAsync(int episodeId)
    {
        // Note: EpisodeCases navigation not scaffolded - query EpisodeCase table separately if needed
        return await _dbSet
            .FirstOrDefaultAsync(e => e.EpisodeId == episodeId && e.DateDeleted == null);
    }

    public async Task<IEnumerable<Episode>> SearchByFiltersAsync(
        string? episodeName,
        int? memberId,
        DateTime? dateFrom,
        DateTime? dateTo)
    {
        var query = _dbSet
            .Where(e => e.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(episodeName))
        {
            query = query.Where(e => e.EpisodeDescription != null && e.EpisodeDescription.Contains(episodeName));
        }

        if (memberId.HasValue)
        {
            query = query.Where(e => e.MemberId == memberId.Value);
        }

        if (dateFrom.HasValue)
        {
            query = query.Where(e => e.DateCreated >= DateOnly.FromDateTime(dateFrom.Value));
        }

        if (dateTo.HasValue)
        {
            query = query.Where(e => e.DateCreated <= DateOnly.FromDateTime(dateTo.Value));
        }

        return await query
            .OrderByDescending(e => e.DateCreated)
            .ToListAsync();
    }
}
