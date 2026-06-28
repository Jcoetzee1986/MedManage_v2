using MedManage.Core.DTOs.CaseFacilityType;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseFacilityTypeService
{
    Task<IEnumerable<CaseFacilityTypeDto>> GetByCaseIdAsync(int caseId);
    Task<CaseFacilityTypeDto?> GetByIdAsync(int id);
    Task<CaseFacilityTypeDto> CreateAsync(CreateCaseFacilityTypeRequest request);
    Task<CaseFacilityTypeDto> UpdateAsync(int id, UpdateCaseFacilityTypeRequest request);
    Task<bool> DeleteAsync(int id);
}
