using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Session locking service using the existing Session_User_Case table.
/// Prevents concurrent editing of the same case by multiple users.
/// </summary>
public class CaseLockService : ICaseLockService
{
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CaseLockService(MedManageDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CaseLockDto> AcquireLockAsync(int caseId, CancellationToken cancellationToken = default)
    {
        // Check if the case exists
        var caseExists = await _context.Cases
            .AnyAsync(c => c.CaseId == caseId && c.DateDeleted == null, cancellationToken);

        if (!caseExists)
        {
            throw new KeyNotFoundException($"Case with ID {caseId} not found");
        }

        // Check for existing lock
        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        if (existingLock != null)
        {
            // Lock already exists
            var currentUserId = _currentUserService.UserId ?? "unknown";
            if (existingLock.UserID == currentUserId)
            {
                // Already locked by this user - refresh timestamp
                existingLock.DateUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);

                return new CaseLockDto
                {
                    IsLocked = true,
                    CaseId = caseId,
                    LockedByUserId = existingLock.UserID,
                    LockedByUserName = existingLock.UserID,
                    LockedAt = existingLock.DateInserted,
                };
            }

            // Locked by another user
            return new CaseLockDto
            {
                IsLocked = true,
                CaseId = caseId,
                LockedByUserId = existingLock.UserID,
                LockedByUserName = existingLock.UserID,
                LockedAt = existingLock.DateInserted,
            };
        }

        // Acquire new lock
        var userId = _currentUserService.UserId ?? "unknown";
        var newLock = new SessionUserCase
        {
            CaseId = caseId,
            UserID = userId,
            DateInserted = DateTime.UtcNow,
        };

        _context.SessionUserCases.Add(newLock);
        await _context.SaveChangesAsync(cancellationToken);

        return new CaseLockDto
        {
            IsLocked = true,
            CaseId = caseId,
            LockedByUserId = userId,
            LockedByUserName = userId,
            LockedAt = newLock.DateInserted,
        };
    }

    public async Task<bool> ReleaseLockAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId ?? "unknown";

        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        if (existingLock == null)
        {
            return false; // No lock to release
        }

        // Only the lock owner (or if no user check) can release
        if (existingLock.UserID != currentUserId)
        {
            return false; // Not the lock owner
        }

        // Soft delete the lock (or hard delete since it's a session lock)
        existingLock.DateDeleted = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<CaseLockDto> GetLockStateAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var existingLock = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId && s.DateDeleted == null, cancellationToken);

        if (existingLock == null)
        {
            return new CaseLockDto
            {
                IsLocked = false,
                CaseId = caseId,
            };
        }

        return new CaseLockDto
        {
            IsLocked = true,
            CaseId = caseId,
            LockedByUserId = existingLock.UserID,
            LockedByUserName = existingLock.UserID,
            LockedAt = existingLock.DateInserted,
        };
    }
}
