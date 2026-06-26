using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Mapping.Manual;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Generic service implementation for reference data CRUD operations.
/// Eliminates boilerplate by providing a single implementation for all lookup tables.
/// </summary>
/// <typeparam name="TEntity">The entity type (must inherit BaseEntity)</typeparam>
/// <typeparam name="TDto">The read DTO type</typeparam>
/// <typeparam name="TCreateDto">The create request DTO type</typeparam>
/// <typeparam name="TUpdateDto">The update request DTO type</typeparam>
public class ReferenceDataService<TEntity, TDto, TCreateDto, TUpdateDto> 
    : IReferenceDataService<TDto, TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntityMapper<TEntity, TDto, TCreateDto, TUpdateDto> _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly string _entityName;

    public ReferenceDataService(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        IEntityMapper<TEntity, TDto, TCreateDto, TUpdateDto> mapper,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _entityName = typeof(TEntity).Name;
    }

    public async Task<IEnumerable<TDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(includeDeleted);
        return entities.Select(e => _mapper.ToDto(e));
    }

    public async Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? default : _mapper.ToDto(entity);
    }

    public async Task<TDto> CreateAsync(TCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.ToEntity(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId;

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.ToDto(entity);
    }

    public async Task<TDto> UpdateAsync(int id, TUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"{_entityName} with ID {id} not found");
        }

        _mapper.ApplyUpdate(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.ToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }

        entity.DateDeleted = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
