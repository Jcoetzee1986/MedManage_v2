using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class SpecialityRepository : Repository<Speciality>, ISpecialityRepository
{
    public SpecialityRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Speciality>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.Speciality1)
            .ToListAsync();
    }
}
