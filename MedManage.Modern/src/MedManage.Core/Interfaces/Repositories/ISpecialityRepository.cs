using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ISpecialityRepository : IRepository<Speciality>
{
    Task<IEnumerable<Speciality>> GetActiveAsync();
}
