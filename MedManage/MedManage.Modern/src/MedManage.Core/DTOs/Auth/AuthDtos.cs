namespace MedManage.Core.DTOs.Auth;

/// <summary>
/// Login request DTO
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Registration request DTO
/// </summary>
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Authentication response with JWT token
/// </summary>
public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? Message { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public UserInfo? User { get; set; }
}

/// <summary>
/// User information DTO
/// </summary>
public class UserInfo
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Change password request DTO
/// </summary>
public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

/// <summary>
/// Refresh token request DTO
/// </summary>
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Available client DTO for multi-client switching
/// </summary>
public class AvailableClientDto
{
    public int MainClientId { get; set; }
    public string? MainClientName { get; set; }
}

/// <summary>
/// Switch client request DTO
/// </summary>
public class SwitchClientRequest
{
    public int MainClientId { get; set; }
}
