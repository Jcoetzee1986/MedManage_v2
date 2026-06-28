using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CptRepository : Repository<Cpt>, ICptRepository
{
    public CptRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cpt>> SearchByFiltersAsync(string? code, string? description, DateTime? effectiveDate)
    {
        var query = _dbSet.Where(x => x.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(code))
        {
            query = query.Where(x => x.Code != null && x.Code.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            query = query.Where(x => x.ShortDescr != null && x.ShortDescr.Contains(description));
        }

        // Note: Cpt entity doesn't have a Date property - removed effectiveDate filter
        // if (effectiveDate.HasValue)
        // {
        //     query = query.Where(x => x.Date <= effectiveDate.Value);
        // }

        return await query
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task<Cpt?> GetByCptCodeAsync(string cptCode)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Code == cptCode && x.DateDeleted == null);
    }

    public async Task<IEnumerable<Cpt>> GetActiveCodesAsync(DateTime effectiveDate)
    {
        // Note: Cpt entity doesn't have a Date property - returning all active codes
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.Code)
            .ToListAsync();
    }
}
