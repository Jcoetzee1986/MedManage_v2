namespace MedManage.Core.Configuration;

/// <summary>
/// Email service configuration settings
/// </summary>
public class EmailSettings
{
    public const string SectionName = "EmailSettings";

    /// <summary>
    /// SMTP server host
    /// </summary>
    public string SmtpHost { get; set; } = string.Empty;

    /// <summary>
    /// SMTP server port (typically 587 for TLS, 465 for SSL, 25 for non-secure)
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// SMTP username for authentication
    /// </summary>
    public string SmtpUsername { get; set; } = string.Empty;

    /// <summary>
    /// SMTP password for authentication
    /// </summary>
    public string SmtpPassword { get; set; } = string.Empty;

    /// <summary>
    /// Enable SSL/TLS encryption
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// From email address
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// From name displayed in emails
    /// </summary>
    public string FromName { get; set; } = "MedManage";

    /// <summary>
    /// Test mode: redirect all emails to this address
    /// </summary>
    public bool UseTestMode { get; set; }

    /// <summary>
    /// Test email address to receive all emails in test mode
    /// </summary>
    public string? TestEmailAddress { get; set; }

    /// <summary>
    /// Default timeout for SMTP operations in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
