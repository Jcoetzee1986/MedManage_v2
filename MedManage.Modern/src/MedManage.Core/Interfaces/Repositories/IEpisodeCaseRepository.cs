using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IEpisodeCaseRepository : IRepository<EpisodeCase>
{
    Task<IEnumerable<EpisodeCase>> GetByEpisodeIdAsync(int episodeId);
}
