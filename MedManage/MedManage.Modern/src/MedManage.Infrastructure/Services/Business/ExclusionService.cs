using AutoMapper;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class ExclusionService : IExclusionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ExclusionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ExclusionDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Exclusions.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<ExclusionDto>>(entities);
    }

    public async Task<IEnumerable<ExclusionDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Exclusions.GetActiveAsync();
        return _mapper.Map<IEnumerable<ExclusionDto>>(entities);
    }

    public async Task<ExclusionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Exclusions.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            return null;
        }
        return _mapper.Map<ExclusionDto>(entity);
    }

    public async Task<ExclusionDto> CreateAsync(CreateExclusionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Exclusion>(dto);
        
        await _unitOfWork.Exclusions.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ExclusionDto>(entity);
    }

    public async Task<ExclusionDto> UpdateAsync(UpdateExclusionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Exclusions.GetByIdAsync(dto.ExclusionId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Exclusion with ID {dto.ExclusionId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.Exclusions.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ExclusionDto>(entity);
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
