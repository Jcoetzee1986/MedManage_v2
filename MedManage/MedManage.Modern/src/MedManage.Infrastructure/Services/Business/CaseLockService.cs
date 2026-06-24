using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedManage.Core.Configuration;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Session locking service using the Session_User_Case table.
/// Locks automatically expire after a configurable inactivity timeout (default 5 hours).
/// The "last activity" is tracked via DateUpdated — clients must send periodic heartbeats.
/// </summary>
public class CaseLockService : ICaseLockService
{
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly CaseLockSettings _settings;
    private readonly ILogger<CaseLockService> _logger;

    public CaseLockService(
        MedManageDbContext context,
        ICurrentUserService currentUserService,
        IOptions<CaseLockSettings> settings,
        ILogger<CaseLockService> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<CaseLockDto> AcquireLockAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var caseExists = await _context.Cases
            .AnyAsync(c => c.CaseId == caseId && c.DateDeleted == null, cancellationToken);

        if (!caseExists)
        {
            throw new KeyNotFoundException($"Case with ID {caseId} not found");
        }

        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        var currentUserId = _currentUserService.UserId ?? "unknown";
        var now = DateTime.UtcNow;

        if (existingLock != null)
        {
            // Check if the existing lock has expired due to inactivity
            var lastActivity = existingLock.DateUpdated ?? existingLock.DateInserted ?? now;
            var expiresAt = lastActivity.AddHours(_settings.InactivityTimeoutHours);

            if (now >= expiresAt)
            {
                // Lock has expired — reclaim it
                _logger.LogInformation(
                    "Expired lock on Case {CaseId} by user {PreviousUser} (last activity: {LastActivity}). Reclaiming for {CurrentUser}.",
                    caseId, existingLock.UserID, lastActivity, currentUserId);

                existingLock.DateDeleted = now;
                // Fall through to create a new lock below
            }
            else if (existingLock.UserID == currentUserId)
            {
                // Already locked by this user — refresh the heartbeat
                existingLock.DateUpdated = now;
                await _context.SaveChangesAsync(cancellationToken);

                return BuildLockDto(caseId, existingLock, now);
            }
            else
            {
                // Locked by another user and NOT expired — return conflict info
                return BuildLockDto(caseId, existingLock, now);
            }
        }

        // Acquire new lock
        var newLock = new SessionUserCase
        {
            CaseId = caseId,
            UserID = currentUserId,
            DateInserted = now,
            DateUpdated = now, // LastActivity = now
        };

        _context.SessionUserCases.Add(newLock);
        await _context.SaveChangesAsync(cancellationToken);

        return BuildLockDto(caseId, newLock, now);
    }

    public async Task<bool> ReleaseLockAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId ?? "unknown";

        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        if (existingLock == null)
            return false;

        // Only the lock owner can release
        if (existingLock.UserID != currentUserId)
            return false;

        existingLock.DateDeleted = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Lock released on Case {CaseId} by user {UserId}", caseId, currentUserId);
        return true;
    }

    public async Task<CaseLockDto> GetLockStateAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        if (existingLock == null)
        {
            return new CaseLockDto { IsLocked = false, CaseId = caseId };
        }

        var now = DateTime.UtcNow;
        var lastActivity = existingLock.DateUpdated ?? existingLock.DateInserted ?? now;
        var expiresAt = lastActivity.AddHours(_settings.InactivityTimeoutHours);

        // If expired, treat as unlocked (cleanup service will remove it shortly)
        if (now >= expiresAt)
        {
            return new CaseLockDto { IsLocked = false, CaseId = caseId };
        }

        return BuildLockDto(caseId, existingLock, now);
    }

    public async Task<bool> RefreshLockAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId ?? "unknown";

        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        if (existingLock == null)
            return false;

        if (existingLock.UserID != currentUserId)
            return false;

        // Update the activity timestamp (heartbeat)
        existingLock.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> ReleaseAllUserLocksAsync(string userId, CancellationToken cancellationToken = default)
    {
        var userLocks = await _context.SessionUserCases
            .Where(s => s.UserID == userId && s.DateDeleted == null)
            .ToListAsync(cancellationToken);

        if (userLocks.Count == 0)
            return 0;

        var now = DateTime.UtcNow;
        foreach (var lk in userLocks)
        {
            lk.DateDeleted = now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Released {Count} lock(s) for user {UserId} on logout", userLocks.Count, userId);
        return userLocks.Count;
    }

    public async Task<int> ReleaseExpiredLocksAsync(CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddHours(-_settings.InactivityTimeoutHours);

        // Find locks where the last activity is older than the cutoff
        var expiredLocks = await _context.SessionUserCases
            .Where(s => s.DateDeleted == null
                && (s.DateUpdated ?? s.DateInserted) < cutoff)
            .ToListAsync(cancellationToken);

        if (expiredLocks.Count == 0)
            return 0;

        var now = DateTime.UtcNow;
        foreach (var lk in expiredLocks)
        {
            lk.DateDeleted = now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cleanup: released {Count} expired lock(s) (inactivity > {Hours}h)",
            expiredLocks.Count, _settings.InactivityTimeoutHours);

        return expiredLocks.Count;
    }

    private CaseLockDto BuildLockDto(int caseId, SessionUserCase lockRecord, DateTime now)
    {
        var lastActivity = lockRecord.DateUpdated ?? lockRecord.DateInserted ?? now;
        var expiresAt = lastActivity.AddHours(_settings.InactivityTimeoutHours);

        return new CaseLockDto
        {
            IsLocked = true,
            CaseId = caseId,
            LockedByUserId = lockRecord.UserID,
            LockedByUserName = lockRecord.UserID,
            LockedAt = lockRecord.DateInserted,
            LastActivity = lastActivity,
            ExpiresAt = expiresAt,
        };
    }
}
