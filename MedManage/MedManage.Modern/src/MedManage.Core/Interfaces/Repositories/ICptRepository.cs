using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICptRepository : IRepository<Cpt>
{
    Task<IEnumerable<Cpt>> SearchByFiltersAsync(string? code, string? description, DateTime? effectiveDate);
    Task<Cpt?> GetByCptCodeAsync(string cptCode);
    Task<IEnumerable<Cpt>> GetActiveCodesAsync(DateTime effectiveDate);
}
