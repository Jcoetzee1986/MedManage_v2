using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IRaceRepository : IRepository<Race>
{
    Task<IEnumerable<Race>> GetActiveAsync();
}
