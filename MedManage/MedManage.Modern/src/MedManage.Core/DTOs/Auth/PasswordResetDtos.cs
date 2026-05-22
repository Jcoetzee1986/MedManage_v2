namespace MedManage.Core.DTOs.Auth;

/// <summary>
/// Request to initiate password reset
/// </summary>
public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Request to reset password using PIN
/// </summary>
public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Response for password reset operations
/// </summary>
public class PasswordResetResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}

/// <summary>
/// Request to verify PIN validity
/// </summary>
public class VerifyPinRequest
{
    public string Email { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
}
