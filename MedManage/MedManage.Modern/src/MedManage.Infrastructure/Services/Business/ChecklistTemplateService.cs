using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class ChecklistTemplateService : IChecklistTemplateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public ChecklistTemplateService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ChecklistTemplateDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.ChecklistTemplates.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<ChecklistTemplateDto>>(entities);
    }

    public async Task<ChecklistTemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChecklistTemplates.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<ChecklistTemplateDto>(entity);
    }

    public async Task<ChecklistTemplateDto> CreateAsync(CreateChecklistTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ChecklistTemplate>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        await _unitOfWork.ChecklistTemplates.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChecklistTemplateDto>(entity);
    }

    public async Task<ChecklistTemplateDto> UpdateAsync(UpdateChecklistTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChecklistTemplates.GetByIdAsync(dto.ChecklistTemplateId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"ChecklistTemplate with ID {dto.ChecklistTemplateId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.ChecklistTemplates.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChecklistTemplateDto>(entity);
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
