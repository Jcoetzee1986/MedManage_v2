namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Result of auth number prefix generation/validation
/// </summary>
public class AuthNumberPrefixResult
{
    /// <summary>
    /// The generated auth number with prefix applied
    /// </summary>
    public string? GeneratedAuthNumber { get; set; }

    /// <summary>
    /// The prefix from the medical aid (CasePrefix field)
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// The base auth number without prefix
    /// </summary>
    public string? BaseNumber { get; set; }

    /// <summary>
    /// Whether the auth number already contains the correct prefix
    /// </summary>
    public bool HasCorrectPrefix { get; set; }

    /// <summary>
    /// The medical aid name that owns this prefix
    /// </summary>
    public string? MedicalAidName { get; set; }
}
