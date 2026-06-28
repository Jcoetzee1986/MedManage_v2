using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface IFacilityTypeService
{
    Task<IEnumerable<FacilityTypeDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<FacilityTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FacilityTypeDto> CreateAsync(CreateFacilityTypeDto dto, CancellationToken cancellationToken = default);
    Task<FacilityTypeDto> UpdateAsync(UpdateFacilityTypeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
