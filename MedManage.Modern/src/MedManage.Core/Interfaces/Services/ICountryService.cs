using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<CountryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CountryDto> CreateAsync(CreateCountryDto dto, CancellationToken cancellationToken = default);
    Task<CountryDto> UpdateAsync(UpdateCountryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
