using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ServiceProviderTariffRepository : Repository<ServiceProviderTariff>, IServiceProviderTariffRepository
{
    public ServiceProviderTariffRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<ServiceProviderTariff?> GetByIdWithDetailsAsync(int serviceProviderTariffId)
    {       
        // Note: ServiceProvider and TariffName navigations not scaffolded
        return await _dbSet
            .FirstOrDefaultAsync(spt => spt.ServiceProviderTariffId == serviceProviderTariffId 
                                     && spt.DateDeleted == null);
    }

    public async Task<IEnumerable<ServiceProviderTariff>> GetByServiceProviderIdAsync(int serviceProviderId)
    {
        // Note: TariffName navigation not scaffolded
        return await _dbSet
            .Where(spt => spt.ServiceProviderId == serviceProviderId && spt.DateDeleted == null)
            .OrderBy(spt => spt.TariffNameId)
            .ToListAsync();
    }
}
