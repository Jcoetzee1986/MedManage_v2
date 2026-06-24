using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IServiceProviderDiscountRepository : IRepository<ServiceProviderMainClientDiscount>
{
    Task<IEnumerable<ServiceProviderMainClientDiscount>> GetByServiceProviderIdAsync(int serviceProviderId);
    Task<ServiceProviderMainClientDiscount?> GetByProviderAndClientAsync(int serviceProviderId, int mainClientId);
}
