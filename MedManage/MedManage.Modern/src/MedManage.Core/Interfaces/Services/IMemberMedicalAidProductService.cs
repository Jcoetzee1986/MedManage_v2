using MedManage.Core.DTOs.MemberMedicalAidProduct;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Member medical aid product history operations
/// </summary>
public interface IMemberMedicalAidProductService
{
    Task<IEnumerable<MemberMedicalAidProductDto>> GetByMemberIdAsync(int memberId, CancellationToken cancellationToken = default);
    Task<MemberMedicalAidProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MemberMedicalAidProductDto> CreateAsync(int memberId, CreateMemberMedicalAidProductDto dto, CancellationToken cancellationToken = default);
    Task<MemberMedicalAidProductDto> UpdateAsync(int id, UpdateMemberMedicalAidProductDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
