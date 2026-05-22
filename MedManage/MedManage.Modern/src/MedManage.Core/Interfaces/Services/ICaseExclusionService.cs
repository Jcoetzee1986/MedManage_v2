using System.Collections.Generic;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseExclusion;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseExclusionService
{
    Task<IEnumerable<CaseExclusionDto>> GetAllAsync();
    Task<CaseExclusionDto?> GetByIdAsync(int caseId, int exclusionId);
    Task<IEnumerable<CaseExclusionDto>> GetByCaseIdAsync(int caseId);
    Task<CaseExclusionDto> CreateAsync(CreateCaseExclusionDto dto);
    Task<CaseExclusionDto> UpdateAsync(int caseId, int exclusionId, UpdateCaseExclusionDto dto);
    Task<bool> DeleteAsync(int caseId, int exclusionId);
}
