using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ServiceProviderTariffCustomRepository : Repository<ServiceProviderTariffCustom>, IServiceProviderTariffCustomRepository
{
    public ServiceProviderTariffCustomRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceProviderTariffCustom>> GetByServiceProviderIdAsync(int serviceProviderId)
    {
        // Note: ServiceProvider and BaseTariff navigations not scaffolded
        return await _dbSet
            .Where(sptc => sptc.ServiceProviderId == serviceProviderId && sptc.DateDeleted == null)
            .OrderBy(sptc => sptc.BaseTariffId)
            .ToListAsync();
    }

    public async Task InsertFromExcelAsync(ServiceProviderTariffCustom tariff)
    {
        tariff.DateInserted = DateTime.Now;
        await _dbSet.AddAsync(tariff);
        await _context.SaveChangesAsync();
    }
}
