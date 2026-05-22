using MedManage.Core.DTOs.CaseLetterNote;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseLetterNoteService
{
    Task<IEnumerable<CaseLetterNoteDto>> GetAllAsync();
    Task<CaseLetterNoteDto?> GetByCaseIdAsync(int caseId);
    Task<CaseLetterNoteDto> CreateAsync(CreateCaseLetterNoteDto dto);
    Task<CaseLetterNoteDto> UpdateAsync(int caseId, UpdateCaseLetterNoteDto dto);
    Task<bool> DeleteAsync(int caseId);
}
