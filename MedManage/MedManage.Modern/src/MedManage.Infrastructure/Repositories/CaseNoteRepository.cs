using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseNoteRepository : Repository<CaseNote>, ICaseNoteRepository
{
    public CaseNoteRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseNote>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderByDescending(x => x.DateInserted)
            .ToListAsync();
    }

    public async Task<CaseNote?> GetLastNoteByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderByDescending(x => x.DateInserted)
            .FirstOrDefaultAsync();
    }
}
