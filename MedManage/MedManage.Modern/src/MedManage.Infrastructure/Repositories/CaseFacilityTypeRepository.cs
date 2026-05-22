using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseFacilityTypeRepository : Repository<CaseFacilityType>, ICaseFacilityTypeRepository
{
    public CaseFacilityTypeRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CaseFacilityType>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Include(x => x.FacilityType)
            .Where(x => x.CaseId == caseId && x.DateDeleted == null)
            .ToListAsync();
    }
}
