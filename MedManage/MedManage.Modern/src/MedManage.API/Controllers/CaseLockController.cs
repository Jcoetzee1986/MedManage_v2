using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// Manages session locks for cases to prevent concurrent editing.
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
    /// Get the current lock state of a case
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
    /// If the case is already locked by another user, returns the existing lock info.
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

            // Check if the lock was acquired by the current user or already held by someone else
            var currentUserId = User.FindFirst("sub")?.Value 
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? "unknown";

            if (lockState.LockedByUserId != currentUserId && lockState.LockedByUserId != "unknown")
            {
                return Conflict(ApiResponse<CaseLockDto>.ErrorResponse(
                    $"Case is already locked by user '{lockState.LockedByUserName}'"));
            }

            return Ok(ApiResponse<CaseLockDto>.SuccessResponse(lockState, "Lock acquired successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseLockDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Release the lock on a case. Only the lock owner can release it.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ReleaseLock(int caseId, CancellationToken cancellationToken)
    {
        var released = await _caseLockService.ReleaseLockAsync(caseId, cancellationToken);

        if (!released)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("No lock found for this case, or you are not the lock owner"));
        }

        return NoContent();
    }
}
