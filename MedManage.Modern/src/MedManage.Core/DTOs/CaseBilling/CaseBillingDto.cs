using System;

namespace MedManage.Core.DTOs.CaseBilling;

public class CreateCaseBillingDto
{
    public int? CaseId { get; set; }
    public int? ServiceProviderId { get; set; }
    public DateOnly? AccountDate { get; set; }
    public DateOnly? AccountToDate { get; set; }
    public string? AccountNumber { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateOnly? DateReceived { get; set; }
    public bool? Submitted { get; set; }
    public DateOnly? DateSubmitted { get; set; }
    public bool? Reported { get; set; }
    public DateOnly? DateReported { get; set; }
    public string? ReceivedByName { get; set; }
    public decimal? AmountDue { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Rejected { get; set; }
    public bool? Paid { get; set; }
    public DateOnly? DatePaid { get; set; }
    public string? Remittance { get; set; }
    public decimal? FinalInvoiceAmountDue { get; set; }
    public int? BillingStatusId { get; set; }
    public string? PatientName { get; set; }
    public string? PatientInitials { get; set; }
    public string? PatientSurname { get; set; }
    public string? Comment { get; set; }
}

public class UpdateCaseBillingDto
{
    public int? CaseId { get; set; }
    public int? ServiceProviderId { get; set; }
    public DateOnly? AccountDate { get; set; }
    public DateOnly? AccountToDate { get; set; }
    public string? AccountNumber { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateOnly? DateReceived { get; set; }
    public bool? Submitted { get; set; }
    public DateOnly? DateSubmitted { get; set; }
    public bool? Reported { get; set; }
    public DateOnly? DateReported { get; set; }
    public string? ReceivedByName { get; set; }
    public decimal? AmountDue { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Rejected { get; set; }
    public bool? Paid { get; set; }
    public DateOnly? DatePaid { get; set; }
    public string? Remittance { get; set; }
    public decimal? FinalInvoiceAmountDue { get; set; }
    public int? BillingStatusId { get; set; }
    public string? PatientName { get; set; }
    public string? PatientInitials { get; set; }
    public string? PatientSurname { get; set; }
    public string? Comment { get; set; }
}

public class CaseBillingDto
{
    public int CaseBillingId { get; set; }
    public int? CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public int? ServiceProviderId { get; set; }
    public string? ProviderName { get; set; }
    public string? MemberName { get; set; }
    public string? BillingStatusName { get; set; }
    public DateOnly? AccountDate { get; set; }
    public DateOnly? AccountToDate { get; set; }
    public string? AccountNumber { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateOnly? DateReceived { get; set; }
    public bool? Submitted { get; set; }
    public DateOnly? DateSubmitted { get; set; }
    public bool? Reported { get; set; }
    public DateOnly? DateReported { get; set; }
    public string? ReceivedByName { get; set; }
    public decimal? AmountDue { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Rejected { get; set; }
    public bool? Paid { get; set; }
    public DateOnly? DatePaid { get; set; }
    public string? Remittance { get; set; }
    public decimal? FinalInvoiceAmountDue { get; set; }
    public int? BillingStatusId { get; set; }
    public string? PatientName { get; set; }
    public string? PatientInitials { get; set; }
    public string? PatientSurname { get; set; }
    public string? Comment { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}
