using AutoMapper;
using MedManage.Core.DTOs.Episode;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class EpisodeService : IEpisodeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public EpisodeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<EpisodeDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Episodes.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<EpisodeDto>>(entities);
    }

    public async Task<EpisodeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Episodes.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            return null;
        }
        return _mapper.Map<EpisodeDto>(entity);
    }

    public async Task<EpisodeDto?> GetByIdWithCasesAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Episodes.GetByIdWithCasesAsync(id);
        return entity == null ? null : _mapper.Map<EpisodeDto>(entity);
    }

    public async Task<IEnumerable<EpisodeDto>> SearchAsync(EpisodeSearchFilters filters, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Episodes.SearchByFiltersAsync(
            filters.EpisodeName,
            filters.MemberId,
            filters.DateFrom,
            filters.DateTo);
        
        return _mapper.Map<IEnumerable<EpisodeDto>>(entities);
    }

    public async Task<EpisodeDto> CreateAsync(CreateEpisodeDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Episode>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        // If DateCreated is not provided, set it to today
        if (!entity.DateCreated.HasValue)
        {
            entity.DateCreated = DateOnly.FromDateTime(DateTime.UtcNow);
        }
        
        await _unitOfWork.Episodes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<EpisodeDto>(entity);
    }

    public async Task<EpisodeDto> UpdateAsync(UpdateEpisodeDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Episodes.GetByIdAsync(dto.EpisodeId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Episode with ID {dto.EpisodeId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.Episodes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<EpisodeDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Episodes.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.Episodes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
