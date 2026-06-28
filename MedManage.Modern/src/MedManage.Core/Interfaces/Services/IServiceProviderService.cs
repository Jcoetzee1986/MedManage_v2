using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ServiceProvider;

namespace MedManage.Core.Interfaces.Services;

public interface IServiceProviderService
{
    // Core CRUD
    Task<ServiceProviderDto?> GetByIdAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceProviderDto>> SearchAsync(ServiceProviderSearchRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderDto> CreateAsync(CreateServiceProviderRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderDto> UpdateAsync(UpdateServiceProviderRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int serviceProviderId, CancellationToken cancellationToken = default);

    // Autocomplete
    Task<IEnumerable<ServiceProviderAutocompleteDto>> AutocompleteAsync(string query, CancellationToken cancellationToken = default);

    // Tariff assignment CRUD
    Task<IEnumerable<ServiceProviderTariffDto>> GetTariffsAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<ServiceProviderTariffDto?> GetTariffByIdAsync(int serviceProviderId, long tariffId, CancellationToken cancellationToken = default);
    Task<ServiceProviderTariffDto> CreateTariffAsync(int serviceProviderId, CreateServiceProviderTariffRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderTariffDto> UpdateTariffAsync(int serviceProviderId, UpdateServiceProviderTariffRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteTariffAsync(int serviceProviderId, long tariffId, CancellationToken cancellationToken = default);

    // Custom tariff CRUD
    Task<IEnumerable<ServiceProviderTariffCustomDto>> GetCustomTariffsAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<ServiceProviderTariffCustomDto?> GetCustomTariffByIdAsync(int serviceProviderId, long customTariffId, CancellationToken cancellationToken = default);
    Task<ServiceProviderTariffCustomDto> CreateCustomTariffAsync(int serviceProviderId, CreateServiceProviderTariffCustomRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderTariffCustomDto> UpdateCustomTariffAsync(int serviceProviderId, UpdateServiceProviderTariffCustomRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteCustomTariffAsync(int serviceProviderId, long customTariffId, CancellationToken cancellationToken = default);

    // Discount CRUD (per MainClient)
    Task<IEnumerable<ServiceProviderDiscountDto>> GetDiscountsAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<ServiceProviderDiscountDto?> GetDiscountByClientAsync(int serviceProviderId, int mainClientId, CancellationToken cancellationToken = default);
    Task<ServiceProviderDiscountDto> CreateDiscountAsync(int serviceProviderId, CreateServiceProviderDiscountRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProviderDiscountDto> UpdateDiscountAsync(int serviceProviderId, UpdateServiceProviderDiscountRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteDiscountAsync(int serviceProviderId, int mainClientId, CancellationToken cancellationToken = default);
}
