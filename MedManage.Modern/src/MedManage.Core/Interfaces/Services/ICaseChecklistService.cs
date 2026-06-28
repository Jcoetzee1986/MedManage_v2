using System.Collections.Generic;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseChecklist;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseChecklistService
{
    Task<IEnumerable<CaseChecklistDto>> GetAllAsync();
    Task<CaseChecklistDto?> GetByIdAsync(int caseId, int checklistTemplateId);
    Task<IEnumerable<CaseChecklistDto>> GetByCaseIdAsync(int caseId);
    Task<CaseChecklistDto> CreateAsync(CreateCaseChecklistDto dto);
    Task<CaseChecklistDto> UpdateAsync(int caseId, int checklistTemplateId, UpdateCaseChecklistDto dto);
    Task<bool> DeleteAsync(int caseId, int checklistTemplateId);
}
