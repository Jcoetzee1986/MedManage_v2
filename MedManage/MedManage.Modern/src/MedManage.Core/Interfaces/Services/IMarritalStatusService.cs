using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface IMarritalStatusService
{
    Task<IEnumerable<MarritalStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<MarritalStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MarritalStatusDto> CreateAsync(CreateMarritalStatusDto dto, CancellationToken cancellationToken = default);
    Task<MarritalStatusDto> UpdateAsync(UpdateMarritalStatusDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
