using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface INappiCodeRepository : IRepository<NappiCode>
{
    Task<IEnumerable<NappiCode>> SearchAsync(string? nappiCode, string? description, DateTime? date);
    Task<NappiCode?> GetByNappiCodeAsync(string nappiCode);
    Task<IEnumerable<NappiCode>> GetActiveCodesAsync(DateTime effectiveDate);
}
