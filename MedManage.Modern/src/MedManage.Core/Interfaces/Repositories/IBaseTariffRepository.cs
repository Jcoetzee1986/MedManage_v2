using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IBaseTariffRepository : IRepository<BaseTariff>
{
    Task<BaseTariff?> GetNewCustomCodeAsync();
    Task<int> InsertCustomAsync(BaseTariff baseTariff);
}
