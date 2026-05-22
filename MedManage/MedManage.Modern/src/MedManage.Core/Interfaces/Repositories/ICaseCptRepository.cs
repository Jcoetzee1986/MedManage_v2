using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseCptRepository : IRepository<CaseCpt>
{
    Task<IEnumerable<CaseCpt>> GetByCaseIdAsync(int caseId);
}
