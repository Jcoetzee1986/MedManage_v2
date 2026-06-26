using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class ExclusionService : IExclusionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ExclusionService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ExclusionDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Exclusions.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<IEnumerable<ExclusionDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Exclusions.GetActiveAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<ExclusionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Exclusions.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            return null;
        }
        return entity.ToDto();
    }

    public async Task<ExclusionDto> CreateAsync(CreateExclusionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.Exclusions.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<ExclusionDto> UpdateAsync(UpdateExclusionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Exclusions.GetByIdAsync(dto.ExclusionId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Exclusion with ID {dto.ExclusionId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.Exclusions.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Exclusions.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.Exclusions.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
