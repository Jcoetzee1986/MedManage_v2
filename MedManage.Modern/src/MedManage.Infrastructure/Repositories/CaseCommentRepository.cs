using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseCommentRepository : Repository<CaseComment>, ICaseCommentRepository
{
    public CaseCommentRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseComment>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderByDescending(x => x.DateInserted)
            .ToListAsync();
    }
}
