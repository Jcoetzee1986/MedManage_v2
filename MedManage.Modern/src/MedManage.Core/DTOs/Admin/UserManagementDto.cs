namespace MedManage.Core.DTOs.Admin;

/// <summary>
/// DTO representing a user for admin management
/// </summary>
public class UserDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool IsApproved { get; set; }
    public bool IsLockedOut { get; set; }
    public bool IsPermanentlyBlocked { get; set; }
    public int FailedPasswordAttemptCount { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastLoginDate { get; set; }
    public DateTime LastActivityDate { get; set; }
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Request to approve a pending user
/// </summary>
public class ApproveUserRequest
{
    public Guid UserId { get; set; }
}

/// <summary>
/// Request to lock a user account
/// </summary>
public class LockUserRequest
{
    public Guid UserId { get; set; }
}

/// <summary>
/// Request to unlock a user account
/// </summary>
public class UnlockUserRequest
{
    public Guid UserId { get; set; }
}

/// <summary>
/// Request to assign roles to a user
/// </summary>
public class AssignRolesRequest
{
    public Guid UserId { get; set; }
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// DTO representing an active user session
/// </summary>
public class ActiveSessionDto
{
    public Guid TokenId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}

/// <summary>
/// Request to terminate a session
/// </summary>
public class TerminateSessionRequest
{
    public Guid TokenId { get; set; }
}

/// <summary>
/// DTO representing a role
/// </summary>
public class RoleDto
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// Request to create a new user (admin)
/// </summary>
public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? TemporaryPassword { get; set; }
    public List<string> Roles { get; set; } = new();
    public bool SendWelcomeEmail { get; set; } = true;
}

/// <summary>
/// Request to reset a user's password (admin)
/// </summary>
public class AdminResetPasswordRequest
{
    public string? NewPassword { get; set; }
    public bool SendEmail { get; set; } = true;
}

/// <summary>
/// Request to update user details (admin)
/// </summary>
public class UpdateUserDetailsRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
