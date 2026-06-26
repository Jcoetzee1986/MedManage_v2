using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class BillingStatusService : IBillingStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public BillingStatusService(IUnitOfWork unitOfWork, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<BillingStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.BillingStatuses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<BillingStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.BillingStatuses.GetByIdAsync(id);
        return entity == null ? null : entity.ToDto();
    }

    public async Task<BillingStatusDto> CreateAsync(CreateBillingStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.BillingStatuses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<BillingStatusDto> UpdateAsync(UpdateBillingStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.BillingStatuses.GetByIdAsync(dto.BillingStatusId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"BillingStatus with ID {dto.BillingStatusId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.BillingStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
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
