using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IIcdRepository : IRepository<Icd>
{
    Task<IEnumerable<Icd>> SearchByFiltersAsync(string? code, string? description);
    Task<Icd?> GetByIcdCodeAsync(string icdCode);
    Task<IEnumerable<Icd>> GetActiveCodesAsync();
}
