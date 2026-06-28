using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseTariffRepository : IRepository<CaseTariff>
{
    Task<IEnumerable<CaseTariff>> GetByCaseIdAsync(int caseId);
    Task<IEnumerable<CaseTariff>> GetForReportAsync(DateTime dateFrom, DateTime dateTo);
}
