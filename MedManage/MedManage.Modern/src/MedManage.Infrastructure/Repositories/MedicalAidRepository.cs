using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MedicalAidRepository : Repository<MedicalAid>, IMedicalAidRepository
{
    public MedicalAidRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<MedicalAid?> GetByIdWithDetailsAsync(int medicalAidId)
    {
        return await _dbSet
            .Include(ma => ma.MedicalAidTariffs)
            .Include(ma => ma.MedicalAidExclusions)
            .FirstOrDefaultAsync(ma => ma.MedicalAidId == medicalAidId && ma.DateDeleted == null);
    }

    public async Task<MedicalAid?> GetByMedicalAidIdAsync(int medicalAidId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ma => ma.MedicalAidId == medicalAidId && ma.DateDeleted == null);
    }

    public async Task<IEnumerable<MedicalAid>> GetActiveAsync()
    {
        return await _dbSet
            .Where(ma => ma.DateDeleted == null)
            .OrderBy(ma => ma.MedicalAidName)
            .ToListAsync();
    }
}
