using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IServiceProviderTariffRepository : IRepository<ServiceProviderTariff>
{
    Task<ServiceProviderTariff?> GetByIdWithDetailsAsync(int serviceProviderTariffId);
    Task<IEnumerable<ServiceProviderTariff>> GetByServiceProviderIdAsync(int serviceProviderId);
}
