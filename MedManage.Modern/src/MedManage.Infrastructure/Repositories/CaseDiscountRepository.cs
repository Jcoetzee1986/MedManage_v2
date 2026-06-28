using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseDiscountRepository : Repository<CaseDiscount>, ICaseDiscountRepository
{
    public CaseDiscountRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseDiscount>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .OrderBy(x => x.DateInserted)
            .ToListAsync();
    }
}
