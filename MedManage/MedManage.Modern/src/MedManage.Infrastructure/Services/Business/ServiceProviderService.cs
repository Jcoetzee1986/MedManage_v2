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
            query = query.Where(sp => sp.ServiceProviderName != null && sp.ServiceProviderName.Contains(request.ServiceProviderName));
        }

        if (!string.IsNullOrWhiteSpace(request.ServiceProviderSurname))
        {
            query = query.Where(sp => sp.ServiceProviderSurname != null && sp.ServiceProviderSurname.Contains(request.ServiceProviderSurname));
        }

        if (!string.IsNullOrWhiteSpace(request.PracticeName))
        {
            query = query.Where(sp => sp.PracticeName != null && sp.PracticeName.Contains(request.PracticeName));
        }

        if (!string.IsNullOrWhiteSpace(request.PracticeNr))
        {
            query = query.Where(sp => sp.PracticeNr != null && sp.PracticeNr.Contains(request.PracticeNr));
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
        
        // Set audit fields
        serviceProvider.UserID = _currentUserService.UserId ?? "SYSTEM";
        
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
        
        // Set update audit fields
        existingServiceProvider.DateUpdated = DateTime.UtcNow;
        existingServiceProvider.UpdatedUserID = _currentUserService.UserId ?? "SYSTEM";
        
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
}
