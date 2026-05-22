using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MemberNoteRepository : Repository<MemberNote>, IMemberNoteRepository
{
    public MemberNoteRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MemberNote>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Where(x => x.MemberId == memberId && x.DateDeleted == null)
            .OrderByDescending(x => x.DateInserted)
            .ToListAsync();
    }
}
