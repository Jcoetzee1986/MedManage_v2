using MedManage.Core.DTOs.CaseCpt;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Case CPT code operations
/// </summary>
public interface ICaseCptService
{
    Task<IEnumerable<CaseCptDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<CaseCptDto?> GetByIdAsync(int caseId, int id, CancellationToken cancellationToken = default);
    Task<CaseCptDto> CreateAsync(int caseId, CreateCaseCptDto dto, CancellationToken cancellationToken = default);
    Task<CaseCptDto> UpdateAsync(int caseId, int id, UpdateCaseCptDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int caseId, int id, CancellationToken cancellationToken = default);
}
