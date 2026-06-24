using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ServiceProvider;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class ServiceProviderService : IServiceProviderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ServiceProviderService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    #region Core CRUD

    public async Task<ServiceProviderDto?> GetByIdAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var serviceProvider = await _unitOfWork.ServiceProviders.GetByIdAsync(serviceProviderId);
        return serviceProvider == null ? null : _mapper.Map<ServiceProviderDto>(serviceProvider);
    }

    public async Task<PagedResult<ServiceProviderDto>> SearchAsync(ServiceProviderSearchRequest request, CancellationToken cancellationToken = default)
    {
        var allServiceProviders = await _unitOfWork.ServiceProviders.GetAllAsync();
        var query = allServiceProviders.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.ServiceProviderName))
        {
            query = query.Where(sp => sp.ServiceProviderName != null && sp.ServiceProviderName.Contains(request.ServiceProviderName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.ServiceProviderSurname))
        {
            query = query.Where(sp => sp.ServiceProviderSurname != null && sp.ServiceProviderSurname.Contains(request.ServiceProviderSurname, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.PracticeName))
        {
            query = query.Where(sp => sp.PracticeName != null && sp.PracticeName.Contains(request.PracticeName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.PracticeNr))
        {
            query = query.Where(sp => sp.PracticeNr != null && sp.PracticeNr.Contains(request.PracticeNr, StringComparison.OrdinalIgnoreCase));
        }

        if (request.SpecialityId.HasValue)
        {
            query = query.Where(sp => sp.SpecialityId == request.SpecialityId.Value);
        }

        if (request.IsHospital.HasValue)
        {
            query = query.Where(sp => sp.IsHospital == request.IsHospital.Value);
        }

        if (request.Visible.HasValue)
        {
            query = query.Where(sp => sp.Visible == request.Visible.Value);
        }

        // Apply soft delete filter
        query = query.Where(sp => sp.DateDeleted == null);

        // Get total count
        var totalCount = query.Count();

        // Apply pagination
        var serviceProviders = query
            .OrderBy(sp => sp.ServiceProviderSurname)
            .ThenBy(sp => sp.ServiceProviderName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<ServiceProviderDto>
        {
            Items = _mapper.Map<List<ServiceProviderDto>>(serviceProviders),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<ServiceProviderDto> CreateAsync(CreateServiceProviderRequest request, CancellationToken cancellationToken = default)
    {
        var serviceProvider = _mapper.Map<ServiceProvider>(request);
        
        await _unitOfWork.ServiceProviders.AddAsync(serviceProvider);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderDto>(serviceProvider);
    }

    public async Task<ServiceProviderDto> UpdateAsync(UpdateServiceProviderRequest request, CancellationToken cancellationToken = default)
    {
        var existingServiceProvider = await _unitOfWork.ServiceProviders.GetByIdAsync(request.ServiceProviderId);
        if (existingServiceProvider == null)
        {
            throw new KeyNotFoundException($"ServiceProvider with ID {request.ServiceProviderId} not found");
        }

        _mapper.Map(request, existingServiceProvider);
        
        await _unitOfWork.ServiceProviders.UpdateAsync(existingServiceProvider);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderDto>(existingServiceProvider);
    }

    public async Task<bool> DeleteAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var serviceProvider = await _unitOfWork.ServiceProviders.GetByIdAsync(serviceProviderId);
        if (serviceProvider == null)
        {
            return false;
        }

        await _unitOfWork.ServiceProviders.DeleteAsync(serviceProvider);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var serviceProvider = await _unitOfWork.ServiceProviders.GetByIdAsync(serviceProviderId);
        return serviceProvider != null;
    }

    #endregion

    #region Autocomplete

    public async Task<IEnumerable<ServiceProviderAutocompleteDto>> AutocompleteAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
        {
            return Enumerable.Empty<ServiceProviderAutocompleteDto>();
        }

        var results = await _unitOfWork.ServiceProviders.AutocompleteSearchAsync(query);
        return _mapper.Map<IEnumerable<ServiceProviderAutocompleteDto>>(results);
    }

    #endregion

    #region Tariff Assignment CRUD

    public async Task<IEnumerable<ServiceProviderTariffDto>> GetTariffsAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var tariffs = await _unitOfWork.ServiceProviderTariffs.GetByServiceProviderIdAsync(serviceProviderId);
        return _mapper.Map<IEnumerable<ServiceProviderTariffDto>>(tariffs);
    }

    public async Task<ServiceProviderTariffDto?> GetTariffByIdAsync(int serviceProviderId, long tariffId, CancellationToken cancellationToken = default)
    {
        var tariffs = await _unitOfWork.ServiceProviderTariffs.GetByServiceProviderIdAsync(serviceProviderId);
        var tariff = tariffs.FirstOrDefault(t => t.ServiceProviderTariffId == tariffId);
        return tariff == null ? null : _mapper.Map<ServiceProviderTariffDto>(tariff);
    }

    public async Task<ServiceProviderTariffDto> CreateTariffAsync(int serviceProviderId, CreateServiceProviderTariffRequest request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ServiceProviderTariff>(request);
        entity.ServiceProviderId = serviceProviderId;
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId;

        await _unitOfWork.ServiceProviderTariffs.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderTariffDto>(entity);
    }

    public async Task<ServiceProviderTariffDto> UpdateTariffAsync(int serviceProviderId, UpdateServiceProviderTariffRequest request, CancellationToken cancellationToken = default)
    {
        var tariffs = await _unitOfWork.ServiceProviderTariffs.GetByServiceProviderIdAsync(serviceProviderId);
        var existing = tariffs.FirstOrDefault(t => t.ServiceProviderTariffId == request.ServiceProviderTariffId);
        
        if (existing == null)
        {
            throw new KeyNotFoundException($"ServiceProviderTariff with ID {request.ServiceProviderTariffId} not found for provider {serviceProviderId}");
        }

        existing.TariffNameId = request.TariffNameId;
        existing.MainClientId = request.MainClientId;
        existing.StartActiveDate = request.StartActiveDate;
        existing.EndActiveDate = request.EndActiveDate;
        existing.TariffPeriodName = request.TariffPeriodName;
        existing.PercentageAdded = request.PercentageAdded;
        existing.DateUpdated = DateTime.UtcNow;
        existing.UpdatedUserID = _currentUserService.UserId;

        await _unitOfWork.ServiceProviderTariffs.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderTariffDto>(existing);
    }

    public async Task<bool> DeleteTariffAsync(int serviceProviderId, long tariffId, CancellationToken cancellationToken = default)
    {
        var tariffs = await _unitOfWork.ServiceProviderTariffs.GetByServiceProviderIdAsync(serviceProviderId);
        var existing = tariffs.FirstOrDefault(t => t.ServiceProviderTariffId == tariffId);
        
        if (existing == null)
        {
            return false;
        }

        await _unitOfWork.ServiceProviderTariffs.DeleteAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    #endregion

    #region Custom Tariff CRUD

    public async Task<IEnumerable<ServiceProviderTariffCustomDto>> GetCustomTariffsAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var customTariffs = await _unitOfWork.ServiceProviderTariffCustoms.GetByServiceProviderIdAsync(serviceProviderId);
        return _mapper.Map<IEnumerable<ServiceProviderTariffCustomDto>>(customTariffs);
    }

    public async Task<ServiceProviderTariffCustomDto?> GetCustomTariffByIdAsync(int serviceProviderId, long customTariffId, CancellationToken cancellationToken = default)
    {
        var customTariffs = await _unitOfWork.ServiceProviderTariffCustoms.GetByServiceProviderIdAsync(serviceProviderId);
        var customTariff = customTariffs.FirstOrDefault(t => t.ServiceProviderTariffCustomId == customTariffId);
        return customTariff == null ? null : _mapper.Map<ServiceProviderTariffCustomDto>(customTariff);
    }

    public async Task<ServiceProviderTariffCustomDto> CreateCustomTariffAsync(int serviceProviderId, CreateServiceProviderTariffCustomRequest request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ServiceProviderTariffCustom>(request);
        entity.ServiceProviderId = serviceProviderId;
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId;

        await _unitOfWork.ServiceProviderTariffCustoms.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderTariffCustomDto>(entity);
    }

    public async Task<ServiceProviderTariffCustomDto> UpdateCustomTariffAsync(int serviceProviderId, UpdateServiceProviderTariffCustomRequest request, CancellationToken cancellationToken = default)
    {
        var customTariffs = await _unitOfWork.ServiceProviderTariffCustoms.GetByServiceProviderIdAsync(serviceProviderId);
        var existing = customTariffs.FirstOrDefault(t => t.ServiceProviderTariffCustomId == request.ServiceProviderTariffCustomId);
        
        if (existing == null)
        {
            throw new KeyNotFoundException($"ServiceProviderTariffCustom with ID {request.ServiceProviderTariffCustomId} not found for provider {serviceProviderId}");
        }

        existing.BaseTariffId = request.BaseTariffId;
        existing.MainClientId = request.MainClientId;
        existing.TariffAmount = request.TariffAmount;
        existing.StartDate = request.StartDate;
        existing.EndDate = request.EndDate;
        existing.DateUpdated = DateTime.UtcNow;
        existing.UpdatedUserID = _currentUserService.UserId;

        await _unitOfWork.ServiceProviderTariffCustoms.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderTariffCustomDto>(existing);
    }

    public async Task<bool> DeleteCustomTariffAsync(int serviceProviderId, long customTariffId, CancellationToken cancellationToken = default)
    {
        var customTariffs = await _unitOfWork.ServiceProviderTariffCustoms.GetByServiceProviderIdAsync(serviceProviderId);
        var existing = customTariffs.FirstOrDefault(t => t.ServiceProviderTariffCustomId == customTariffId);
        
        if (existing == null)
        {
            return false;
        }

        await _unitOfWork.ServiceProviderTariffCustoms.DeleteAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    #endregion

    #region Discount CRUD

    public async Task<IEnumerable<ServiceProviderDiscountDto>> GetDiscountsAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var discounts = await _unitOfWork.ServiceProviderDiscounts.GetByServiceProviderIdAsync(serviceProviderId);
        return _mapper.Map<IEnumerable<ServiceProviderDiscountDto>>(discounts);
    }

    public async Task<ServiceProviderDiscountDto?> GetDiscountByClientAsync(int serviceProviderId, int mainClientId, CancellationToken cancellationToken = default)
    {
        var discount = await _unitOfWork.ServiceProviderDiscounts.GetByProviderAndClientAsync(serviceProviderId, mainClientId);
        return discount == null ? null : _mapper.Map<ServiceProviderDiscountDto>(discount);
    }

    public async Task<ServiceProviderDiscountDto> CreateDiscountAsync(int serviceProviderId, CreateServiceProviderDiscountRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ServiceProviderMainClientDiscount
        {
            ServiceProviderId = serviceProviderId,
            MainClientId = request.MainClientId,
            Discount = request.Discount,
            DateInserted = DateTime.UtcNow,
            UserID = _currentUserService.UserId
        };

        await _unitOfWork.ServiceProviderDiscounts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderDiscountDto>(entity);
    }

    public async Task<ServiceProviderDiscountDto> UpdateDiscountAsync(int serviceProviderId, UpdateServiceProviderDiscountRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.ServiceProviderDiscounts.GetByProviderAndClientAsync(serviceProviderId, request.MainClientId);
        
        if (existing == null)
        {
            throw new KeyNotFoundException($"Discount not found for provider {serviceProviderId} and client {request.MainClientId}");
        }

        existing.Discount = request.Discount;
        existing.DateUpdated = DateTime.UtcNow;
        existing.UpdatedUserID = _currentUserService.UserId;

        await _unitOfWork.ServiceProviderDiscounts.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceProviderDiscountDto>(existing);
    }

    public async Task<bool> DeleteDiscountAsync(int serviceProviderId, int mainClientId, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.ServiceProviderDiscounts.GetByProviderAndClientAsync(serviceProviderId, mainClientId);
        
        if (existing == null)
        {
            return false;
        }

        await _unitOfWork.ServiceProviderDiscounts.DeleteAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    #endregion
}
