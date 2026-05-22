using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface ISpecialityService
{
    Task<IEnumerable<SpecialityDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<SpecialityDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SpecialityDto> CreateAsync(CreateSpecialityDto dto, CancellationToken cancellationToken = default);
    Task<SpecialityDto> UpdateAsync(UpdateSpecialityDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
