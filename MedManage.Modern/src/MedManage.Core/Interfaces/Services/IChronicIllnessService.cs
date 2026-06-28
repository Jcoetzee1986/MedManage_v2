using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface IChronicIllnessService
{
    Task<IEnumerable<ChronicIllnessDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<ChronicIllnessDto?> GetByIdAsync(double? id, CancellationToken cancellationToken = default);
    Task<ChronicIllnessDto> CreateAsync(CreateChronicIllnessDto dto, CancellationToken cancellationToken = default);
    Task<ChronicIllnessDto> UpdateAsync(UpdateChronicIllnessDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(double? id, CancellationToken cancellationToken = default);
}
