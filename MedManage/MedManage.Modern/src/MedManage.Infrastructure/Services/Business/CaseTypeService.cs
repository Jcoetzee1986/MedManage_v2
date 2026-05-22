using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseTypeService : ICaseTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public CaseTypeService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CaseTypeDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.CaseTypes.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<CaseTypeDto>>(entities);
    }

    public async Task<CaseTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseTypes.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<CaseTypeDto>(entity);
    }

    public async Task<CaseTypeDto> CreateAsync(CreateCaseTypeDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CaseType>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        await _unitOfWork.CaseTypes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CaseTypeDto>(entity);
    }

    public async Task<CaseTypeDto> UpdateAsync(UpdateCaseTypeDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseTypes.GetByIdAsync(dto.CaseTypeId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"CaseType with ID {dto.CaseTypeId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.CaseTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CaseTypeDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseTypes.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
