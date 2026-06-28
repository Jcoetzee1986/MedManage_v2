namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Generic service interface for reference data CRUD operations.
/// All reference data entities (lookup tables) follow this pattern.
/// </summary>
/// <typeparam name="TDto">The read DTO type</typeparam>
/// <typeparam name="TCreateDto">The create request DTO type</typeparam>
/// <typeparam name="TUpdateDto">The update request DTO type</typeparam>
public interface IReferenceDataService<TDto, TCreateDto, TUpdateDto>
{
    Task<IEnumerable<TDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TDto> CreateAsync(TCreateDto dto, CancellationToken cancellationToken = default);
    Task<TDto> UpdateAsync(int id, TUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
