using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class ServiceProviderRepository : Repository<ServiceProvider>, IServiceProviderRepository
{
    public ServiceProviderRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<ServiceProvider?> GetByIdWithDetailsAsync(int serviceProviderId)
    {
        return await _dbSet
            .Include(sp => sp.Speciality)
            // Note: ServiceProviderTariffs navigation not scaffolded
            .FirstOrDefaultAsync(sp => sp.ServiceProviderId == serviceProviderId && sp.DateDeleted == null);
    }

    public async Task<IEnumerable<ServiceProvider>> SearchByFiltersAsync(
        string? practiceNumber,
        string? practiceName,
        int? specialityId)
    {
        var query = _dbSet
            .Include(sp => sp.Speciality)
            .Where(sp => sp.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(practiceNumber))
        {
            query = query.Where(sp => sp.PracticeNr != null && sp.PracticeNr.Contains(practiceNumber));
        }

        if (!string.IsNullOrWhiteSpace(practiceName))
        {
            query = query.Where(sp => sp.PracticeName != null && sp.PracticeName.Contains(practiceName));
        }

        if (specialityId.HasValue)
        {
            query = query.Where(sp => sp.SpecialityId == specialityId.Value);
        }

        return await query
            .OrderBy(sp => sp.PracticeName)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceProvider>> AutocompleteSearchAsync(string searchTerm)
    {
        return await _dbSet
            .Where(sp => (sp.PracticeName != null && sp.PracticeName.Contains(searchTerm))
                      || (sp.PracticeNr != null && sp.PracticeNr.Contains(searchTerm))
                      && sp.DateDeleted == null)
            .OrderBy(sp => sp.PracticeName)
            .Take(20)
            .ToListAsync();
    }

    public async Task<ServiceProvider?> GetAfterAutocompleteAsync(int serviceProviderId)
    {
        return await GetByIdWithDetailsAsync(serviceProviderId);
    }

    public async Task<bool> PracticeNumberExistsAsync(string practiceNumber, int? excludeId = null)
    {
        var query = _dbSet.Where(sp => sp.PracticeNr == practiceNumber && sp.DateDeleted == null);

        if (excludeId.HasValue)
        {
            query = query.Where(sp => sp.ServiceProviderId != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
