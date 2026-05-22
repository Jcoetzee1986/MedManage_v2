using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MemberChronicIllnessRepository : Repository<MemberChronicIllness>, IMemberChronicIllnessRepository
{
    public MemberChronicIllnessRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MemberChronicIllness>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(x => x.ChronicIllness)
            .Where(x => x.MemberId == memberId && x.DateDeleted == null)
            .ToListAsync();
    }
}
