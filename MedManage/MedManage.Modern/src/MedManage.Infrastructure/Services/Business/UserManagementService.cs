using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MedManage.Core.DTOs.Admin;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service for administrative user management (list, approve, lock/unlock, role assignment)
/// </summary>
public class UserManagementService : IUserManagementService
{
    private readonly MedManageDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserManagementService> _logger;

    public UserManagementService(MedManageDbContext context, IEmailService emailService, ILogger<UserManagementService> logger)
    {
        _context = context;
        _emailService = emailService;
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
        IsPermanentlyBlocked = user.AspnetMembership?.IsPermanentlyBlocked ?? false,
        FailedPasswordAttemptCount = user.AspnetMembership?.FailedPasswordAttemptCount ?? 0,
        CreateDate = user.AspnetMembership?.CreateDate ?? DateTime.MinValue,
        LastLoginDate = user.AspnetMembership?.LastLoginDate ?? DateTime.MinValue,
        LastActivityDate = user.LastActivityDate,
        Roles = user.AspnetUsersInRoles.Select(ur => ur.Role.RoleName).ToList()
    };

    public async Task<UserDto> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // Generate a temporary password if not provided
        var temporaryPassword = request.TemporaryPassword;
        if (string.IsNullOrWhiteSpace(temporaryPassword))
        {
            temporaryPassword = GenerateTemporaryPassword();
        }

        // Get default application
        var application = await _context.AspnetApplications.FirstOrDefaultAsync(cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("No application found in the system");
        }

        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        // Create user
        var user = new AspnetUser
        {
            ApplicationId = application.ApplicationId,
            UserId = userId,
            UserName = request.Username,
            LoweredUserName = request.Username.ToLower(),
            IsAnonymous = false,
            LastActivityDate = now,
            DateInserted = now
        };

        // Generate salt and hash password
        var salt = PasswordHasher.GenerateSalt();
        var hashedPassword = PasswordHasher.HashPassword(temporaryPassword, salt);

        // Create membership (auto-approved since admin created)
        var membership = new AspnetMembership
        {
            ApplicationId = application.ApplicationId,
            UserId = userId,
            Password = hashedPassword,
            PasswordSalt = salt,
            PasswordFormat = 1, // Hashed
            Email = request.Email,
            LoweredEmail = request.Email.ToLower(),
            IsApproved = true,
            IsLockedOut = false,
            IsPermanentlyBlocked = false,
            CreateDate = now,
            LastLoginDate = now,
            LastPasswordChangedDate = now,
            LastLockoutDate = DateTime.MinValue,
            FailedPasswordAttemptCount = 0,
            FailedPasswordAttemptWindowStart = DateTime.MinValue,
            FailedPasswordAnswerAttemptCount = 0,
            FailedPasswordAnswerAttemptWindowStart = DateTime.MinValue,
            DateInserted = now
        };

        _context.AspnetUsers.Add(user);
        _context.AspnetMemberships.Add(membership);
        await _context.SaveChangesAsync(cancellationToken);

        // Assign roles
        if (request.Roles.Count > 0)
        {
            var requestedRoles = await _context.AspnetRoles
                .Where(r => r.ApplicationId == application.ApplicationId && request.Roles.Contains(r.RoleName))
                .ToListAsync(cancellationToken);

            foreach (var role in requestedRoles)
            {
                _context.AspnetUsersInRoles.Add(new AspnetUsersInRole
                {
                    UserId = userId,
                    RoleId = role.RoleId
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        // Send welcome email
        if (request.SendWelcomeEmail && !string.IsNullOrWhiteSpace(request.Email))
        {
            await _emailService.SendWelcomeEmailWithPasswordAsync(request.Email, request.Username, temporaryPassword);
        }

        _logger.LogInformation("Admin created user {Username} ({UserId})", request.Username, userId);

        // Return the created user
        var createdUser = await _context.AspnetUsers
            .Include(u => u.AspnetMembership)
            .Include(u => u.AspnetUsersInRoles)
                .ThenInclude(ur => ur.Role)
            .FirstAsync(u => u.UserId == userId, cancellationToken);

        return MapToDto(createdUser);
    }

    public async Task<bool> AdminResetPasswordAsync(Guid userId, AdminResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.AspnetUsers
            .Include(u => u.AspnetMembership)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user?.AspnetMembership == null) return false;

        var membership = user.AspnetMembership;

        // Generate new password if not provided
        var newPassword = request.NewPassword;
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            newPassword = GenerateTemporaryPassword();
        }

        // Hash and set new password
        var newSalt = PasswordHasher.GenerateSalt();
        var newHashedPassword = PasswordHasher.HashPassword(newPassword, newSalt);

        membership.Password = newHashedPassword;
        membership.PasswordSalt = newSalt;
        membership.LastPasswordChangedDate = DateTime.UtcNow;

        // Clear failed attempts and unlock
        membership.FailedPasswordAttemptCount = 0;
        membership.IsLockedOut = false;
        membership.DateUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // Send email with new password
        if (request.SendEmail && !string.IsNullOrWhiteSpace(membership.Email))
        {
            await _emailService.SendAdminPasswordResetEmailAsync(membership.Email, user.UserName, newPassword);
        }

        _logger.LogInformation("Admin reset password for user {UserId}", userId);
        return true;
    }

    public async Task<bool> ClearFailedAttemptsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var membership = await _context.AspnetMemberships
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);

        if (membership == null) return false;

        membership.FailedPasswordAttemptCount = 0;
        membership.IsLockedOut = false;
        membership.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Admin cleared failed login attempts for user {UserId}", userId);
        return true;
    }

    public async Task<bool> PermanentlyBlockUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var membership = await _context.AspnetMemberships
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);

        if (membership == null) return false;

        membership.IsPermanentlyBlocked = true;
        membership.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Admin permanently blocked user {UserId}", userId);
        return true;
    }

    private static string GenerateTemporaryPassword()
    {
        // Generate a readable temporary password: 3 uppercase + 3 lowercase + 3 digits + 1 special
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghjkmnpqrstuvwxyz";
        const string digits = "23456789";
        const string special = "!@#$%&*";

        var password = new char[12];
        var rng = RandomNumberGenerator.Create();

        password[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
        password[1] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
        password[2] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
        password[3] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        password[4] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        password[5] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        password[6] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        password[7] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        password[8] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        password[9] = special[RandomNumberGenerator.GetInt32(special.Length)];
        password[10] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        password[11] = digits[RandomNumberGenerator.GetInt32(digits.Length)];

        // Shuffle the password
        for (int i = password.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }
}
