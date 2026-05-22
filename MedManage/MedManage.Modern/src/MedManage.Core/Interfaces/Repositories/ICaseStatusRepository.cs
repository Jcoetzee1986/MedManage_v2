using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseStatusRepository : IRepository<CaseStatus>
{
    Task<IEnumerable<CaseStatus>> GetActiveAsync();
    Task<CaseStatus?> GetByDescriptionAsync(string description);
}
