using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMarritalStatusRepository : IRepository<MarritalStatus>
{
    Task<IEnumerable<MarritalStatus>> GetActiveAsync();
}
