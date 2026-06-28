using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMemberStatusRepository : IRepository<MemberStatus>
{
    Task<IEnumerable<MemberStatus>> GetActiveAsync();
}
