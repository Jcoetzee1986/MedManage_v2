using System.Security.Cryptography;
using System.Text;

namespace MedManage.Infrastructure.Services;

/// <summary>
/// Service for secure password hashing using SHA1 (ASP.NET Membership legacy)
/// and SHA256 for new users
/// </summary>
public class PasswordHasher
{
    /// <summary>
    /// Hashes password using SHA1 for legacy ASP.NET Membership compatibility
    /// </summary>
    public static string HashPasswordLegacy(string password)
    {
        using var sha1 = SHA1.Create();
        var bytes = Encoding.Unicode.GetBytes(password);
        var hash = sha1.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Hashes password using SHA256 for new users
    /// </summary>
    public static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = password + salt;
        var bytes = Encoding.UTF8.GetBytes(saltedPassword);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Generates a random salt
    /// </summary>
    public static string GenerateSalt()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Verifies password against legacy SHA1 hash
    /// </summary>
    public static bool VerifyPasswordLegacy(string password, string hash)
    {
        var computedHash = HashPasswordLegacy(password);
        return computedHash == hash;
    }

    /// <summary>
    /// Verifies password against salted hash
    /// </summary>
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var computedHash = HashPassword(password, salt);
        return computedHash == hash;
    }
}
