using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// Manages session locks for cases to prevent concurrent editing.
/// Locks automatically expire after a configurable inactivity period (default 5 hours).
/// Clients must send periodic heartbeats to keep locks alive.
/// </summary>
[ApiController]
[Route("api/cases/{caseId}/lock")]
[Authorize]
public class CaseLockController : ControllerBase
{
    private readonly ICaseLockService _caseLockService;

    public CaseLockController(ICaseLockService caseLockService)
    {
        _caseLockService = caseLockService;
    }

    /// <summary>
    /// Get the current lock state of a case.
    /// Returns IsLocked=false if the lock has expired due to inactivity.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CaseLockDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLockState(int caseId, CancellationToken cancellationToken)
    {
        var lockState = await _caseLockService.GetLockStateAsync(caseId, cancellationToken);
        return Ok(ApiResponse<CaseLockDto>.SuccessResponse(lockState));
    }

    /// <summary>
    /// Acquire a lock on a case for the current user.
    /// If another user holds an expired lock, it will be automatically reclaimed.
    /// If the lock is held by another user and NOT expired, returns 409 Conflict.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseLockDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseLockDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<CaseLockDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AcquireLock(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var lockState = await _caseLockService.AcquireLockAsync(caseId, cancellationToken);

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? "unknown";

            // Lock is considered acquired if:
            // - We are the lock owner
            // - The lock owner is "unknown" (dev/unauthenticated)
            // - The current user is "unknown" (dev/unauthenticated) 
            if (lockState.LockedByUserId == currentUserId 
                || lockState.LockedByUserId == "unknown" 
                || currentUserId == "unknown")
            {
                return Ok(ApiResponse<CaseLockDto>.SuccessResponse(lockState, "Lock acquired successfully"));
            }

            return Conflict(ApiResponse<CaseLockDto>.ErrorResponse(
                $"Case is locked by user '{lockState.LockedByUserName}' (active since {lockState.LastActivity:g}, expires {lockState.ExpiresAt:g})"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseLockDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Refresh the lock heartbeat. Call this periodically (every few minutes) while the user
    /// is actively working on the case. Without heartbeats, the lock expires after the configured
    /// inactivity timeout (default 5 hours).
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshLock(int caseId, CancellationToken cancellationToken)
    {
        var refreshed = await _caseLockService.RefreshLockAsync(caseId, cancellationToken);

        if (!refreshed)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse(
                "No active lock found for this case, or you are not the lock owner. The lock may have expired."));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Lock refreshed"));
    }

    /// <summary>
    /// Release the lock on a case. Only the lock owner can release it.
    /// Also called automatically on logout and browser close.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReleaseLock(int caseId, CancellationToken cancellationToken)
    {
        var released = await _caseLockService.ReleaseLockAsync(caseId, cancellationToken);

        if (!released)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("No lock found for this case, or you are not the lock owner"));
        }

        return NoContent();
    }

    /// <summary>
    /// Release ALL locks held by the current user. Called on logout.
    /// </summary>
    [HttpDelete("/api/cases/locks/mine")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReleaseMyLocks(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var released = await _caseLockService.ReleaseAllUserLocksAsync(userId, cancellationToken);
        return Ok(ApiResponse<int>.SuccessResponse(released, $"Released {released} lock(s)"));
    }

    /// <summary>
    /// [Admin] Get all active case locks
    /// </summary>
    [HttpGet("/api/admin/locks")]
    [Authorize(Roles = "System Administrator")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLocks(CancellationToken cancellationToken)
    {
        var locks = await _caseLockService.GetAllLocksAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<object>>.SuccessResponse(locks));
    }

    /// <summary>
    /// [Admin] Force-release a specific case lock
    /// </summary>
    [HttpDelete("/api/admin/locks/{caseId}")]
    [Authorize(Roles = "System Administrator")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AdminReleaseLock(int caseId, CancellationToken cancellationToken)
    {
        var released = await _caseLockService.AdminReleaseLockAsync(caseId, cancellationToken);
        if (!released)
            return NotFound(ApiResponse<bool>.ErrorResponse("No lock found for this case"));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Lock force-released"));
    }

    /// <summary>
    /// [Admin] Force-release ALL case locks
    /// </summary>
    [HttpDelete("/api/admin/locks")]
    [Authorize(Roles = "System Administrator")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AdminReleaseAllLocks(CancellationToken cancellationToken)
    {
        var count = await _caseLockService.AdminReleaseAllLocksAsync(cancellationToken);
        return Ok(ApiResponse<int>.SuccessResponse(count, $"Released {count} lock(s)"));
    }
}
