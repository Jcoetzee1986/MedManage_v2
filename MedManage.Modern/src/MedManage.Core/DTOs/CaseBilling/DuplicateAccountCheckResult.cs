namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// Result of a duplicate account number check
/// </summary>
public class DuplicateAccountCheckResult
{
    public bool IsDuplicate { get; set; }
    public List<CaseBillingDto> ExistingBillings { get; set; } = new();
    public string? Message { get; set; }
}
