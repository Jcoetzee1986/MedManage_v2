namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// Request model for checking duplicate account numbers in billing records.
/// </summary>
public class CheckDuplicateAccountRequest
{
    /// <summary>
    /// The account number to check for duplicates.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Optional billing ID to exclude from duplicate check (used during updates).
    /// </summary>
    public int? ExcludeBillingId { get; set; }
}
