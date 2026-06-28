using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// Request to update the remittance number on one or more billing records.
/// </summary>
public class RemittanceUpdateRequest
{
    /// <summary>
    /// The billing record IDs to update with the remittance number.
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one billing ID is required.")]
    public List<int> BillingIds { get; set; } = new();

    /// <summary>
    /// The remittance number from the funder/medical aid.
    /// </summary>
    [Required]
    [StringLength(150, ErrorMessage = "Remittance number cannot exceed 150 characters.")]
    public string RemittanceNumber { get; set; } = string.Empty;
}

/// <summary>
/// Result of a remittance update operation.
/// </summary>
public class RemittanceUpdateResult
{
    /// <summary>
    /// Number of billing records successfully updated.
    /// </summary>
    public int SuccessCount { get; set; }

    /// <summary>
    /// Number of billing records that failed (not found).
    /// </summary>
    public int FailedCount { get; set; }

    /// <summary>
    /// IDs that were successfully updated.
    /// </summary>
    public List<int> SuccessIds { get; set; } = new();

    /// <summary>
    /// IDs that could not be found.
    /// </summary>
    public List<int> FailedIds { get; set; } = new();

    /// <summary>
    /// Summary message.
    /// </summary>
    public string? Message { get; set; }
}
