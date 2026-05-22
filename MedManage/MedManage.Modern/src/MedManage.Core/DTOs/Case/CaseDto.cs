namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Case read model
/// </summary>
public class CaseDto
{
    public int CaseId { get; set; }
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
    public bool? WcaIod { get; set; }
    public decimal? TotalLengthOfStay { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? FinalInvoiceAmount { get; set; }
    public string? FinalInvoiceAmountUpdated { get; set; }
    public int? StatusId { get; set; }
    public string? CaseDescription { get; set; }
    public string? Changes { get; set; }
    public string? Limits { get; set; }
    public string? Exclusions { get; set; }
    public DateOnly? DateCreated { get; set; }
    public bool? HasBooking { get; set; }
    public DateTime? ChangeToCaseDate { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    public int? CaseCategoryId { get; set; }

    // Audit fields
    public DateTime DateInserted { get; set; }
    public string UserID { get; set; } = string.Empty;
    public DateTime? DateUpdated { get; set; }
    public string? UpdatedUserID { get; set; }
    public DateTime? DateDeleted { get; set; }
}
