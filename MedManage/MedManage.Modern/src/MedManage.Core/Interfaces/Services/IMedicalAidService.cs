using MedManage.Core.DTOs.MedicalAid;

namespace MedManage.Core.Interfaces.Services;

public interface IMedicalAidService
{
    Task<IEnumerable<MedicalAidDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalAidDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<MedicalAidDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MedicalAidDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalAidDto>> SearchAsync(MedicalAidSearchFilters filters, CancellationToken cancellationToken = default);
    Task<MedicalAidDto> CreateAsync(CreateMedicalAidDto dto, CancellationToken cancellationToken = default);
    Task<MedicalAidDto> UpdateAsync(UpdateMedicalAidDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
