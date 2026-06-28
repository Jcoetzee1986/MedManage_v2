namespace MedManage.Core.DTOs.CaseFacilityType;

/// <summary>
/// Read model for a case facility type (days-in-care record)
/// </summary>
public class CaseFacilityTypeDto
{
    public int CaseIdFacilityTypeId { get; set; }
    public int FacilityTypeId { get; set; }
    public int CaseId { get; set; }
    public DateTime DateAdmitted { get; set; }
    public DateTime? DateDischarged { get; set; }
    public decimal? Los { get; set; }
    public string? FacilityTypeCode { get; set; }
    public int? MinutesOnVentilator { get; set; }
    public string? Comments { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

/// <summary>
/// Request model for creating a case facility type record
/// </summary>
public class CreateCaseFacilityTypeRequest
{
    public int CaseId { get; set; }
    public int FacilityTypeId { get; set; }
    public DateTime DateAdmitted { get; set; }
    public DateTime? DateDischarged { get; set; }
    public decimal? Los { get; set; }
    public string? FacilityTypeCode { get; set; }
    public int? MinutesOnVentilator { get; set; }
    public string? Comments { get; set; }
}

/// <summary>
/// Request model for updating a case facility type record
/// </summary>
public class UpdateCaseFacilityTypeRequest
{
    public int FacilityTypeId { get; set; }
    public DateTime DateAdmitted { get; set; }
    public DateTime? DateDischarged { get; set; }
    public decimal? Los { get; set; }
    public string? FacilityTypeCode { get; set; }
    public int? MinutesOnVentilator { get; set; }
    public string? Comments { get; set; }
}
