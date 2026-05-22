using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface IChecklistTemplateService
{
    Task<IEnumerable<ChecklistTemplateDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<ChecklistTemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ChecklistTemplateDto> CreateAsync(CreateChecklistTemplateDto dto, CancellationToken cancellationToken = default);
    Task<ChecklistTemplateDto> UpdateAsync(UpdateChecklistTemplateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
