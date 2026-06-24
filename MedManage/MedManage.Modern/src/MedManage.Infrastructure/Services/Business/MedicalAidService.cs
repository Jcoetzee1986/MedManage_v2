using AutoMapper;
using MedManage.Core.DTOs.MedicalAid;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MedicalAidService : IMedicalAidService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MedicalAidService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    // ─── Medical Aid CRUD ──────────────────────────────────────────

    public async Task<IEnumerable<MedicalAidDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAids.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<MedicalAidDto>>(entities);
    }

    public async Task<IEnumerable<MedicalAidDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAids.GetActiveAsync();
        return _mapper.Map<IEnumerable<MedicalAidDto>>(entities);
    }

    public async Task<MedicalAidDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByMedicalAidIdAsync(id);
        return entity == null ? null : _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<MedicalAidDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByIdWithDetailsAsync(id);
        return entity == null ? null : _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<IEnumerable<MedicalAidDto>> SearchAsync(MedicalAidSearchFilters filters, CancellationToken cancellationToken = default)
    {
        IEnumerable<MedicalAid> entities;

        if (filters.ActiveOnly == true)
        {
            entities = await _unitOfWork.MedicalAids.GetActiveAsync();
        }
        else
        {
            entities = await _unitOfWork.MedicalAids.GetAllAsync();
            
            if (!filters.IncludeDeleted)
            {
                entities = entities.Where(x => x.DateDeleted == null);
            }
        }

        if (!string.IsNullOrWhiteSpace(filters.MedicalAidName))
        {
            entities = entities.Where(x => x.MedicalAidName != null && 
                                          x.MedicalAidName.Contains(filters.MedicalAidName, StringComparison.OrdinalIgnoreCase));
        }

        return _mapper.Map<IEnumerable<MedicalAidDto>>(entities);
    }

    public async Task<MedicalAidDto> CreateAsync(CreateMedicalAidDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<MedicalAid>(dto);
        
        await _unitOfWork.MedicalAids.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<MedicalAidDto> UpdateAsync(UpdateMedicalAidDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByMedicalAidIdAsync(dto.MedicalAidId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Medical aid with ID {dto.MedicalAidId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.MedicalAids.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MedicalAidDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAids.GetByMedicalAidIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.MedicalAids.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    // ─── Product CRUD ──────────────────────────────────────────────

    public async Task<IEnumerable<MedicalAidProductDto>> GetProductsByMedicalAidIdAsync(int medicalAidId, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAidProducts.GetByMedicalAidIdAsync(medicalAidId);
        return _mapper.Map<IEnumerable<MedicalAidProductDto>>(entities);
    }

    public async Task<MedicalAidProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAidProducts.GetByIdAsync(productId);
        return entity == null ? null : _mapper.Map<MedicalAidProductDto>(entity);
    }

    public async Task<MedicalAidProductDto> CreateProductAsync(int medicalAidId, CreateMedicalAidProductDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new MedicalAidProduct
        {
            MainClientId = medicalAidId,
            MedAidProductName = dto.MedAidProductName,
            AllowServices = dto.AllowServices ?? true
        };

        await _unitOfWork.MedicalAidProducts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MedicalAidProductDto>(entity);
    }

    public async Task<MedicalAidProductDto> UpdateProductAsync(UpdateMedicalAidProductDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAidProducts.GetByIdAsync(dto.MedAidProductId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Medical aid product with ID {dto.MedAidProductId} not found");
        }

        entity.MedAidProductName = dto.MedAidProductName;
        entity.AllowServices = dto.AllowServices;

        await _unitOfWork.MedicalAidProducts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MedicalAidProductDto>(entity);
    }

    public async Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MedicalAidProducts.GetByIdAsync(productId);
        if (entity == null)
        {
            return false;
        }

        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.MedicalAidProducts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    // ─── Exclusion CRUD ────────────────────────────────────────────

    public async Task<IEnumerable<MedicalAidExclusionDto>> GetExclusionsByMedicalAidIdAsync(int medicalAidId, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAidExclusions.GetByMedicalAidIdAsync(medicalAidId);
        return entities.Select(e => new MedicalAidExclusionDto
        {
            MedicalAidId = e.MedicalAidId,
            BaseTariffId = e.BaseTariffId,
            BaseTariffDescription = e.BaseTariff?.TariffDescription
        }).ToList();
    }

    public async Task<MedicalAidExclusionDto> AddExclusionAsync(int medicalAidId, CreateMedicalAidExclusionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new MedicalAidExclusion
        {
            MedicalAidId = medicalAidId,
            BaseTariffId = dto.BaseTariffId
        };

        await _unitOfWork.MedicalAidExclusions.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new MedicalAidExclusionDto
        {
            MedicalAidId = entity.MedicalAidId,
            BaseTariffId = entity.BaseTariffId
        };
    }

    public async Task<bool> RemoveExclusionAsync(int medicalAidId, string baseTariffId, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAidExclusions.FindAsync(
            e => e.MedicalAidId == medicalAidId && e.BaseTariffId == baseTariffId);
        
        var entity = entities.FirstOrDefault();
        if (entity == null)
        {
            return false;
        }

        await _unitOfWork.MedicalAidExclusions.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    // ─── Tariff Association ────────────────────────────────────────

    public async Task<IEnumerable<MedicalAidTariffDto>> GetTariffsByMedicalAidIdAsync(int medicalAidId, CancellationToken cancellationToken = default)
    {
        var medicalAid = await _unitOfWork.MedicalAids.GetByIdWithDetailsAsync(medicalAidId);
        if (medicalAid == null)
        {
            return Enumerable.Empty<MedicalAidTariffDto>();
        }

        return medicalAid.MedicalAidTariffs.Select(t => new MedicalAidTariffDto
        {
            MedicalAidId = t.MedicalAidId,
            TariffNameId = t.TariffNameId,
            TariffName = t.TariffName?.TariffName1
        }).ToList();
    }

    public async Task<MedicalAidTariffDto> AddTariffAsync(int medicalAidId, CreateMedicalAidTariffDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new MedicalAidTariff
        {
            MedicalAidId = medicalAidId,
            TariffNameId = dto.TariffNameId
        };

        await _unitOfWork.MedicalAidTariffs.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new MedicalAidTariffDto
        {
            MedicalAidId = entity.MedicalAidId,
            TariffNameId = entity.TariffNameId
        };
    }

    public async Task<bool> RemoveTariffAsync(int medicalAidId, int tariffNameId, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.MedicalAidTariffs.FindAsync(
            t => t.MedicalAidId == medicalAidId && t.TariffNameId == tariffNameId);
        
        var entity = entities.FirstOrDefault();
        if (entity == null)
        {
            return false;
        }

        await _unitOfWork.MedicalAidTariffs.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
