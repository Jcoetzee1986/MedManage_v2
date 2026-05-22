using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IChecklistTemplateRepository : IRepository<ChecklistTemplate>
{
    Task<IEnumerable<ChecklistTemplate>> GetActiveAsync();
}
