namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Result of checking a member's eligibility for services
/// </summary>
public class MemberStatusCheckResult
{
    public int MemberId { get; set; }
    public bool AllowServices { get; set; }
    public bool IsSuspended { get; set; }
    public bool IsDeceased { get; set; }
    public bool IsMedicalAidExhausted { get; set; }
    public bool IsActive { get; set; }
    public string? MemberName { get; set; }
    public string? ProductName { get; set; }
    public string? MedicalAidName { get; set; }
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// Whether the member is eligible for case creation (AllowServices = true, not suspended, not deceased)
    /// </summary>
    public bool IsEligible => AllowServices && !IsSuspended && !IsDeceased;
}
