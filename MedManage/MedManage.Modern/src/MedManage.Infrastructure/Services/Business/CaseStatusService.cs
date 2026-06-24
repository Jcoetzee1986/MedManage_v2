using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseStatusService : ICaseStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public CaseStatusService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CaseStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.CaseStatuses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<CaseStatusDto>>(entities);
    }

    public async Task<CaseStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseStatuses.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<CaseStatusDto>(entity);
    }

    public async Task<CaseStatusDto> CreateAsync(CreateCaseStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CaseStatus>(dto);
        
        await _unitOfWork.CaseStatuses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CaseStatusDto>(entity);
    }

    public async Task<CaseStatusDto> UpdateAsync(UpdateCaseStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseStatuses.GetByIdAsync(dto.CaseStatusId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"CaseStatus with ID {dto.CaseStatusId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.CaseStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CaseStatusDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseStatuses.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
