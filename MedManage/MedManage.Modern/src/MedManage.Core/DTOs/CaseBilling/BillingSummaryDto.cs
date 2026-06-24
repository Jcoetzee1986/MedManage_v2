namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// Aggregated billing summary for a case
/// </summary>
public class BillingSummaryDto
{
    public int CaseId { get; set; }
    public int TotalBillings { get; set; }
    public decimal TotalAmountDue { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalPenalty { get; set; }
    public decimal TotalRejected { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalOutstanding { get; set; }
    public int PaidCount { get; set; }
    public int SubmittedCount { get; set; }
    public int PendingCount { get; set; }
    public List<CaseBillingDto> Billings { get; set; } = new();
}
