using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MedicalAidTariffRepository : Repository<MedicalAidTariff>, IMedicalAidTariffRepository
{
    public MedicalAidTariffRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalAidTariff>> GetByMedicalAidIdAsync(int medicalAidId)
    {
        return await _dbSet
            .Include(mat => mat.TariffName)
            .Where(mat => mat.MedicalAidId == medicalAidId && mat.DateDeleted == null)
            .ToListAsync();
    }
}
