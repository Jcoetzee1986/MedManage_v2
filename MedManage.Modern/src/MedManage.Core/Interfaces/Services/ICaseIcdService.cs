using MedManage.Core.DTOs.CaseIcd;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Case ICD code operations
/// </summary>
public interface ICaseIcdService
{
    Task<IEnumerable<CaseIcdDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<CaseIcdDto?> GetByIdAsync(int caseId, int icdId, CancellationToken cancellationToken = default);
    Task<CaseIcdDto> CreateAsync(int caseId, CreateCaseIcdDto dto, CancellationToken cancellationToken = default);
    Task<CaseIcdDto> UpdateAsync(int caseId, int icdId, UpdateCaseIcdDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int caseId, int icdId, CancellationToken cancellationToken = default);
}
