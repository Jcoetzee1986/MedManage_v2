using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseService
{
    Task<CaseDto?> GetByIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<PagedResult<CaseDto>> SearchAsync(CaseSearchRequest request, CancellationToken cancellationToken = default);
    Task<CaseDto> CreateAsync(CreateCaseRequest request, CancellationToken cancellationToken = default);
    Task<CaseDto> UpdateAsync(UpdateCaseRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int caseId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int caseId, CancellationToken cancellationToken = default);
    Task<DuplicateCheckResult> CheckDuplicateAsync(DuplicateCheckRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<CaseDto>> GetMyCasesAsync(string userId, CancellationToken cancellationToken = default);
}
