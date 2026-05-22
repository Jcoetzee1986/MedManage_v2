using MedManage.Core.DTOs.CaseNote;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for CaseNote operations
/// </summary>
public interface ICaseNoteService
{
    Task<IEnumerable<CaseNoteDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<CaseNoteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CaseNoteDto>> GetByCaseIdAsync(int caseId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<CaseNoteDto> CreateAsync(CreateCaseNoteDto dto, CancellationToken cancellationToken = default);
    Task<CaseNoteDto> UpdateAsync(int id, UpdateCaseNoteDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
