using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseTypeRepository : IRepository<CaseType>
{
    Task<IEnumerable<CaseType>> GetActiveAsync();
    Task<IEnumerable<CaseType>> GetForFiltersAsync();
}
