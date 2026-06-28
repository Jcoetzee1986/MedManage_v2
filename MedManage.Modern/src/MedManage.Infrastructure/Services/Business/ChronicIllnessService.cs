using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class ChronicIllnessService : IChronicIllnessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public ChronicIllnessService(IUnitOfWork unitOfWork, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ChronicIllnessDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.ChronicIllnesses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<ChronicIllnessDto?> GetByIdAsync(double? id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChronicIllnesses.GetByChronicIllnessIdAsync(id);
        return entity == null ? null : entity.ToDto();
    }

    public async Task<ChronicIllnessDto> CreateAsync(CreateChronicIllnessDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.ChronicIllnesses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<ChronicIllnessDto> UpdateAsync(UpdateChronicIllnessDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChronicIllnesses.GetByChronicIllnessIdAsync(dto.ChronicIllnessId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"ChronicIllness with ID {dto.ChronicIllnessId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.ChronicIllnesses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(double? id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChronicIllnesses.GetByChronicIllnessIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.ChronicIllnesses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
