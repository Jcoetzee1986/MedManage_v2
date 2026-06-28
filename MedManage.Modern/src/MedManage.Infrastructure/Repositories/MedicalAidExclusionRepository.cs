using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MedicalAidExclusionRepository : Repository<MedicalAidExclusion>, IMedicalAidExclusionRepository
{
    public MedicalAidExclusionRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalAidExclusion>> GetByMedicalAidIdAsync(int medicalAidId)
    {
        return await _dbSet
            .Include(mae => mae.BaseTariff)
            .Where(mae => mae.MedicalAidId == medicalAidId && mae.DateDeleted == null)
            .ToListAsync();
    }

    public async Task InsertBySpecialityAsync(int medicalAidId, int specialityId, int exclusionId)
    {
        // Note: MedicalAidExclusion entity has composite key (MedicalAidId, BaseTariffId)
        // SpecialityId and ExclusionId properties don't exist - need to map to BaseTariffId
        // This method needs to be redesigned based on actual schema requirements
        throw new NotImplementedException("MedicalAidExclusion schema doesn't support SpecialityId - use BaseTariffId instead");
    }

    public async Task InsertByHospitalTypeAsync(int medicalAidId, int hospitalTypeId, int exclusionId)
    {
        // Note: MedicalAidExclusion entity has composite key (MedicalAidId, BaseTariffId)
        // HospitalTypeId and ExclusionId properties don't exist - need to map to BaseTariffId
        // This method needs to be redesigned based on actual schema requirements
        throw new NotImplementedException("MedicalAidExclusion schema doesn't support HospitalTypeId - use BaseTariffId instead");
    }
}
