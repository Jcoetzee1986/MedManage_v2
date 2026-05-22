using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IBaseTariffRepository : IRepository<BaseTariff>
{
    Task<IEnumerable<BaseTariff>> GetAllAsync();
    Task<BaseTariff?> GetNewCustomCodeAsync();
    Task<int> InsertCustomAsync(BaseTariff baseTariff);
}
