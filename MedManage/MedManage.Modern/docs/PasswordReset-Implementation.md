# Password Reset Implementation

## Overview
The password reset feature allows users to recover their accounts via email. The system uses a PIN-based approach for security and simplicity.

## Architecture

### Components
1. **PasswordResetToken Entity** - Stores reset tokens and PINs
2. **EmailService** - Sends emails via SMTP with test mode support
3. **AuthService** - Handles password reset logic
4. **AuthController** - Exposes REST API endpoints

## Features

### Security Features
- **6-digit cryptographic PIN** - Generated using `RandomNumberGenerator` (100,000 to 999,999)
- **1-hour expiration** - Tokens expire after 60 minutes
- **Single-use tokens** - Marked as used after successful reset
- **Token invalidation** - Previous unused tokens invalidated when new request made
- **Email obfuscation** - Success message doesn't reveal if email exists
- **Account unlock** - Password reset unlocks locked accounts and resets failed login attempts

### Email Features
- **HTML email templates** - Professional styled emails
- **Test mode** - Redirect all emails to test address in development
- **Visual banner** - Test mode adds yellow banner showing original recipient
- **Configurable SMTP** - Settings via appsettings.json and environment variables
- **Timeout configuration** - Prevent hanging on slow SMTP servers

## Database Schema

### PasswordResetTokens Table
```sql
CREATE TABLE [dbo].[PasswordResetTokens]
(
    [TokenId] UNIQUEIDENTIFIER PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL FK -> aspnet_Users,
    [Token] NVARCHAR(100) NOT NULL UNIQUE,
    [Pin] NVARCHAR(6) NOT NULL UNIQUE,
    [Email] NVARCHAR(256) NOT NULL,
    [ExpiresAt] DATETIME NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0,
    [CreatedDate] DATETIME NOT NULL,
    -- Audit fields from BaseEntity
    [DateInserted] DATETIME NOT NULL,
    [CreatedByUserID] INT NULL,
    [DateUpdated] DATETIME NULL,
    [UpdatedByUserID] INT NULL
);
```

**Indexes:**
- Unique on `Token` (for future URL-based reset)
- Unique on `Pin` (for PIN validation)
- Non-unique on `UserId` (for user lookup)
- Composite on `Email, Pin` (common query pattern)

## Configuration

### appsettings.json
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "",
    "SmtpPassword": "",
    "EnableSsl": true,
    "FromEmail": "noreply@medmanage.com",
    "FromName": "MedManage",
    "UseTestMode": true,
    "TestEmailAddress": "test@medmanage.com",
    "TimeoutSeconds": 30
  }
}
```

### Environment Variables
Override SMTP settings with environment variables:
- `SMTP_HOST` - Override SmtpHost
- `SMTP_PORT` - Override SmtpPort
- `SMTP_USERNAME` - Override SmtpUsername
- `SMTP_PASSWORD` - Override SmtpPassword
- `SMTP_FROM_EMAIL` - Override FromEmail

### Test Mode
When `UseTestMode: true`:
- All emails redirect to `TestEmailAddress`
- Yellow banner added to email showing original recipient:
  ```
  TEST MODE: Originally for: user@example.com
  ```

## API Endpoints

### 1. Forgot Password
**POST** `/api/auth/forgot-password`

Initiates password reset by sending 6-digit PIN to user's email.

**Request:**
```json
{
  "email": "user@medmanage.com"
}
```

**Response (Success):**
```json
{
  "success": true,
  "message": "If the email exists in our system, a password reset PIN will be sent."
}
```

**Security Note:** Returns success even if email not found to prevent enumeration attacks.

### 2. Verify PIN (Optional)
**POST** `/api/auth/verify-pin`

Validates PIN before showing password reset form. Optional pre-check.

**Request:**
```json
{
  "email": "user@medmanage.com",
  "pin": "123456"
}
```

**Response:**
```json
{
  "valid": true
}
```

### 3. Reset Password
**POST** `/api/auth/reset-password`

Completes password reset using PIN.

**Request:**
```json
{
  "email": "user@medmanage.com",
  "pin": "123456",
  "newPassword": "NewPassword123!",
  "confirmPassword": "NewPassword123!"
}
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Password has been successfully reset. You can now login with your new password."
}
```

**Response (Invalid PIN):**
```json
{
  "success": false,
  "message": "Invalid or expired PIN."
}
```

## User Flow

### Frontend Flow
1. User clicks "Forgot Password" link
2. User enters email address
3. System sends PIN to email (if exists)
4. User checks email and copies 6-digit PIN
5. User enters PIN and new password in app
6. System validates PIN and updates password
7. User logs in with new password

### Backend Flow
```
ForgotPassword:
1. Query user by email
2. Generate 6-digit PIN
3. Generate 100-char token (for future URL reset)
4. Invalidate previous unused tokens
5. Save token to database (1-hour expiration)
6. Send email with PIN
7. Return success message

