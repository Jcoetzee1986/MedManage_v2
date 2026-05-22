using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Member;

namespace MedManage.Core.Interfaces.Services;

public interface IMemberService
{
    Task<MemberDto?> GetByIdAsync(int memberId, CancellationToken cancellationToken = default);
    Task<PagedResult<MemberDto>> SearchAsync(MemberSearchRequest request, CancellationToken cancellationToken = default);
    Task<MemberDto> CreateAsync(CreateMemberRequest request, CancellationToken cancellationToken = default);
    Task<MemberDto> UpdateAsync(UpdateMemberRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int memberId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int memberId, CancellationToken cancellationToken = default);
}
