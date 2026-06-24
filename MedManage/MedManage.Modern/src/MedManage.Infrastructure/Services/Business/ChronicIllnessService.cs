using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class ChronicIllnessService : IChronicIllnessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public ChronicIllnessService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ChronicIllnessDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.ChronicIllnesses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<ChronicIllnessDto>>(entities);
    }

    public async Task<ChronicIllnessDto?> GetByIdAsync(double? id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChronicIllnesses.GetByChronicIllnessIdAsync(id);
        return entity == null ? null : _mapper.Map<ChronicIllnessDto>(entity);
    }

    public async Task<ChronicIllnessDto> CreateAsync(CreateChronicIllnessDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ChronicIllness>(dto);
        
        await _unitOfWork.ChronicIllnesses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChronicIllnessDto>(entity);
    }

    public async Task<ChronicIllnessDto> UpdateAsync(UpdateChronicIllnessDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChronicIllnesses.GetByChronicIllnessIdAsync(dto.ChronicIllnessId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"ChronicIllness with ID {dto.ChronicIllnessId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.ChronicIllnesses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChronicIllnessDto>(entity);
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
