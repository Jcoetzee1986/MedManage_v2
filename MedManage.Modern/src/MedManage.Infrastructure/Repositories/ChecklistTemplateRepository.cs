using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ChecklistTemplateRepository : Repository<ChecklistTemplate>, IChecklistTemplateRepository
{
    public ChecklistTemplateRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ChecklistTemplate>> GetActiveAsync()
    {
        return await _dbSet
            .Where(x => x.DateDeleted == null)
            .OrderBy(x => x.ChecklistPrompt)
            .ToListAsync();
    }
}
