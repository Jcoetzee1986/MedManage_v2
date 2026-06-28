using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseDiscountRepository : IRepository<CaseDiscount>
{
    Task<IEnumerable<CaseDiscount>> GetByCaseIdAsync(int caseId);
}
