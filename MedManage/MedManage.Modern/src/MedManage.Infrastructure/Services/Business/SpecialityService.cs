using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class SpecialityService : ISpecialityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public SpecialityService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<SpecialityDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Specialities.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<SpecialityDto>>(entities);
    }

    public async Task<SpecialityDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Specialities.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<SpecialityDto>(entity);
    }

    public async Task<SpecialityDto> CreateAsync(CreateSpecialityDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Speciality>(dto);
        
        await _unitOfWork.Specialities.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<SpecialityDto>(entity);
    }

    public async Task<SpecialityDto> UpdateAsync(UpdateSpecialityDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Specialities.GetByIdAsync(dto.SpecialityId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Speciality with ID {dto.SpecialityId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.Specialities.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<SpecialityDto>(entity);
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
