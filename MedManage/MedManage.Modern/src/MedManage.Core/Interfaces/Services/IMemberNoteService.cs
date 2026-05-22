using MedManage.Core.DTOs.MemberNote;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for MemberNote operations
/// </summary>
public interface IMemberNoteService
{
    Task<IEnumerable<MemberNoteDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<MemberNoteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemberNoteDto>> GetByMemberIdAsync(int memberId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<MemberNoteDto> CreateAsync(CreateMemberNoteDto dto, CancellationToken cancellationToken = default);
    Task<MemberNoteDto> UpdateAsync(int id, UpdateMemberNoteDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
