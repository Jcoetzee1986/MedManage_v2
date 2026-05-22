using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMedicalAidRepository : IRepository<MedicalAid>
{
    Task<MedicalAid?> GetByIdWithDetailsAsync(int medicalAidId);
    Task<MedicalAid?> GetByMedicalAidIdAsync(int medicalAidId);
    Task<IEnumerable<MedicalAid>> GetActiveAsync();
}
