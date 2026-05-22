using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseNappiCodeRepository : IRepository<CaseNappiCode>
{
    Task<IEnumerable<CaseNappiCode>> GetByCaseIdAsync(int caseId);
}
