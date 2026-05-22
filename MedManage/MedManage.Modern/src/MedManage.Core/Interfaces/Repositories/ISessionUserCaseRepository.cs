using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ISessionUserCaseRepository : IRepository<SessionUserCase>
{
    Task<IEnumerable<SessionUserCase>> GetByCaseIdAsync(int caseId);
}
