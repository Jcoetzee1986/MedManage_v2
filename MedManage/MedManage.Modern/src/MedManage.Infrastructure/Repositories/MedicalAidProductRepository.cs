using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MedicalAidProductRepository : Repository<MedicalAidProduct>, IMedicalAidProductRepository
{
    public MedicalAidProductRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalAidProduct>> GetByMedicalAidIdAsync(int mainClientId)
    {
        // Products with matching MainClientId OR with NULL MainClientId (global/shared products)
        return await _dbSet
            .Where(p => (p.MainClientId == mainClientId || p.MainClientId == null) && p.DateDeleted == null)
            .OrderBy(p => p.MedAidProductName)
            .ToListAsync();
    }

    public async Task<IEnumerable<MedicalAidProduct>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.MedAidProductName)
            .ToListAsync();
    }
}
