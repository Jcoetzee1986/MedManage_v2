using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IChronicIllnessRepository : IRepository<ChronicIllness>
{
    Task<IEnumerable<ChronicIllness>> GetActiveAsync();
    Task<ChronicIllness?> GetByNameAsync(string name);
    Task<ChronicIllness?> GetByChronicIllnessIdAsync(double? id);
}
