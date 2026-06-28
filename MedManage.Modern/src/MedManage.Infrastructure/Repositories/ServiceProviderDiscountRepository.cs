using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ServiceProviderDiscountRepository : Repository<ServiceProviderMainClientDiscount>, IServiceProviderDiscountRepository
{
    public ServiceProviderDiscountRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceProviderMainClientDiscount>> GetByServiceProviderIdAsync(int serviceProviderId)
    {
        return await _dbSet
            .Where(d => d.ServiceProviderId == serviceProviderId && d.DateDeleted == null)
            .OrderBy(d => d.MainClientId)
            .ToListAsync();
    }

    public async Task<ServiceProviderMainClientDiscount?> GetByProviderAndClientAsync(int serviceProviderId, int mainClientId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(d => d.ServiceProviderId == serviceProviderId 
                                   && d.MainClientId == mainClientId 
                                   && d.DateDeleted == null);
    }
}
