using MedManage.Core.DTOs.CaseNappi;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Case NAPPI code operations
/// </summary>
public interface ICaseNappiService
{
    Task<IEnumerable<CaseNappiDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<CaseNappiDto?> GetByIdAsync(int caseId, int id, CancellationToken cancellationToken = default);
    Task<CaseNappiDto> CreateAsync(int caseId, CreateCaseNappiDto dto, CancellationToken cancellationToken = default);
    Task<CaseNappiDto> UpdateAsync(int caseId, int id, UpdateCaseNappiDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int caseId, int id, CancellationToken cancellationToken = default);
}
