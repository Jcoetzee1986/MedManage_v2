using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMedicalAidTariffRepository : IRepository<MedicalAidTariff>
{
    Task<IEnumerable<MedicalAidTariff>> GetByMedicalAidIdAsync(int medicalAidId);
}
