using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IEpisodeRepository : IRepository<Episode>
{
    Task<Episode?> GetByIdWithCasesAsync (int episodeId);
    Task<IEnumerable<Episode>> SearchByFiltersAsync(
        string? episodeName,
        int? memberId,
        DateTime? dateFrom,
        DateTime? dateTo);
}
