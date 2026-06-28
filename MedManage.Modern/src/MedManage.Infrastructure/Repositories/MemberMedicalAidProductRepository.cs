using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MemberMedicalAidProductRepository : Repository<MemberMedicalAidProduct>, IMemberMedicalAidProductRepository
{
    public MemberMedicalAidProductRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MemberMedicalAidProduct>> GetByMemberIdAsync(int memberId)
    {
        // Note: MemberMedicalAidProduct doesn't have navigation properties - query by MemberId only
        return await _dbSet
            .Where(x => x.MemberId == memberId && x.DateDeleted == null)
            .ToListAsync();
    }
}
