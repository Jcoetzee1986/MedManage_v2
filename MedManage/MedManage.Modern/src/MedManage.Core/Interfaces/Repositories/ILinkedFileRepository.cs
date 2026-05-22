using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ILinkedFileRepository : IRepository<LinkedFile>
{
    Task<IEnumerable<LinkedFile>> GetByEntityIdAndTypeAsync(int entityId, string entityType);
    Task DeleteByLinkedFileIdAsync(int linkedFileId);
}
