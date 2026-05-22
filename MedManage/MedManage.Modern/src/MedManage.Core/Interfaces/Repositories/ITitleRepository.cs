using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ITitleRepository : IRepository<Title>
{
    Task<IEnumerable<Title>> GetActiveAsync();
}
