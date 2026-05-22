using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CountryService : ICountryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public CountryService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CountryDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Countries.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<CountryDto>>(entities);
    }

    public async Task<CountryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Countries.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<CountryDto>(entity);
    }

    public async Task<CountryDto> CreateAsync(CreateCountryDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Country>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
        await _unitOfWork.Countries.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CountryDto>(entity);
    }

    public async Task<CountryDto> UpdateAsync(UpdateCountryDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Countries.GetByIdAsync(dto.CountryId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Country with ID {dto.CountryId} not found");
        }
        
        _mapper.Map(dto, entity);
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
        await _unitOfWork.Countries.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CountryDto>(entity);
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
