namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Request model for creating a new case
/// </summary>
public class CreateCaseRequest
{
    public string? AuthNumber { get; set; }
    public string? AccountNr { get; set; }
    public int? MemberId { get; set; }
    public int? ReferToId { get; set; }
    public int? ReferFromId { get; set; }
    public DateOnly? AdmissionDate { get; set; }
    public TimeOnly? AdmissionTime { get; set; }
    public DateOnly? DischargeDate { get; set; }
    public TimeOnly? DischargeTime { get; set; }
    public int? AuthTypeId { get; set; }
    public int? CaseTypeId { get => AuthTypeId; set => AuthTypeId = value; }
    public bool? WcaIod { get; set; }
    public decimal? TotalLengthOfStay { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? FinalInvoiceAmount { get; set; }
    public string? FinalInvoiceAmountUpdated { get; set; }
    public int? StatusId { get; set; }
    public int? CaseStatusId { get => StatusId; set => StatusId = value; }
    public string? CaseDescription { get; set; }
    public string? Changes { get; set; }
    public string? Limits { get; set; }
    public string? Exclusions { get; set; }
    public DateOnly? DateCreated { get; set; }
    public bool? HasBooking { get; set; }
    public DateTime? ChangeToCaseDate { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    public int? CaseCategoryId { get; set; }
    // Frontend sends dates as strings
    public string? DateAdmitted { get; set; }
    public string? DateDischarged { get; set; }
}
