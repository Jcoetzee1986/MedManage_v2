using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class LinkedFileRepository : Repository<LinkedFile>, ILinkedFileRepository
{
    public LinkedFileRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LinkedFile>> GetByEntityIdAndTypeAsync(int entityId, string entityType)
    {
        return await _dbSet
            .Where(x => x.EntityId == entityId && x.EntityType == entityType && x.DateDeleted == null)
            .OrderByDescending(x => x.DateInserted)
            .ToListAsync();
    }

    public async Task DeleteByLinkedFileIdAsync(int linkedFileId)
    {
        var entity = await _dbSet.FindAsync(linkedFileId);
        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }
}
