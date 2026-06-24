using MedManage.Core.DTOs.Case;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for managing case session locks to prevent concurrent edits
/// </summary>
public interface ICaseLockService
{
    /// <summary>
    /// Acquires a lock on a case for the current user.
    /// Returns the lock state (success or existing lock info).
    /// </summary>
    Task<CaseLockDto> AcquireLockAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases the lock on a case (only the lock owner can release).
    /// </summary>
    Task<bool> ReleaseLockAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current lock state of a case.
    /// </summary>
    Task<CaseLockDto> GetLockStateAsync(int caseId, CancellationToken cancellationToken = default);
}
