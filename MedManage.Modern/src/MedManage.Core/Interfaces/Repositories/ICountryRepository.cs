using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICountryRepository : IRepository<Country>
{
    Task<IEnumerable<Country>> GetActiveAsync();
    Task<Country?> GetByCountryNameAsync(string countryName);
}
