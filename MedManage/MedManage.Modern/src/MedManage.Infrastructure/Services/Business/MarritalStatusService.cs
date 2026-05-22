using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MarritalStatusService : IMarritalStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public MarritalStatusService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<MarritalStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MarritalStatuses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<MarritalStatusDto>>(entities);
    }

    public async Task<MarritalStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MarritalStatuses.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<MarritalStatusDto>(entity);
    }

    public async Task<MarritalStatusDto> CreateAsync(CreateMarritalStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<MarritalStatus>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        await _unitOfWork.MarritalStatuses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MarritalStatusDto>(entity);
    }

    public async Task<MarritalStatusDto> UpdateAsync(UpdateMarritalStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MarritalStatuses.GetByIdAsync(dto.MarritalStatusId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"MarritalStatus with ID {dto.MarritalStatusId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.MarritalStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MarritalStatusDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MarritalStatuses.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.MarritalStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
