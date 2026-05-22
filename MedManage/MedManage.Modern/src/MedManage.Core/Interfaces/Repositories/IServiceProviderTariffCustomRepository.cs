using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IServiceProviderTariffCustomRepository : IRepository<ServiceProviderTariffCustom>
{
    Task<IEnumerable<ServiceProviderTariffCustom>> GetByServiceProviderIdAsync(int serviceProviderId);
    Task InsertFromExcelAsync(ServiceProviderTariffCustom tariff);
}
