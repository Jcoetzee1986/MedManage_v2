using System.Collections.Generic;
using System.Threading.Tasks;
using MedManage.Core.DTOs.EpisodeCase;

namespace MedManage.Core.Interfaces.Services;

public interface IEpisodeCaseService
{
    Task<IEnumerable<EpisodeCaseDto>> GetAllAsync();
    Task<EpisodeCaseDto?> GetByIdAsync(int episodeId, int caseId);
    Task<IEnumerable<EpisodeCaseDto>> GetByEpisodeIdAsync(int episodeId);
    Task<IEnumerable<EpisodeCaseDto>> GetByCaseIdAsync(int caseId);
    Task<EpisodeCaseDto> CreateAsync(CreateEpisodeCaseDto dto);
    Task<EpisodeCaseDto> UpdateAsync(int episodeId, int caseId, UpdateEpisodeCaseDto dto);
    Task<bool> DeleteAsync(int episodeId, int caseId);
}
