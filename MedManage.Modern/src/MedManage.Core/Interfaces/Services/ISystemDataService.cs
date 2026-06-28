using MedManage.Core.DTOs.Admin;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for managing system configuration data
/// </summary>
public interface ISystemDataService
{
    /// <summary>
    /// Gets the current system configuration
    /// </summary>
    Task<SystemDataDto?> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets system configuration by ID
    /// </summary>
    Task<SystemDataDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates new system configuration
    /// </summary>
    Task<SystemDataDto> CreateAsync(CreateSystemDataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates existing system configuration
    /// </summary>
    Task<SystemDataDto> UpdateAsync(UpdateSystemDataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes system configuration by ID
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the system logo
    /// </summary>
    Task<bool> UpdateLogoAsync(int id, byte[] logoData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the system logo
    /// </summary>
    Task<byte[]?> GetLogoAsync(int id, CancellationToken cancellationToken = default);
}
