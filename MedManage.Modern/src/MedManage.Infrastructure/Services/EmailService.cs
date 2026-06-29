using System.Net;
using System.Net.Mail;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedManage.Core.Configuration;
using MedManage.Core.Interfaces;

namespace MedManage.Infrastructure.Services;

/// <summary>
/// Email service implementation using SMTP with template support
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;
    private readonly string _templateBasePath;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
        
        // Get template path relative to assembly location
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _templateBasePath = Path.Combine(assemblyLocation ?? "", "Templates", "Email");
    }

    private async Task<string> LoadTemplateAsync(string templateName)
    {
        try
        {
            var templatePath = Path.Combine(_templateBasePath, templateName);
            
            if (!File.Exists(templatePath))
            {
                _logger.LogWarning("Email template not found at: {TemplatePath}", templatePath);
                return string.Empty;
            }

            return await File.ReadAllTextAsync(templatePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load email template: {TemplateName}", templateName);
            return string.Empty;
        }
    }

    private string ReplaceTemplatePlaceholders(string template, Dictionary<string, string> replacements)
    {
        var result = template;
        foreach (var replacement in replacements)
        {
            result = result.Replace($"{{{{{replacement.Key}}}}}", replacement.Value);
        }
        return result;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        try
        {
            // In test mode, redirect to test email address
            var actualRecipient = to;
            if (_emailSettings.UseTestMode && !string.IsNullOrEmpty(_emailSettings.TestEmailAddress))
            {
                _logger.LogInformation("Test mode enabled. Redirecting email from {OriginalRecipient} to {TestRecipient}", 
                    to, _emailSettings.TestEmailAddress);
                actualRecipient = _emailSettings.TestEmailAddress;
                
                // Add original recipient info to body
                body = $"<div style='background-color: #fff3cd; padding: 10px; margin-bottom: 20px; border: 1px solid #ffc107;'>" +
                       $"<strong>TEST MODE:</strong> This email was originally intended for: <strong>{to}</strong>" +
                       $"</div>" + body;
            }

            using var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            message.To.Add(actualRecipient);

            using var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                Timeout = _emailSettings.TimeoutSeconds * 1000
            };

            await smtpClient.SendMailAsync(message);

            _logger.LogInformation("Email sent successfully to {Recipient} with subject: {Subject}", 
                actualRecipient, subject);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient} with subject: {Subject}. SMTP: {SmtpHost}:{SmtpPort}, From: {FromEmail}", 
                to, subject, _emailSettings.SmtpHost, _emailSettings.SmtpPort, _emailSettings.FromEmail);
            return false;
        }
    }

    public async Task<bool> SendPasswordResetPinAsync(string to, string username, string pin)
    {
        var subject = "MedManage - Password Reset PIN";
        
        var template = await LoadTemplateAsync("PasswordResetPin.html");
        
        if (string.IsNullOrEmpty(template))
        {
            _logger.LogWarning("Password reset template not found, using fallback");
            template = GetPasswordResetPinFallbackTemplate();
        }

        var replacements = new Dictionary<string, string>
        {
            { "Username", username },
            { "Pin", pin },
            { "Year", DateTime.Now.Year.ToString() }
        };

        var body = ReplaceTemplatePlaceholders(template, replacements);

        return await SendEmailAsync(to, subject, body, isHtml: true);
    }

    public async Task<bool> SendWelcomeEmailAsync(string to, string username)
    {
        var subject = "Welcome to MedManage";
        
        var template = await LoadTemplateAsync("Welcome.html");
        
        if (string.IsNullOrEmpty(template))
        {
            _logger.LogError("Failed to load welcome email template, using fallback");
            // Fallback to inline template if file not found
            template = GetWelcomeEmailFallbackTemplate();
        }

        var replacements = new Dictionary<string, string>
        {
            { "Username", username },
            { "LoginUrl", "https://medmanage.com/login" }, // TODO: Get from configuration
            { "Year", DateTime.Now.Year.ToString() }
        };

        var body = ReplaceTemplatePlaceholders(template, replacements);

        return await SendEmailAsync(to, subject, body, isHtml: true);
    }

    public async Task<bool> SendWelcomeEmailWithPasswordAsync(string to, string username, string temporaryPassword)
    {
        var subject = "Welcome to MedManage - Your Account Has Been Created";
        
        var body = GetWelcomeWithPasswordFallbackTemplate();
        var replacements = new Dictionary<string, string>
        {
            { "Username", username },
            { "TemporaryPassword", temporaryPassword },
            { "LoginUrl", "https://medmanage.com/login" },
            { "Year", DateTime.Now.Year.ToString() }
        };

        body = ReplaceTemplatePlaceholders(body, replacements);

        return await SendEmailAsync(to, subject, body, isHtml: true);
    }

    public async Task<bool> SendAdminPasswordResetEmailAsync(string to, string username, string newPassword)
    {
        var subject = "MedManage - Your Password Has Been Reset";
        
        var body = GetAdminPasswordResetFallbackTemplate();
        var replacements = new Dictionary<string, string>
        {
            { "Username", username },
            { "NewPassword", newPassword },
            { "LoginUrl", "https://medmanage.com/login" },
            { "Year", DateTime.Now.Year.ToString() }
        };

        body = ReplaceTemplatePlaceholders(body, replacements);

        return await SendEmailAsync(to, subject, body, isHtml: true);
    }

    #region Fallback Templates
    
    private string GetPasswordResetPinFallbackTemplate()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #007bff; color: white; padding: 20px; text-align: center; }
        .content { background-color: #f8f9fa; padding: 30px; border-radius: 5px; margin-top: 20px; }
        .pin-box { background-color: #fff; border: 2px solid #007bff; padding: 20px; text-align: center; 
                    font-size: 32px; font-weight: bold; letter-spacing: 8px; margin: 20px 0; border-radius: 5px; }
        .warning { color: #dc3545; font-weight: bold; margin-top: 20px; }
        .footer { text-align: center; margin-top: 30px; font-size: 12px; color: #6c757d; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Password Reset Request</h1>
        </div>
        <div class='content'>
            <p>Hello <strong>{{Username}}</strong>,</p>
            <p>You have requested to reset your password. Please use the following PIN code to complete the password reset process:</p>
            
            <div class='pin-box'>
                {{Pin}}
            </div>
            
            <p><strong>This PIN will expire in 1 hour.</strong></p>
            
            <p>If you did not request this password reset, please ignore this email and your password will remain unchanged.</p>
            
            <div class='warning'>
                ⚠️ For security reasons, never share this PIN with anyone. MedManage staff will never ask for your PIN.
            </div>
        </div>
        <div class='footer'>
            <p>&copy; {{Year}} MedManage. All rights reserved.</p>
            <p>This is an automated message, please do not reply.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetWelcomeEmailFallbackTemplate()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #28a745; color: white; padding: 20px; text-align: center; }
        .content { background-color: #f8f9fa; padding: 30px; border-radius: 5px; margin-top: 20px; }
        .button { background-color: #28a745; color: white; padding: 12px 30px; text-decoration: none; 
                  border-radius: 5px; display: inline-block; margin-top: 20px; }
        .footer { text-align: center; margin-top: 30px; font-size: 12px; color: #6c757d; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to MedManage!</h1>
        </div>
        <div class='content'>
            <p>Hello <strong>{{Username}}</strong>,</p>
            <p>Thank you for registering with MedManage. Your account has been successfully created!</p>
            <p>You can now log in and start using our medical case management system to:</p>
            <ul>
                <li>Manage patient cases efficiently</li>
                <li>Track medical procedures and billing</li>
                <li>Access comprehensive reporting tools</li>
                <li>Collaborate with your healthcare team</li>
            </ul>
            <p>If you have any questions or need assistance, please don't hesitate to contact our support team.</p>
            <p style='text-align: center;'>
                <a href='{{LoginUrl}}' class='button'>Login to MedManage</a>
            </p>
        </div>
        <div class='footer'>
            <p>&copy; {{Year}} MedManage. All rights reserved.</p>
            <p>This is an automated message, please do not reply.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetWelcomeWithPasswordFallbackTemplate()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #28a745; color: white; padding: 20px; text-align: center; }
        .content { background-color: #f8f9fa; padding: 30px; border-radius: 5px; margin-top: 20px; }
        .password-box { background-color: #fff; border: 2px solid #28a745; padding: 15px; text-align: center; 
                    font-size: 18px; font-weight: bold; margin: 20px 0; border-radius: 5px; }
        .button { background-color: #28a745; color: white; padding: 12px 30px; text-decoration: none; 
                  border-radius: 5px; display: inline-block; margin-top: 20px; }
        .warning { color: #dc3545; font-weight: bold; margin-top: 20px; }
        .footer { text-align: center; margin-top: 30px; font-size: 12px; color: #6c757d; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to MedManage!</h1>
        </div>
        <div class='content'>
            <p>Hello <strong>{{Username}}</strong>,</p>
            <p>An administrator has created an account for you on MedManage. Here are your login details:</p>
            
            <p><strong>Username:</strong> {{Username}}</p>
            <div class='password-box'>
                Temporary Password: {{TemporaryPassword}}
            </div>
            
            <div class='warning'>
                ⚠️ Please change your password after your first login for security purposes.
            </div>
            
            <p style='text-align: center;'>
                <a href='{{LoginUrl}}' class='button'>Login to MedManage</a>
            </p>
        </div>
        <div class='footer'>
            <p>&copy; {{Year}} MedManage. All rights reserved.</p>
            <p>This is an automated message, please do not reply.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetAdminPasswordResetFallbackTemplate()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #007bff; color: white; padding: 20px; text-align: center; }
        .content { background-color: #f8f9fa; padding: 30px; border-radius: 5px; margin-top: 20px; }
        .password-box { background-color: #fff; border: 2px solid #007bff; padding: 15px; text-align: center; 
                    font-size: 18px; font-weight: bold; margin: 20px 0; border-radius: 5px; }
        .button { background-color: #007bff; color: white; padding: 12px 30px; text-decoration: none; 
                  border-radius: 5px; display: inline-block; margin-top: 20px; }
        .warning { color: #dc3545; font-weight: bold; margin-top: 20px; }
        .footer { text-align: center; margin-top: 30px; font-size: 12px; color: #6c757d; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Password Reset</h1>
        </div>
        <div class='content'>
            <p>Hello <strong>{{Username}}</strong>,</p>
            <p>Your password has been reset by an administrator. Here is your new password:</p>
            
            <div class='password-box'>
                New Password: {{NewPassword}}
            </div>
            
            <div class='warning'>
                ⚠️ Please change your password after logging in for security purposes.
            </div>
            
            <p style='text-align: center;'>
                <a href='{{LoginUrl}}' class='button'>Login to MedManage</a>
            </p>
        </div>
        <div class='footer'>
            <p>&copy; {{Year}} MedManage. All rights reserved.</p>
            <p>This is an automated message, please do not reply.</p>
        </div>
    </div>
</body>
</html>";
    }
    
    #endregion
}
