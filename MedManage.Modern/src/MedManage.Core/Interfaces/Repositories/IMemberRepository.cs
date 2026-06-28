using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByIdWithDetailsAsync(int memberId);
    Task<Member?> GetByMemberNumberAsync(string memberNumber);
    Task<IEnumerable<Member>> SearchByFiltersAsync(
        string? memberNumber,
        string? firstName,
        string? lastName,
        string? idNumber,
        int? medicalAidId,
        int? statusId,
        bool includeDeleted = false);
    Task<bool> MemberNumberExistsAsync(string memberNumber, int? excludeMemberId = null);
    Task<IEnumerable<Member>> GetByMedicalAidAsync(int medicalAidId);
}
