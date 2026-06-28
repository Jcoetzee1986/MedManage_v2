using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseStatusService
{
    Task<IEnumerable<CaseStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<CaseStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CaseStatusDto> CreateAsync(CreateCaseStatusDto dto, CancellationToken cancellationToken = default);
    Task<CaseStatusDto> UpdateAsync(UpdateCaseStatusDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
