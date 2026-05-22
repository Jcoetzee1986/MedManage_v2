using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class BaseTariffRepository : Repository<BaseTariff>, IBaseTariffRepository
{
    public BaseTariffRepository(MedManageDbContext context) : base(context)
    {
    }

    public new async Task<IEnumerable<BaseTariff>> GetAllAsync()
    {
        return await _dbSet
            .Where(bt => bt.DateDeleted == null)
            .OrderBy(bt => bt.TariffDescription)
            .ToListAsync();
    }

    public async Task<BaseTariff?> GetNewCustomCodeAsync()
    {
        // Note: CustomCode and BaseTariffCode properties don't exist on BaseTariff entity
        // This method needs to be redesigned based on actual entity schema
        // For now, returning the tariff with the highest BaseTariffId
        var maxId = await _dbSet
            .Where(bt => bt.DateDeleted == null)
            .OrderByDescending(bt => bt.BaseTariffId)
            .Select(bt => bt.BaseTariffId)
            .FirstOrDefaultAsync();

        if (maxId == null)
        {
            return null;
        }

        // Return a new entity with the next ID (client will set appropriate ID)
        return new BaseTariff { BaseTariffId = maxId }; // Client needs to increment
    }

    public async Task<int> InsertCustomAsync(BaseTariff baseTariff)
    {
        // Note: CustomCode property doesn't exist on BaseTariff entity
        // Inserting tariff without setting CustomCode flag
        baseTariff.DateInserted = DateTime.Now;
        await _dbSet.AddAsync(baseTariff);
        await _context.SaveChangesAsync();
        return 1; // Return success (BaseTariffId is string, can't return it as int)
    }
}
