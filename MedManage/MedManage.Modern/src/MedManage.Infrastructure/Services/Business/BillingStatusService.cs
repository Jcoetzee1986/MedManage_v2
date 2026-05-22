using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class BillingStatusService : IBillingStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public BillingStatusService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<BillingStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.BillingStatuses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<BillingStatusDto>>(entities);
    }

    public async Task<BillingStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.BillingStatuses.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<BillingStatusDto>(entity);
    }

    public async Task<BillingStatusDto> CreateAsync(CreateBillingStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<BillingStatus>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        await _unitOfWork.BillingStatuses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<BillingStatusDto>(entity);
    }

    public async Task<BillingStatusDto> UpdateAsync(UpdateBillingStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.BillingStatuses.GetByIdAsync(dto.BillingStatusId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"BillingStatus with ID {dto.BillingStatusId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.BillingStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<BillingStatusDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.BillingStatuses.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.BillingStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
