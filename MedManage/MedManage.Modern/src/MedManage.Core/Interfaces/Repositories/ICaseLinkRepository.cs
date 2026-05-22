using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseLinkRepository : IRepository<CaseLink>
{
    Task<IEnumerable<CaseLink>> GetByCaseIdAsync(int caseId);
}
