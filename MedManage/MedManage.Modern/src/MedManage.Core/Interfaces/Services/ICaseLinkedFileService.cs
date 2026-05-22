using MedManage.Core.DTOs.CaseLinkedFile;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseLinkedFileService
{
    Task<IEnumerable<CaseLinkedFileDto>> GetAllAsync();
    Task<CaseLinkedFileDto?> GetByIdAsync(int id);
    Task<IEnumerable<CaseLinkedFileDto>> GetByCaseIdAsync(int caseId);
    Task<IEnumerable<CaseLinkedFileDto>> GetByMemberIdAsync(int memberId);
    Task<CaseLinkedFileDto> CreateAsync(CreateCaseLinkedFileDto dto);
    Task<CaseLinkedFileDto> UpdateAsync(int id, UpdateCaseLinkedFileDto dto);
    Task<bool> DeleteAsync(int id);
}
