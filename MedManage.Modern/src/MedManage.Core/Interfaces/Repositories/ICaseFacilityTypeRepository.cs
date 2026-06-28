using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseFacilityTypeRepository : IRepository<CaseFacilityType>
{
    Task<IEnumerable<CaseFacilityType>> GetByCaseIdAsync(int caseId);
}
