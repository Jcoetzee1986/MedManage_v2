using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class LanguageRepository : Repository<Language>, ILanguageRepository
{
    public LanguageRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Language>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.Language1)
            .ToListAsync();
    }
}
