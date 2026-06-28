using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IGenderRepository : IRepository<Gender>
{
    Task<IEnumerable<Gender>> GetActiveAsync();
}
