using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class SpecialityService : ISpecialityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public SpecialityService(IUnitOfWork unitOfWork, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<SpecialityDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Specialities.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<SpecialityDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Specialities.GetByIdAsync(id);
        return entity == null ? null : entity.ToDto();
    }

    public async Task<SpecialityDto> CreateAsync(CreateSpecialityDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.Specialities.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<SpecialityDto> UpdateAsync(UpdateSpecialityDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Specialities.GetByIdAsync(dto.SpecialityId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Speciality with ID {dto.SpecialityId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.Specialities.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Specialities.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.Specialities.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
