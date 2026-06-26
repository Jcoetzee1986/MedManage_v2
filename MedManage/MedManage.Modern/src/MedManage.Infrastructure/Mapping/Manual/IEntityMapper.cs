namespace MedManage.Infrastructure.Mapping.Manual;

/// <summary>
/// Generic mapper interface for reference data types used by ReferenceDataService.
/// </summary>
public interface IEntityMapper<TEntity, TDto, TCreateDto, TUpdateDto>
{
    TDto ToDto(TEntity entity);
    TEntity ToEntity(TCreateDto dto);
    void ApplyUpdate(TUpdateDto dto, TEntity entity);
}
