using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class IcdRepository : Repository<Icd>, IIcdRepository
{
    public IcdRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Icd>> SearchByFiltersAsync(string? code, string? description)
    {
        var query = _dbSet.Where(x => x.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(code))
        {
            query = query.Where(x => x.DiagnosisCode != null && x.DiagnosisCode.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            query = query.Where(x => x.DiagnosisDesc != null && x.DiagnosisDesc.Contains(description));
        }

        return await query
            .OrderBy(x => x.DiagnosisCode)
            .ToListAsync();
    }

    public async Task<Icd?> GetByIcdCodeAsync(string icdCode)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.DiagnosisCode == icdCode && x.DateDeleted == null);
    }

    public async Task<IEnumerable<Icd>> GetActiveCodesAsync()
    {
        // Note: ICD entity doesn't have Date property - returning all active codes
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.DiagnosisCode)
            .ToListAsync();
    }
}
