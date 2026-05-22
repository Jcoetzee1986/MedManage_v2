using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class NappiCodeRepository : Repository<NappiCode>, INappiCodeRepository
{
    public NappiCodeRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<NappiCode>> SearchAsync(string? nappiCode, string? description, DateTime? date)
    {
        var query = _dbSet.Where(x => x.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(nappiCode))
        {
            query = query.Where(x => x.Code != null && x.Code.Contains(nappiCode));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            query = query.Where(x => x.Description != null && x.Description.Contains(description));
        }

        // Note: NappiCode doesn't have a generic Date property - it has StartDate/EndDate
        if (date.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(date.Value);
            query = query.Where(x => (!x.StartDate.HasValue || x.StartDate <= dateOnly) && 
                                    (!x.EndDate.HasValue || x.EndDate >= dateOnly));
        }

        return await query
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task<NappiCode?> GetByNappiCodeAsync(string nappiCode)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Code == nappiCode && x.DateDeleted == null);
    }

    public async Task<IEnumerable<NappiCode>> GetActiveCodesAsync(DateTime effectiveDate)
    {
        var dateOnly = DateOnly.FromDateTime(effectiveDate);
        return await _dbSet
            .Where(x => x.DateDeleted == null && 
                       (!x.StartDate.HasValue || x.StartDate <= dateOnly) && 
                       (!x.EndDate.HasValue || x.EndDate >= dateOnly))
            .OrderBy(x => x.Code)
            .ToListAsync();
    }
}
