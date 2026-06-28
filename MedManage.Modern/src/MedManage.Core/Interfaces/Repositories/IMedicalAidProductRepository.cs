using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMedicalAidProductRepository : IRepository<MedicalAidProduct>
{
    Task<IEnumerable<MedicalAidProduct>> GetByMedicalAidIdAsync(int medicalAidId);
    Task<IEnumerable<MedicalAidProduct>> GetActiveAsync();
}
