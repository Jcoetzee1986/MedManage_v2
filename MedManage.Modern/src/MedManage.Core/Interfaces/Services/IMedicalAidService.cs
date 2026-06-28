using MedManage.Core.DTOs.MedicalAid;

namespace MedManage.Core.Interfaces.Services;

public interface IMedicalAidService
{
    // Medical Aid CRUD
    Task<IEnumerable<MedicalAidDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalAidDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<MedicalAidDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MedicalAidDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalAidDto>> SearchAsync(MedicalAidSearchFilters filters, CancellationToken cancellationToken = default);
    Task<MedicalAidDto> CreateAsync(CreateMedicalAidDto dto, CancellationToken cancellationToken = default);
    Task<MedicalAidDto> UpdateAsync(UpdateMedicalAidDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    // Product CRUD
    Task<IEnumerable<MedicalAidProductDto>> GetProductsByMedicalAidIdAsync(int medicalAidId, CancellationToken cancellationToken = default);
    Task<MedicalAidProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<MedicalAidProductDto> CreateProductAsync(int medicalAidId, CreateMedicalAidProductDto dto, CancellationToken cancellationToken = default);
    Task<MedicalAidProductDto> UpdateProductAsync(UpdateMedicalAidProductDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken = default);

    // Exclusion CRUD
    Task<IEnumerable<MedicalAidExclusionDto>> GetExclusionsByMedicalAidIdAsync(int medicalAidId, CancellationToken cancellationToken = default);
    Task<MedicalAidExclusionDto> AddExclusionAsync(int medicalAidId, CreateMedicalAidExclusionDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveExclusionAsync(int medicalAidId, string baseTariffId, CancellationToken cancellationToken = default);

    // Tariff Association
    Task<IEnumerable<MedicalAidTariffDto>> GetTariffsByMedicalAidIdAsync(int medicalAidId, CancellationToken cancellationToken = default);
    Task<MedicalAidTariffDto> AddTariffAsync(int medicalAidId, CreateMedicalAidTariffDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveTariffAsync(int medicalAidId, int tariffNameId, CancellationToken cancellationToken = default);
}
