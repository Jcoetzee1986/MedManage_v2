namespace MedManage.Core.Interfaces;

/// <summary>
/// Service for retrieving current authenticated user information
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current authenticated user's ID
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Gets the current authenticated user's username
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Indicates whether a user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }
}
