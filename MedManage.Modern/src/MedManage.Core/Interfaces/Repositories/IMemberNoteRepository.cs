using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMemberNoteRepository : IRepository<MemberNote>
{
    Task<IEnumerable<MemberNote>> GetByMemberIdAsync(int memberId);
}
