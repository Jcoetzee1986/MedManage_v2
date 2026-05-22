using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MedManage.Core.DTOs.Auth;
using MedManage.Core.Interfaces;
using MedManage.Core.Entities;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services;

/// <summary>
/// Authentication service implementation with JWT token generation
/// </summary>
public class AuthService : IAuthService
{
    private readonly MedManageDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        MedManageDbContext context, 
        IConfiguration configuration,
        IEmailService emailService,
        ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            // Find user by username
            var user = await _context.AspnetUsers
                .Include(u => u.AspnetMembership)
                .Include(u => u.AspnetUsersInRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.LoweredUserName == request.Username.ToLower());

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var membership = user.AspnetMembership;
            if (membership == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User account not found"
                };
            }

            // Check if account is locked
            if (membership.IsLockedOut)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Account is locked. Please contact administrator."
                };
            }

            // Check if account is approved
            if (!membership.IsApproved)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Account is not approved"
                };
            }

            // Verify password based on format
            bool isPasswordValid = false;
            if (membership.PasswordFormat == 1) // Hashed
            {
                isPasswordValid = PasswordHasher.VerifyPassword(request.Password, membership.Password, membership.PasswordSalt);
            }
            else if (membership.PasswordFormat == 0) // Clear text (legacy)
            {
                isPasswordValid = membership.Password == request.Password;
            }

            if (!isPasswordValid)
            {
                // Increment failed login attempts
                membership.FailedPasswordAttemptCount++;
                membership.FailedPasswordAttemptWindowStart = DateTime.UtcNow;
                
                // Lock account after 5 failed attempts
                if (membership.FailedPasswordAttemptCount >= 5)
                {
                    membership.IsLockedOut = true;
                    membership.LastLockoutDate = DateTime.UtcNow;
                }
                
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Update last login date and reset failed attempts
            membership.LastLoginDate = DateTime.UtcNow;
            membership.FailedPasswordAttemptCount = 0;
            await _context.SaveChangesAsync();

            // Get user roles
            var roles = user.AspnetUsersInRoles
                .Select(ur => ur.Role.RoleName)
                .ToList();

            // Generate JWT access token
            var token = GenerateJwtToken(user.UserId, user.UserName, roles);
            var expiresAt = DateTime.UtcNow.AddMinutes(15);

            // Generate refresh token
            var refreshToken = await GenerateRefreshTokenAsync(user.UserId);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                Message = "Login successful",
                User = new UserInfo
                {
                    UserId = user.UserId,
                    Username = user.UserName,
                    Email = membership.Email,
                    Roles = roles
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"An error occurred during login: {ex.Message}"
            };
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Validate passwords match
            if (request.Password != request.ConfirmPassword)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Passwords do not match"
                };
            }

            // Check if username already exists
            if (await UserExistsAsync(request.Username))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            // Check if email already exists
            var existingEmail = await _context.AspnetMemberships
                .FirstOrDefaultAsync(m => m.LoweredEmail == request.Email.ToLower());
            
            if (existingEmail != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email already registered"
                };
            }

            // Get default application
            var application = await _context.AspnetApplications.FirstOrDefaultAsync();
            if (application == null)
            {
                // Create default application if it doesn't exist
                application = new AspnetApplication
                {
                    ApplicationId = Guid.NewGuid(),
                    ApplicationName = "/",
                    LoweredApplicationName = "/",
                    Description = "MedManage Application"
                };
                _context.AspnetApplications.Add(application);
                await _context.SaveChangesAsync();
            }

            var userId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            // Create user
            var user = new AspnetUser
            {
                ApplicationId = application.ApplicationId,
                UserId = userId,
                UserName = request.Username,
                LoweredUserName = request.Username.ToLower(),
                IsAnonymous = false,
                LastActivityDate = now
            };

            // Generate salt and hash password
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(request.Password, salt);

            // Create membership
            var membership = new AspnetMembership
            {
                ApplicationId = application.ApplicationId,
                UserId = userId,
                Password = hashedPassword,
                PasswordSalt = salt,
                PasswordFormat = 1, // Hashed
                Email = request.Email,
                LoweredEmail = request.Email.ToLower(),
                IsApproved = true,
                IsLockedOut = false,
                CreateDate = now,
                LastLoginDate = now,
                LastPasswordChangedDate = now,
                LastLockoutDate = DateTime.MinValue,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = DateTime.MinValue,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = DateTime.MinValue
            };

            _context.AspnetUsers.Add(user);
            _context.AspnetMemberships.Add(membership);
            await _context.SaveChangesAsync();

            // Generate JWT access token
            var token = GenerateJwtToken(userId, request.Username, new List<string>());
            var expiresAt = DateTime.UtcNow.AddMinutes(15);

            // Generate refresh token
            var refreshToken = await GenerateRefreshTokenAsync(userId);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                Message = "Registration successful",
                User = new UserInfo
                {
                    UserId = userId,
                    Username = request.Username,
                    Email = request.Email,
                    Roles = new List<string>()
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"An error occurred during registration: {ex.Message}"
            };
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        try
        {
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return false;
            }

            var membership = await _context.AspnetMemberships
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (membership == null)
            {
                return false;
            }

            // Verify current password
            bool isCurrentPasswordValid = PasswordHasher.VerifyPassword(
                request.CurrentPassword,
                membership.Password,
                membership.PasswordSalt
            );

            if (!isCurrentPasswordValid)
            {
                return false;
            }

            // Generate new salt and hash new password
            var newSalt = PasswordHasher.GenerateSalt();
            var newHashedPassword = PasswordHasher.HashPassword(request.NewPassword, newSalt);

            membership.Password = newHashedPassword;
            membership.PasswordSalt = newSalt;
            membership.LastPasswordChangedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.AspnetUsers
            .AnyAsync(u => u.LoweredUserName == username.ToLower());
    }

    public async Task<UserInfo?> GetUserInfoAsync(Guid userId)
    {
        var user = await _context.AspnetUsers
            .Include(u => u.AspnetMembership)
            .Include(u => u.AspnetUsersInRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null || user.AspnetMembership == null)
        {
            return null;
        }

        return new UserInfo
        {
            UserId = user.UserId,
            Username = user.UserName,
            Email = user.AspnetMembership.Email,
            Roles = user.AspnetUsersInRoles.Select(ur => ur.Role.RoleName).ToList()
        };
    }

    public string GenerateJwtToken(Guid userId, string username, IEnumerable<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "MedManageAPI";
        var audience = jwtSettings["Audience"] ?? "MedManageClient";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Short-lived access token
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId, string? ipAddress = null, string? userAgent = null)
    {
        // Generate cryptographically secure random token
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);

        // Create refresh token entity
        var refreshToken = new RefreshToken
        {
            TokenId = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            CreatedDate = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // 7-day refresh token lifetime
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsUsed = false,
            //UserID = userId.ToString(),
            DateInserted = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Generated refresh token for user {UserId}", userId);

        return token;
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Find the refresh token
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                    .ThenInclude(u => u.AspnetUsersInRoles)
                        .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token == null || !token.IsActive)
            {
                _logger.LogWarning("Invalid or inactive refresh token attempted");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var user = token.User;

            // Mark old token as used
            token.IsUsed = true;
            token.DateUpdated = DateTime.UtcNow;
            //token.UpdatedUserID = user.UserId.ToString();

            // Generate new tokens
            var roles = user.AspnetUsersInRoles.Select(ur => ur.Role.RoleName).ToList();
            var accessToken = GenerateJwtToken(user.UserId, user.UserName, roles);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.UserId, ipAddress, userAgent);

            // Link old token to new token (rotation chain)
            var newTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == newRefreshToken);
            
            if (newTokenEntity != null)
            {
                token.ReplacedByTokenId = newTokenEntity.TokenId;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Refreshed tokens for user {UserId}", user.UserId);

            return new AuthResponse
            {
                Success = true,
                Token = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                User = new UserInfo
                {
                    UserId = user.UserId,
                    Username = user.UserName,
                    Email = user.AspnetMembership?.Email,
                    Roles = roles
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while refreshing token"
            };
        }
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, string? reason = null)
    {
        try
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token == null)
            {
                return false;
            }

            token.RevokedAt = DateTime.UtcNow;
            token.RevocationReason = reason ?? "Manually revoked";
            token.DateUpdated = DateTime.UtcNow;
            //token.UpdatedUserID = token.UserId.ToString();

            await _context.SaveChangesAsync();

            _logger.LogInformation("Revoked refresh token for user {UserId}", token.UserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking refresh token");
            return false;
        }
    }

    public async Task RevokeAllUserRefreshTokensAsync(Guid userId, string? reason = null)
    {
        try
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.RevocationReason = reason ?? "All tokens revoked";
                token.DateUpdated = DateTime.UtcNow;
                //token.UpdatedUserID = userId.ToString();
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Revoked all refresh tokens for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all refresh tokens for user {UserId}", userId);
            throw;
        }
    }

    public async Task<PasswordResetResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            // Find user by email
            var user = await _context.AspnetUsers
                .Include(u => u.AspnetMembership)
                .FirstOrDefaultAsync(u => u.AspnetMembership != null && 
                                        u.AspnetMembership.LoweredEmail == request.Email.ToLower());

            if (user?.AspnetMembership == null)
            {
                // Return success even if user not found (security best practice)
                return new PasswordResetResponse
                {
                    Success = true,
                    Message = "If the email exists in our system, a password reset PIN will be sent."
                };
            }

            var membership = user.AspnetMembership;

            // Generate 6-digit PIN
            var pin = GeneratePin();

            // Generate unique token for URL-based reset (future use)
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(75))
                .Replace("+", "-").Replace("/", "_").Replace("=", "")
                .Substring(0, 100);

            // Check for existing unused token and invalidate it
            var existingTokens = await _context.PasswordResetTokens
                .Where(t => t.UserId == user.UserId && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var existingToken in existingTokens)
            {
                existingToken.IsUsed = true;
            }

            // Create password reset token
            var resetToken = new PasswordResetToken
            {
                TokenId = Guid.NewGuid(),
                UserId = user.UserId,
                Token = token,
                Pin = pin,
                Email = membership!.Email!,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false,
                CreatedDate = DateTime.UtcNow,
                DateInserted = DateTime.UtcNow,
                DateUpdated = null,
                CreatedByUserID = null,
                UpdatedByUserID = null
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Send email with PIN
            await _emailService.SendPasswordResetPinAsync(membership!.Email!, user.UserName, pin);

            _logger.LogInformation("Password reset PIN sent to user {UserId}", user.UserId);

            return new PasswordResetResponse
            {
                Success = true,
                Message = "If the email exists in our system, a password reset PIN will be sent."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ForgotPasswordAsync for email {Email}", request.Email);
            return new PasswordResetResponse
            {
                Success = false,
                Message = "An error occurred while processing your request."
            };
        }
    }

    public async Task<PasswordResetResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            // Validate passwords match
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Passwords do not match."
                };
            }

            // Find valid reset token
            var resetToken = await _context.PasswordResetTokens
                .Include(t => t.User)
                    .ThenInclude(u => u.AspnetMembership)
                .FirstOrDefaultAsync(t => 
                    t.Email.ToLower() == request.Email.ToLower() &&
                    t.Pin == request.Pin &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (resetToken == null)
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Invalid or expired PIN."
                };
            }

            var membership = resetToken.User.AspnetMembership;
            if (membership == null)
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "User account not found."
                };
            }

            // Generate new salt and hash new password
            var newSalt = PasswordHasher.GenerateSalt();
            var newHashedPassword = PasswordHasher.HashPassword(request.NewPassword, newSalt);

            // Update password
            membership.Password = newHashedPassword;
            membership.PasswordSalt = newSalt;
            membership.LastPasswordChangedDate = DateTime.UtcNow;
            membership.IsLockedOut = false; // Unlock account if it was locked
            membership.FailedPasswordAttemptCount = 0; // Reset failed attempts

            // Mark token as used
            resetToken.IsUsed = true;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Password successfully reset for user {UserId}", resetToken.UserId);

            return new PasswordResetResponse
            {
                Success = true,
                Message = "Password has been successfully reset. You can now login with your new password."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ResetPasswordAsync for email {Email}", request.Email);
            return new PasswordResetResponse
            {
                Success = false,
                Message = "An error occurred while resetting your password."
            };
        }
    }

    public async Task<bool> VerifyResetPinAsync(string email, string pin)
    {
        try
        {
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t =>
                    t.Email.ToLower() == email.ToLower() &&
                    t.Pin == pin &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);

            return resetToken != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VerifyResetPinAsync for email {Email}", email);
            return false;
        }
    }

    private static string GeneratePin()
    {
        // Generate cryptographically secure 6-digit PIN
        var randomNumber = RandomNumberGenerator.GetInt32(100000, 1000000);
        return randomNumber.ToString();
    }
}

