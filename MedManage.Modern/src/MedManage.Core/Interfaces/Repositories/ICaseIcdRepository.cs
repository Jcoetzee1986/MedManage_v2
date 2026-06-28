using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseIcdRepository : IRepository<CaseIcd>
{
    Task<IEnumerable<CaseIcd>> GetByCaseIdAsync(int caseId);
}
