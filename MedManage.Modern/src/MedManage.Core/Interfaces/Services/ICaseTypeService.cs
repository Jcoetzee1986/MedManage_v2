using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseTypeService
{
    Task<IEnumerable<CaseTypeDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<CaseTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CaseTypeDto> CreateAsync(CreateCaseTypeDto dto, CancellationToken cancellationToken = default);
    Task<CaseTypeDto> UpdateAsync(UpdateCaseTypeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
