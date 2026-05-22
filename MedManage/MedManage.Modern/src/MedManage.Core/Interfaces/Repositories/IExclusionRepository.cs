using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IExclusionRepository : IRepository<Exclusion>
{
    Task<IEnumerable<Exclusion>> GetActiveAsync();
}
