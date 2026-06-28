namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Request to generate or validate an auth number with medical aid prefix
/// </summary>
public class AuthNumberPrefixRequest
{
    /// <summary>
    /// The member whose medical aid prefix should be applied
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// The auth number to prefix (or validate prefix for)
    /// </summary>
    public string? AuthNumber { get; set; }
}
