using MedManage.Core.DTOs.MemberChronicIllness;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Member chronic illness operations
/// </summary>
public interface IMemberChronicIllnessService
{
    Task<IEnumerable<MemberChronicIllnessDto>> GetByMemberIdAsync(int memberId, CancellationToken cancellationToken = default);
    Task<MemberChronicIllnessDto> CreateAsync(int memberId, CreateMemberChronicIllnessDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int memberId, int chronicIllnessId, CancellationToken cancellationToken = default);
}
