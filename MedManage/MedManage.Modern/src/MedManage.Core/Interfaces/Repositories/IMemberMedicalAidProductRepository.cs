using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMemberMedicalAidProductRepository : IRepository<MemberMedicalAidProduct>
{
    Task<IEnumerable<MemberMedicalAidProduct>> GetByMemberIdAsync(int memberId);
}
