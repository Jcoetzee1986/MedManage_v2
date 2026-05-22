using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IServiceProviderRepository : IRepository<ServiceProvider>
{
    Task<ServiceProvider?> GetByIdWithDetailsAsync(int serviceProviderId);
    Task<IEnumerable<ServiceProvider>> SearchByFiltersAsync(
        string? practiceNumber,
        string? practiceName,
        int? specialityId);
    Task<IEnumerable<ServiceProvider>> AutocompleteSearchAsync(string searchTerm);
    Task<ServiceProvider?> GetAfterAutocompleteAsync(int serviceProviderId);
    Task<bool> PracticeNumberExistsAsync(string practiceNumber, int? excludeId = null);
}
