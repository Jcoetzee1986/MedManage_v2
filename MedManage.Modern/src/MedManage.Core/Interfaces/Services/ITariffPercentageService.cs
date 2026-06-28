using MedManage.Core.DTOs.TariffPercentage;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for Tariff Percentage management operations
/// </summary>
public interface ITariffPercentageService
{
    // CRUD Operations
    Task<IEnumerable<TariffPercentageDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TariffPercentageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TariffPercentageDto> CreateAsync(CreateTariffPercentageDto dto, CancellationToken cancellationToken = default);
    Task<TariffPercentageDto> UpdateAsync(int id, UpdateTariffPercentageDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    // Propagation (implemented in task 2.4)
    Task<TariffUpdateJobStatus> ApplyPercentageAsync(int tariffPercentageId, CancellationToken cancellationToken = default);
    Task<TariffUpdateJobStatus> GetJobStatusAsync(string jobId, CancellationToken cancellationToken = default);

    // Case Letter Support (implemented in task 2.6)
    Task<IEnumerable<TariffPercentageDto>> GetActivePercentagesForLetterAsync(CancellationToken cancellationToken = default);
}
