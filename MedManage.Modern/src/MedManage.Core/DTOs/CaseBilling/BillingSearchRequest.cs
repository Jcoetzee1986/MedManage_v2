namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// Request model for searching billing records by provider, dates, account number, and status
/// </summary>
public class BillingSearchRequest
{
    // Provider filter
    public int? ServiceProviderId { get; set; }
    public string? ProviderName { get; set; }

    // Member filter
    public string? MemberName { get; set; }
    public string? MemberNumber { get; set; }

    // Account/Invoice filters
    public string? AccountNumber { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Remittance { get; set; }

    // Case filter
    public int? CaseId { get; set; }

    // Status filter
    public int? BillingStatusId { get; set; }
    public bool? Paid { get; set; }
    public bool? Submitted { get; set; }

    // Date filters
    public DateOnly? DateReceivedFrom { get; set; }
    public DateOnly? DateReceivedTo { get; set; }
    public DateOnly? DateSubmittedFrom { get; set; }
    public DateOnly? DateSubmittedTo { get; set; }
    public DateOnly? DatePaidFrom { get; set; }
    public DateOnly? DatePaidTo { get; set; }

    // Main client filter (active client context)
    public int? MainClientId { get; set; }

    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 30;

    // Sort
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}
