using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseLetterNoteRepository : Repository<CaseLetterNote>, ICaseLetterNoteRepository
{
    public CaseLetterNoteRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseLetterNote>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderByDescending(x => x.DateInserted)
            .ToListAsync();
    }
}
