using MedManage.Core.DTOs.Exclusion;

namespace MedManage.Core.Interfaces.Services;

public interface IExclusionService
{
    Task<IEnumerable<ExclusionDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExclusionDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<ExclusionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ExclusionDto> CreateAsync(CreateExclusionDto dto, CancellationToken cancellationToken = default);
    Task<ExclusionDto> UpdateAsync(UpdateExclusionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
