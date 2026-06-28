using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ChronicIllnessRepository : Repository<ChronicIllness>, IChronicIllnessRepository
{
    public ChronicIllnessRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ChronicIllness>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.ChronicIllnessName)
            .ToListAsync();
    }

    public async Task<ChronicIllness?> GetByNameAsync(string name)
    {
        return await _dbSet
            .Where(x => x.ChronicIllnessName == name && x.DateDeleted == null)
            .FirstOrDefaultAsync();
    }

    public async Task<ChronicIllness?> GetByChronicIllnessIdAsync(double? id)
    {
        if (!id.HasValue)
            return null;

        return await _dbSet
            .Where(x => x.ChronicIllnessId == id)
            .FirstOrDefaultAsync();
    }
}
