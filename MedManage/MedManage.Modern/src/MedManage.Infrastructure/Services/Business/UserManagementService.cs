using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MedManage.Core.DTOs.Admin;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service for administrative user management (list, approve, lock/unlock, role assignment)
/// </summary>
public class UserManagementService : IUserManagementService
{
    private readonly MedManageDbContext _context;
    private readonly ILogger<UserManagementService> _logger;

    public UserManagementService(MedManageDbContext context, ILogger<UserManagementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.AspnetUsers
            .Include(u => u.AspnetMembership)
            .Include(u => u.AspnetUsersInRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsAnonymous)
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);

        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.AspnetUsers
            .Include(u => u.AspnetMembership)
            .Include(u => u.AspnetUsersInRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        return user == null ? null : MapToDto(user);
    }

    public async Task<bool> ApproveUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var membership = await _context.AspnetMemberships
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);

        if (membership == null) return false;

        membership.IsApproved = true;
        membership.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserId} approved", userId);
        return true;
    }

    public async Task<bool> LockUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var membership = await _context.AspnetMemberships
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);

        if (membership == null) return false;

        membership.IsLockedOut = true;
        membership.LastLockoutDate = DateTime.UtcNow;
        membership.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserId} locked out", userId);
        return true;
    }

    public async Task<bool> UnlockUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var membership = await _context.AspnetMemberships
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);

        if (membership == null) return false;

        membership.IsLockedOut = false;
        membership.FailedPasswordAttemptCount = 0;
        membership.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserId} unlocked", userId);
        return true;
    }

    public async Task<bool> AssignRolesAsync(Guid userId, List<string> roles, CancellationToken cancellationToken = default)
    {
        var user = await _context.AspnetUsers
            .Include(u => u.AspnetUsersInRoles)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null) return false;

        // Get the application ID from the user
        var applicationId = user.ApplicationId;

        // Remove existing role assignments
        var existingRoles = await _context.AspnetUsersInRoles
            .Where(ur => ur.UserId == userId)
            .ToListAsync(cancellationToken);
        _context.AspnetUsersInRoles.RemoveRange(existingRoles);

        // Look up requested roles by name
        var requestedRoles = await _context.AspnetRoles
            .Where(r => r.ApplicationId == applicationId && roles.Contains(r.RoleName))
            .ToListAsync(cancellationToken);

        // Add new role assignments
        foreach (var role in requestedRoles)
        {
            _context.AspnetUsersInRoles.Add(new AspnetUsersInRole
            {
                UserId = userId,
                RoleId = role.RoleId
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Roles updated for user {UserId}: {Roles}", userId, string.Join(", ", roles));
        return true;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _context.AspnetRoles
            .OrderBy(r => r.RoleName)
            .ToListAsync(cancellationToken);

        return roles.Select(r => new RoleDto
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName,
            Description = r.Description
        });
    }

    private static UserDto MapToDto(AspnetUser user) => new()
    {
        UserId = user.UserId,
        UserName = user.UserName,
        Email = user.AspnetMembership?.Email,
        IsApproved = user.AspnetMembership?.IsApproved ?? false,
        IsLockedOut = user.AspnetMembership?.IsLockedOut ?? false,
        CreateDate = user.AspnetMembership?.CreateDate ?? DateTime.MinValue,
        LastLoginDate = user.AspnetMembership?.LastLoginDate ?? DateTime.MinValue,
        LastActivityDate = user.LastActivityDate,
        Roles = user.AspnetUsersInRoles.Select(ur => ur.Role.RoleName).ToList()
    };
}
