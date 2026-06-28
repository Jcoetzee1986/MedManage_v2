using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IFacilityTypeRepository : IRepository<FacilityType>
{
    Task<IEnumerable<FacilityType>> GetActiveAsync();
}
