using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MemberStatusService : IMemberStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public MemberStatusService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<MemberStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MemberStatuses.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<MemberStatusDto>>(entities);
    }

    public async Task<MemberStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberStatuses.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<MemberStatusDto>(entity);
    }

    public async Task<MemberStatusDto> CreateAsync(CreateMemberStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<MemberStatus>(dto);
        
        await _unitOfWork.MemberStatuses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MemberStatusDto>(entity);
    }

    public async Task<MemberStatusDto> UpdateAsync(UpdateMemberStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberStatuses.GetByIdAsync(dto.MemberStatusId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"MemberStatus with ID {dto.MemberStatusId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.MemberStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MemberStatusDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberStatuses.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.MemberStatuses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
