using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMemberChronicIllnessRepository : IRepository<MemberChronicIllness>
{
    Task<IEnumerable<MemberChronicIllness>> GetByMemberIdAsync(int memberId);
}
