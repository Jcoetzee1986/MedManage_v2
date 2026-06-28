namespace MedManage.Core.Interfaces;

/// <summary>
/// Email service interface for sending emails
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email message
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body (HTML or plain text)</param>
    /// <param name="isHtml">True if body contains HTML</param>
    Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);

    /// <summary>
    /// Sends a password reset PIN email
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="username">Username</param>
    /// <param name="pin">Reset PIN</param>
    Task<bool> SendPasswordResetPinAsync(string to, string username, string pin);

    /// <summary>
    /// Sends a welcome email to new users
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="username">Username</param>
    Task<bool> SendWelcomeEmailAsync(string to, string username);

    /// <summary>
    /// Sends a welcome email with temporary password to admin-created users
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="username">Username</param>
    /// <param name="temporaryPassword">Temporary password for first login</param>
    Task<bool> SendWelcomeEmailWithPasswordAsync(string to, string username, string temporaryPassword);

    /// <summary>
    /// Sends a password reset email with the new password (admin-initiated)
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="username">Username</param>
    /// <param name="newPassword">The new password</param>
    Task<bool> SendAdminPasswordResetEmailAsync(string to, string username, string newPassword);
}