ResetPassword:
1. Query PasswordResetToken by email + PIN
2. Check token not used and not expired
3. Validate password match
4. Hash new password with new salt
5. Update aspnet_Membership password fields
6. Unlock account if locked
7. Reset failed login attempts
8. Mark token as used
9. Return success message
```

## Email Templates

### Password Reset PIN Email
```html
Subject: Password Reset Request - MedManage

<h2>Password Reset Request</h2>
<p>Hi [Username],</p>
<p>You requested to reset your password. Use the PIN below:</p>

<div style="background-color: #f0f0f0; padding: 20px; text-align: center;">
    <div style="font-size: 32pt; letter-spacing: 8px; font-weight: bold;">
        [123456]
    </div>
</div>

<p><strong>This PIN will expire in 1 hour.</strong></p>
<p>If you didn't request this, please ignore this email.</p>
```

### Test Mode Banner (Added in Development)
```html
<div style='background-color: #fff3cd; padding: 10px;'>
    <strong>TEST MODE:</strong> Originally for: <strong>user@example.com</strong>
</div>
```

## Security Considerations

### Best Practices Implemented
✅ Token-based reset (not password hints)  
✅ Time-limited tokens (1 hour)  
✅ Single-use tokens  
✅ Cryptographic PIN generation  
✅ Email enumeration prevention  
✅ Account unlock on successful reset  
✅ Failed attempt counter reset  
✅ Previous token invalidation  

### Future Enhancements
- Rate limiting on forgot password requests
- IP-based throttling
- Notification email when password changed
- Link-based reset as alternative to PIN
- SMS-based 2FA option
- Password strength validation
- Password history (prevent reuse)

## Testing

### Manual Testing with Test Mode
1. Set `UseTestMode: true` in appsettings.json
2. Configure `TestEmailAddress` to your test account
3. Request password reset for any email
4. Check test email account for PIN
5. Use PIN to complete reset

### Automated Testing
See `docs/api-tests.http` for REST Client test cases:
- Request PIN for valid email
- Verify PIN validation
- Reset password with valid PIN
- Test invalid PIN (should fail)
- Test mismatched passwords (should fail)
- Test non-existent email (should succeed with generic message)

### Database Verification
```sql
-- Check recent password reset tokens
SELECT 
    TokenId,
    UserId,
    Email,
    Pin,
    ExpiresAt,
    IsUsed,
    CreatedDate,
    CASE 
        WHEN ExpiresAt < GETUTCDATE() THEN 'Expired'
        WHEN IsUsed = 1 THEN 'Used'
        ELSE 'Active'
    END AS Status
FROM PasswordResetTokens
ORDER BY CreatedDate DESC;

-- Check for orphaned tokens (cleanup)
SELECT COUNT(*) AS OrphanedTokens
FROM PasswordResetTokens
WHERE ExpiresAt < GETUTCDATE() OR IsUsed = 1;

-- Cleanup old tokens (run periodically)
DELETE FROM PasswordResetTokens
WHERE ExpiresAt < DATEADD(day, -7, GETUTCDATE());
```

## Troubleshooting

### Email Not Sending
1. **Check SMTP credentials** - Verify username/password in appsettings.json or environment variables
2. **Check SMTP port** - Ensure port 587 (TLS) or 465 (SSL) is open
3. **Check EnableSsl** - Must be true for most providers
4. **Check logs** - EmailService logs all send attempts with errors
5. **Test SMTP connection** - Use telnet or SMTP test tool

### Gmail Configuration
For Gmail SMTP:
1. Enable 2-factor authentication
2. Generate App Password (not your regular password)
3. Use App Password in `SmtpPassword` setting
4. If blocked, enable "Less secure app access" (not recommended)

### PIN Not Working
1. **Check expiration** - PINs expire after 1 hour
2. **Check if used** - PINs are single-use only
3. **Check email** - Ensure PIN matches exactly (6 digits)
4. **Request new PIN** - Old PINs invalidated when new one requested

### Test Mode Issues
1. **Emails not redirecting** - Verify `UseTestMode: true`
2. **Banner not showing** - Check test email for yellow header
3. **Wrong recipient** - Check `TestEmailAddress` setting

## Dependencies

### NuGet Packages
- System.Net.Mail (built-in) - SMTP email sending
- Microsoft.Extensions.Options (built-in) - Configuration binding
- Microsoft.Extensions.Logging (built-in) - Logging

### Project References
- MedManage.Core - DTOs, interfaces, configuration models
- MedManage.Infrastructure - Entity, service implementation
- MedManage.API - Controller endpoints

## Related Documentation
- [Phase 2 Authentication Complete](Phase2-Authentication-Complete.md)
- [API Tests](api-tests.http)
- [Database Scripts](database/CreatePasswordResetTokensTable.sql)
