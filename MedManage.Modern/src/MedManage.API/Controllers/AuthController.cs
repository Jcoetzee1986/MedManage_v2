using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Auth;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using System.Security.Claims;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICaseLockService _caseLockService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ICaseLockService caseLockService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _caseLockService = caseLockService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Invalid request"
            });
        }

        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Failed login attempt for user: {Username}. Reason: {Message}", 
                request.Username, response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("Successful login for user: {Username}", request.Username);
        return Ok(response);
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request">Registration details</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Invalid request"
            });
        }

        _logger.LogInformation("Registration attempt for user: {Username}", request.Username);

        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Failed registration for user: {Username}. Reason: {Message}", 
                request.Username, response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("Successful registration for user: {Username}", request.Username);
        return CreatedAtAction(nameof(GetUserInfo), new { }, response);
    }

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New JWT token and refresh token</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Invalid refresh token"
            });
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        _logger.LogInformation("Token refresh attempt from IP: {IpAddress}", ipAddress);

        var response = await _authService.RefreshTokenAsync(request.RefreshToken, ipAddress, userAgent);

        if (!response.Success)
        {
            _logger.LogWarning("Failed token refresh from IP: {IpAddress}", ipAddress);
            return BadRequest(response);
        }

        _logger.LogInformation("Successful token refresh for user: {UserId}", response.User?.UserId);
        return Ok(response);
    }

    /// <summary>
    /// Revokes a refresh token
    /// </summary>
    /// <param name="request">Refresh token to revoke</param>
    /// <returns>Success status</returns>
    [HttpPost("revoke")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest(new { message = "Invalid refresh token" });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Token revocation attempt by user: {UserId}", userId);

        var success = await _authService.RevokeRefreshTokenAsync(request.RefreshToken, "User requested revocation");

        if (!success)
        {
            _logger.LogWarning("Failed to revoke token for user: {UserId}", userId);
            return BadRequest(new { message = "Failed to revoke token" });
        }

        _logger.LogInformation("Successfully revoked token for user: {UserId}", userId);
        return Ok(new { message = "Token revoked successfully" });
    }

    /// <summary>
    /// Revokes all refresh tokens for the current user (logout from all devices).
    /// Also releases all case locks held by this user.
    /// </summary>
    /// <returns>Success status</returns>
    [HttpPost("revoke-all")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeAllTokens()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        _logger.LogInformation("Revoking all tokens for user: {UserId}", userId);

        await _authService.RevokeAllUserRefreshTokensAsync(userId, "User requested logout from all devices");

        // Release all case locks held by this user
        var locksReleased = await _caseLockService.ReleaseAllUserLocksAsync(userIdClaim);
        if (locksReleased > 0)
        {
            _logger.LogInformation("Released {Count} case lock(s) for user {UserId} on logout", locksReleased, userId);
        }

        _logger.LogInformation("Successfully revoked all tokens for user: {UserId}", userId);
        return Ok(new { message = "All tokens revoked successfully", locksReleased });
    }

    /// <summary>
    /// Gets the current authenticated user's information
    /// </summary>
    /// <returns>User information</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserInfo()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var userInfo = await _authService.GetUserInfoAsync(userId);

        if (userInfo == null)
        {
            return NotFound(new { Message = "User not found" });
        }

        return Ok(userInfo);
    }

    /// <summary>
    /// Changes the current user's password
    /// </summary>
    /// <param name="request">Password change details</param>
    /// <returns>Success status</returns>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        _logger.LogInformation("Password change attempt for user ID: {UserId}", userId);

        var success = await _authService.ChangePasswordAsync(userId, request);

        if (!success)
        {
            _logger.LogWarning("Failed password change for user ID: {UserId}", userId);
            return BadRequest(new { Message = "Failed to change password. Please verify your current password." });
        }

        _logger.LogInformation("Successful password change for user ID: {UserId}", userId);
        return Ok(new { Message = "Password changed successfully" });
    }

    /// <summary>
    /// Checks if a username is available
    /// </summary>
    /// <param name="username">Username to check</param>
    /// <returns>Availability status</returns>
    [HttpGet("check-username/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckUsername(string username)
    {
        var exists = await _authService.UserExistsAsync(username);
        return Ok(new { Available = !exists });
    }

    /// <summary>
    /// Initiates password reset process by sending a PIN to the user's email
    /// </summary>
    /// <param name="request">Email address</param>
    /// <returns>Success message</returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new PasswordResetResponse
            {
                Success = false,
                Message = "Invalid request"
            });
        }

        _logger.LogInformation("Password reset requested for email: {Email}", request.Email);

        var response = await _authService.ForgotPasswordAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Resets password using PIN
    /// </summary>
    /// <param name="request">Email, PIN, and new password</param>
    /// <returns>Success status</returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new PasswordResetResponse
            {
                Success = false,
                Message = "Invalid request"
            });
        }

        _logger.LogInformation("Password reset attempt for email: {Email}", request.Email);

        var response = await _authService.ResetPasswordAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Failed password reset for email: {Email}. Reason: {Message}", 
                request.Email, response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("Successful password reset for email: {Email}", request.Email);
        return Ok(response);
    }

    /// <summary>
    /// Verifies if a PIN is valid for password reset (optional validation before reset)
    /// </summary>
    /// <param name="request">Email and PIN</param>
    /// <returns>Validity status</returns>
    [HttpPost("verify-pin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyPin([FromBody] VerifyPinRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Valid = false, Message = "Invalid request" });
        }

        var isValid = await _authService.VerifyResetPinAsync(request.Email, request.Pin);

        return Ok(new { Valid = isValid });
    }

    /// <summary>
    /// Gets the list of MainClients the current user has access to
    /// </summary>
    [HttpGet("available-clients")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<AvailableClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAvailableClients()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var clients = await _authService.GetAvailableClientsAsync(userId);
        return Ok(clients);
    }

    /// <summary>
    /// Switches the active MainClient context and returns a new JWT token with updated claims
    /// </summary>
    [HttpPost("switch-client")]
    [Authorize]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SwitchClient([FromBody] SwitchClientRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        _logger.LogInformation("Client switch requested by user {UserId} to client {MainClientId}", userId, request.MainClientId);

        var response = await _authService.SwitchClientAsync(userId, request.MainClientId);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}

