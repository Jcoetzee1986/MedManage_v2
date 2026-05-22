using MedManage.Core.DTOs.Episode;

namespace MedManage.Core.Interfaces.Services;

public interface IEpisodeService
{
    Task<IEnumerable<EpisodeDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<EpisodeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EpisodeDto?> GetByIdWithCasesAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EpisodeDto>> SearchAsync(EpisodeSearchFilters filters, CancellationToken cancellationToken = default);
    Task<EpisodeDto> CreateAsync(CreateEpisodeDto dto, CancellationToken cancellationToken = default);
    Task<EpisodeDto> UpdateAsync(UpdateEpisodeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
