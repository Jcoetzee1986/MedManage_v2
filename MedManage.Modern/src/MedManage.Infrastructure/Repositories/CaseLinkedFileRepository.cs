using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseLinkedFileRepository : Repository<CaseLinkedFile>, ICaseLinkedFileRepository
{
    public CaseLinkedFileRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseLinkedFile>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .ToListAsync();
    }

    public async Task DeleteByCaseLinkedFileIdAsync(int caseLinkedFileId)
    {
        var entity = await _dbSet.FindAsync(caseLinkedFileId);
        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }
}
