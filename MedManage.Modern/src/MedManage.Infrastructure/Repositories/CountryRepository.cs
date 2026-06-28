using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CountryRepository : Repository<Country>, ICountryRepository
{
    public CountryRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Country>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.CountryName)
            .ToListAsync();
    }

    public async Task<Country?> GetByCountryNameAsync(string countryName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.CountryName == countryName && x.DateDeleted == null);
    }
}
