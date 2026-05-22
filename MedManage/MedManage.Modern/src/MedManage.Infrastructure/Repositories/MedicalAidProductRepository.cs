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

    public async Task<IEnumerable<MedicalAidProduct>> GetByMedicalAidIdAsync(int medicalAidId)
    {
        // Note: MedicalAidProduct doesn't have MedicalAidId property - using MainClientId instead
        return await _dbSet
            .Where(p => p.MainClientId == medicalAidId && p.DateDeleted == null)
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
