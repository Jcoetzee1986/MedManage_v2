using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class ChecklistTemplateService : IChecklistTemplateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public ChecklistTemplateService(IUnitOfWork unitOfWork, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ChecklistTemplateDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.ChecklistTemplates.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<ChecklistTemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChecklistTemplates.GetByIdAsync(id);
        return entity == null ? null : entity.ToDto();
    }

    public async Task<ChecklistTemplateDto> CreateAsync(CreateChecklistTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.ChecklistTemplates.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<ChecklistTemplateDto> UpdateAsync(UpdateChecklistTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChecklistTemplates.GetByIdAsync(dto.ChecklistTemplateId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"ChecklistTemplate with ID {dto.ChecklistTemplateId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.ChecklistTemplates.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChecklistTemplates.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.ChecklistTemplates.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
