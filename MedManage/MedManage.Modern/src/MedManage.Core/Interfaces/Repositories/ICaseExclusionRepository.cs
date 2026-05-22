using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseExclusionRepository : IRepository<CaseExclusion>
{
    Task<IEnumerable<CaseExclusion>> GetByCaseIdAsync(int caseId);
}
