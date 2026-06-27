using MedManage.Core.DTOs.Admin;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for administrative user management operations
/// </summary>
public interface IUserManagementService
{
    /// <summary>
    /// Gets all users with their roles and membership information
    /// </summary>
    Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a pending user account
    /// </summary>
    Task<bool> ApproveUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Locks a user account
    /// </summary>
    Task<bool> LockUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unlocks a user account
    /// </summary>
    Task<bool> UnlockUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns roles to a user (replaces existing roles)
    /// </summary>
    Task<bool> AssignRolesAsync(Guid userId, List<string> roles, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all available roles in the system
    /// </summary>
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user (admin operation)
    /// </summary>
    Task<UserDto> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets a user's password (admin operation)
    /// </summary>
    Task<bool> AdminResetPasswordAsync(Guid userId, AdminResetPasswordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears failed login attempts for a user
    /// </summary>
    Task<bool> ClearFailedAttemptsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently blocks/deactivates a user account
    /// </summary>
    Task<bool> PermanentlyBlockUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user details (username, email)
    /// </summary>
    Task<bool> UpdateUserDetailsAsync(string userId, string? userName, string? email, CancellationToken cancellationToken = default);
}
