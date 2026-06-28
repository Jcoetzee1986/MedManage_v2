using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IBillingStatusRepository : IRepository<BillingStatus>
{
    Task<IEnumerable<BillingStatus>> GetActiveAsync();
}
