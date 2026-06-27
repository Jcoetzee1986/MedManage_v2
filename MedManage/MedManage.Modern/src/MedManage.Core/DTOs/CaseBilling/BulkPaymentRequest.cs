using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// Request to mark one or more billing records as paid in bulk.
/// </summary>
public class BulkPaymentRequest
{
    /// <summary>
    /// The billing record IDs to mark as paid.
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one billing ID is required.")]
    public List<int> BillingIds { get; set; } = new();

    /// <summary>
    /// The payment amount applied. If null, each billing's AmountDue is used.
    /// </summary>
    public decimal? PaymentAmount { get; set; }

    /// <summary>
    /// The date of payment.
    /// </summary>
    [Required]
    public DateOnly DatePaid { get; set; }

    /// <summary>
    /// Optional comments about the payment.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Optional remittance number to assign.
    /// </summary>
    public string? Remittance { get; set; }
}

/// <summary>
/// Result of a bulk payment operation.
/// </summary>
public class BulkPaymentResult
{
    /// <summary>
    /// Number of billing records successfully marked as paid.
    /// </summary>
    public int SuccessCount { get; set; }

    /// <summary>
    /// Number of billing records that failed (not found or already paid).
    /// </summary>
    public int FailedCount { get; set; }

    /// <summary>
    /// IDs that were successfully updated.
    /// </summary>
    public List<int> SuccessIds { get; set; } = new();

    /// <summary>
    /// IDs that could not be found or were already paid.
    /// </summary>
    public List<int> FailedIds { get; set; } = new();

    /// <summary>
    /// Summary message.
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// A single item in a billing status import (from CSV).
/// </summary>
public class BillingStatusImportItem
{
    public int Id { get; set; }
    public bool Paid { get; set; }
    public string? DatePaid { get; set; }
    public string? RemittanceNumber { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? RejectedAmount { get; set; }
    public decimal? FinalInvoiceAmount { get; set; }
}
