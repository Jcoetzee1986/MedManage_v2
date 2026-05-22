using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ServiceProvider;

namespace MedManage.Core.Interfaces.Services;

public interface IServiceProviderService
{
    Task<ServiceProviderDto?> GetByIdAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceProviderDto>> SearchAsync(ServiceProviderSearchRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderDto> CreateAsync(CreateServiceProviderRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderDto> UpdateAsync(UpdateServiceProviderRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int serviceProviderId, CancellationToken cancellationToken = default);
}
