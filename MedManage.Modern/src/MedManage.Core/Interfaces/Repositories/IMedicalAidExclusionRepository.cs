using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMedicalAidExclusionRepository : IRepository<MedicalAidExclusion>
{
    Task<IEnumerable<MedicalAidExclusion>> GetByMedicalAidIdAsync(int medicalAidId);
    Task InsertBySpecialityAsync(int medicalAidId, int specialityId, int exclusionId);
    Task InsertByHospitalTypeAsync(int medicalAidId, int hospitalTypeId, int exclusionId);
}
