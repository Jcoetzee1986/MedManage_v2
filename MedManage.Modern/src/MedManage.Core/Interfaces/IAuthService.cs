using MedManage.Core.DTOs.Auth;

namespace MedManage.Core.Interfaces;

/// <summary>
/// Authentication service interface for user authentication and authorization
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with username and password
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Registers a new user
    /// </summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Changes user password
    /// </summary>
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);

    /// <summary>
    /// Validates if a user exists
    /// </summary>
    Task<bool> UserExistsAsync(string username);

    /// <summary>
    /// Gets user information by user ID
    /// </summary>
    Task<UserInfo?> GetUserInfoAsync(Guid userId);
    
    /// <summary>
    /// Generates JWT token for a user
    /// </summary>
    string GenerateJwtToken(Guid userId, string username, IEnumerable<string> roles);

    /// <summary>
    /// Generates a refresh token for a user
    /// </summary>
    Task<string> GenerateRefreshTokenAsync(Guid userId, string? ipAddress = null, string? userAgent = null);

    /// <summary>
    /// Refreshes access token using refresh token
    /// </summary>
    Task<AuthResponse> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null);

    /// <summary>
    /// Revokes a refresh token
    /// </summary>
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, string? reason = null);

    /// <summary>
    /// Revokes all refresh tokens for a user
    /// </summary>
    Task RevokeAllUserRefreshTokensAsync(Guid userId, string? reason = null);

    /// <summary>
    /// Initiates password reset process by sending PIN to user's email
    /// </summary>
    Task<PasswordResetResponse> ForgotPasswordAsync(ForgotPasswordRequest request);

    /// <summary>
    /// Resets password using PIN
    /// </summary>
    Task<PasswordResetResponse> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    /// Verifies if a PIN is valid for password reset
    /// </summary>
    Task<bool> VerifyResetPinAsync(string email, string pin);

    /// <summary>
    /// Gets available main clients the user has access to
    /// </summary>
    Task<IEnumerable<DTOs.Auth.AvailableClientDto>> GetAvailableClientsAsync(Guid userId);

    /// <summary>
    /// Switches the active main client and returns a new token
    /// </summary>
    Task<AuthResponse> SwitchClientAsync(Guid userId, int mainClientId);
}
