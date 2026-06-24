using MedManage.Core.DTOs.CaseLink;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Case Link operations (parent/child relationships)
/// </summary>
public interface ICaseLinkService
{
    Task<IEnumerable<LinkedCaseDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<CaseLinkDto> CreateAsync(int caseId, CreateCaseLinkDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int caseId, int linkedCaseId, CancellationToken cancellationToken = default);
}
