using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface IMemberStatusService
{
    Task<IEnumerable<MemberStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<MemberStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MemberStatusDto> CreateAsync(CreateMemberStatusDto dto, CancellationToken cancellationToken = default);
    Task<MemberStatusDto> UpdateAsync(UpdateMemberStatusDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
