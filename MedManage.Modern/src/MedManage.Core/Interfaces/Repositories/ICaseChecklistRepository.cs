using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseChecklistRepository : IRepository<CaseChecklist>
{
    Task<IEnumerable<CaseChecklist>> GetByCaseIdAsync(int caseId);
}
