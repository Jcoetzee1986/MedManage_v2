using AutoMapper;
using MedManage.Core.DTOs.MedicalAid;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MedicalAidService : IMedicalAidService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MedicalAidService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<MedicalAidDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAids.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<MedicalAidDto>>(entities);
    }

    public async Task<IEnumerable<MedicalAidDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAids.GetActiveAsync();
        return _mapper.Map<IEnumerable<MedicalAidDto>>(entities);
    }

    public async Task<MedicalAidDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByMedicalAidIdAsync(id);
        return entity == null ? null : _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<MedicalAidDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByIdWithDetailsAsync(id);
        return entity == null ? null : _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<IEnumerable<MedicalAidDto>> SearchAsync(MedicalAidSearchFilters filters, CancellationToken cancellationToken = default)
    {
        IEnumerable<MedicalAid> entities;

        if (filters.ActiveOnly == true)
        {
            entities = await _unitOfWork.MedicalAids.GetActiveAsync();
        }
        else
        {
            entities = await _unitOfWork.MedicalAids.GetAllAsync();
            
            if (!filters.IncludeDeleted)
            {
                entities = entities.Where(x => x.DateDeleted == null);
            }
        }

        if (!string.IsNullOrWhiteSpace(filters.MedicalAidName))
        {
            entities = entities.Where(x => x.MedicalAidName != null && 
                                          x.MedicalAidName.Contains(filters.MedicalAidName, StringComparison.OrdinalIgnoreCase));
        }

        return _mapper.Map<IEnumerable<MedicalAidDto>>(entities);
    }

    public async Task<MedicalAidDto> CreateAsync(CreateMedicalAidDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<MedicalAid>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        await _unitOfWork.MedicalAids.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<MedicalAidDto> UpdateAsync(UpdateMedicalAidDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByMedicalAidIdAsync(dto.MedicalAidId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Medical aid with ID {dto.MedicalAidId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.MedicalAids.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByMedicalAidIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.MedicalAids.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
