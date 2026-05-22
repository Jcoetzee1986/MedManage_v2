using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ILanguageRepository : IRepository<Language>
{
    Task<IEnumerable<Language>> GetActiveAsync();
}
