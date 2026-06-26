using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CountryService : ICountryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public CountryService(IUnitOfWork unitOfWork, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CountryDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Countries.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return entities.Select(e => e.ToDto());
    }

    public async Task<CountryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Countries.GetByIdAsync(id);
        return entity == null ? null : entity.ToDto();
    }

    public async Task<CountryDto> CreateAsync(CreateCountryDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        
        await _unitOfWork.Countries.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<CountryDto> UpdateAsync(UpdateCountryDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Countries.GetByIdAsync(dto.CountryId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Country with ID {dto.CountryId} not found");
        }
        
        dto.ApplyTo(entity);
        
        await _unitOfWork.Countries.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Countries.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.Countries.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
