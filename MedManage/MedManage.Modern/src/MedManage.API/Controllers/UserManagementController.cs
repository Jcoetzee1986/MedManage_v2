using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Admin;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// API controller for user management operations (admin only)
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize(Roles = "System Administrator")]
public class UserManagementController : ControllerBase
{
    private readonly IUserManagementService _service;
    private readonly ILogger<UserManagementController> _logger;

    public UserManagementController(IUserManagementService service, ILogger<UserManagementController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with their roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllUsersAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(users));
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _service.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
            return NotFound(ApiResponse<UserDto>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<UserDto>.SuccessResponse(user));
    }

    /// <summary>
    /// Approve a pending user
    /// </summary>
    [HttpPost("{userId}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _service.ApproveUserAsync(userId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "User approved successfully"));
    }

    /// <summary>
    /// Update user details (username, email)
    /// </summary>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDetailsRequest request)
    {
        var result = await _service.UpdateUserDetailsAsync(userId, request.UserName, request.Email);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "User updated successfully"));
    }

    /// <summary>
    /// Lock a user account
    /// </summary>
    [HttpPut("{userId}/lock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Lock(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _service.LockUserAsync(userId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "User locked successfully"));
    }

    /// <summary>
    /// Unlock a user account
    /// </summary>
    [HttpPut("{userId}/unlock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unlock(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _service.UnlockUserAsync(userId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "User unlocked successfully"));
    }

    /// <summary>
    /// Assign roles to a user (replaces existing roles)
    /// </summary>
    [HttpPut("{userId}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRoles(Guid userId, [FromBody] AssignRolesRequest request, CancellationToken cancellationToken)
    {
        if (request.Roles == null || request.Roles.Count == 0)
            return BadRequest(ApiResponse<bool>.ErrorResponse("At least one role must be specified"));

        request.UserId = userId;
        var result = await _service.AssignRolesAsync(userId, request.Roles, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Roles assigned successfully"));
    }

    /// <summary>
    /// Get all available roles
    /// </summary>
    [HttpGet("roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await _service.GetAllRolesAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(roles));
    }

    /// <summary>
    /// Create a new user (admin operation)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(ApiResponse<UserDto>.ErrorResponse("Username and Email are required"));

        try
        {
            var user = await _service.CreateUserAsync(request, cancellationToken);
            return Ok(ApiResponse<UserDto>.SuccessResponse(user, "User created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user {Username}", request.Username);
            return BadRequest(ApiResponse<UserDto>.ErrorResponse($"Failed to create user: {ex.Message}"));
        }
    }

    /// <summary>
    /// Reset a user's password (admin operation)
    /// </summary>
    [HttpPost("{userId}/reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdminResetPassword(Guid userId, [FromBody] AdminResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AdminResetPasswordAsync(userId, request, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Password reset successfully"));
    }

    /// <summary>
    /// Clear failed login attempts for a user
    /// </summary>
    [HttpPost("{userId}/clear-attempts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ClearFailedAttempts(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _service.ClearFailedAttemptsAsync(userId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Failed login attempts cleared"));
    }

    /// <summary>
    /// Permanently block/deactivate a user account
    /// </summary>
    [HttpPut("{userId}/block")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PermanentlyBlock(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _service.PermanentlyBlockUserAsync(userId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"User with ID {userId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "User permanently blocked"));
    }
}
