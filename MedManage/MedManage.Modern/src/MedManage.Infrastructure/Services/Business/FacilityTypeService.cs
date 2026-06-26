using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class FacilityTypeService : IFacilityTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public FacilityTypeService(IUnitOfWork unitOfWork, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<FacilityTypeDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.FacilityTypes.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<FacilityTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacilityTypes.GetByIdAsync(id);
        return entity == null ? null : entity.ToDto();
    }

    public async Task<FacilityTypeDto> CreateAsync(CreateFacilityTypeDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.FacilityTypes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<FacilityTypeDto> UpdateAsync(UpdateFacilityTypeDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacilityTypes.GetByIdAsync(dto.FacilityTypeId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"FacilityType with ID {dto.FacilityTypeId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.FacilityTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacilityTypes.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.FacilityTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
