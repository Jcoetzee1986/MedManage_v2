using MedManage.Core.DTOs.Tariff;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class MainClientTariffService : IMainClientTariffService
{
    private readonly MedManageDbContext _context;

    public MainClientTariffService(MedManageDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MainClientTariffDto>> GetByMainClientIdAsync(int mainClientId)
    {
        var tariffs = await _context.MainClientTariffs
            .Where(t => t.MainClientId == mainClientId && t.DateDeleted == null)
            .OrderBy(t => t.TariffNameId)
            .Select(t => new MainClientTariffDto
            {
                MainClientId = t.MainClientId,
                TariffNameId = t.TariffNameId,
                TariffPeriodName = t.TariffPeriodName,
                DateInserted = t.DateInserted
            })
            .ToListAsync();

        return tariffs;
    }

    public async Task<MainClientTariffDto> CreateAsync(CreateMainClientTariffDto dto)
    {
        var entity = new MainClientTariff
        {
            MainClientId = dto.MainClientId,
            TariffNameId = dto.TariffNameId,
            TariffPeriodName = dto.TariffPeriodName,
            DateInserted = DateTime.UtcNow
        };

        _context.MainClientTariffs.Add(entity);
        await _context.SaveChangesAsync();

        return new MainClientTariffDto
        {
            MainClientId = entity.MainClientId,
            TariffNameId = entity.TariffNameId,
            TariffPeriodName = entity.TariffPeriodName,
            DateInserted = entity.DateInserted
        };
    }

    public async Task<bool> DeleteAsync(int mainClientId, int tariffNameId)
    {
        var entity = await _context.MainClientTariffs
            .FirstOrDefaultAsync(t => t.MainClientId == mainClientId 
                && t.TariffNameId == tariffNameId 
                && t.DateDeleted == null);

        if (entity == null)
            return false;

        entity.DateDeleted = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
