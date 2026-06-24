using MedManage.Core.DTOs.Case;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for managing case session locks to prevent concurrent edits.
/// Locks automatically expire after a configurable period of inactivity (default 5 hours).
/// Locks are also released on user logout or browser close (via Angular beforeunload).
/// </summary>
public interface ICaseLockService
{
    /// <summary>
    /// Acquires a lock on a case for the current user.
    /// Expired locks from other users are automatically reclaimed.
    /// Returns the lock state (success or existing lock info).
    /// </summary>
    Task<CaseLockDto> AcquireLockAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases the lock on a case (only the lock owner can release).
    /// </summary>
    Task<bool> ReleaseLockAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current lock state of a case.
    /// Returns IsLocked=false if the lock has expired due to inactivity.
    /// </summary>
    Task<CaseLockDto> GetLockStateAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the activity timestamp on a lock (heartbeat).
    /// Should be called periodically by the client while the user is actively working on a case.
    /// Returns false if the lock doesn't exist or belongs to another user.
    /// </summary>
    Task<bool> RefreshLockAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases all locks held by a specific user. Called on logout.
    /// </summary>
    Task<int> ReleaseAllUserLocksAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases all locks that have exceeded the inactivity timeout.
    /// Called by the background cleanup service.
    /// </summary>
    Task<int> ReleaseExpiredLocksAsync(CancellationToken cancellationToken = default);
}
