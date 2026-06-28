using MedManage.Core.DTOs.CaseComment;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseCommentService
{
    Task<IEnumerable<CaseCommentDto>> GetAllAsync();
    Task<CaseCommentDto?> GetByIdAsync(int id);
    Task<IEnumerable<CaseCommentDto>> GetByCaseIdAsync(int caseId);
    Task<CaseCommentDto> CreateAsync(CreateCaseCommentDto dto);
    Task<CaseCommentDto> UpdateAsync(int id, UpdateCaseCommentDto dto);
    Task<bool> DeleteAsync(int id);
}
