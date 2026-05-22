namespace MedManage.Core.DTOs.CaseNote;

/// <summary>
/// DTO for CaseNote entity
/// </summary>
public class CaseNoteDto
{
    public int CaseNoteId { get; set; }
    public string? Note { get; set; }
    public DateTime? DateCreated { get; set; }
    public decimal? InterimAmount { get; set; }
    public int? CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public decimal? InterimHospital { get; set; }
    public decimal? InterimRadiology { get; set; }
    public decimal? InterimDialysis { get; set; }
    public decimal? InterimSpecialist { get; set; }
    public decimal? InterimPhysio { get; set; }
    public decimal? InterimTransport { get; set; }
    public decimal? InterimAccomodation { get; set; }
    public decimal? InterimScript { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

/// <summary>
/// DTO for creating a new CaseNote
/// </summary>
public class CreateCaseNoteDto
{
    public string? Note { get; set; }
    public DateTime? DateCreated { get; set; }
    public decimal? InterimAmount { get; set; }
    public int? CaseId { get; set; }
    public string? CaseNumber { get; set; }
    public decimal? InterimHospital { get; set; }
    public decimal? InterimRadiology { get; set; }
    public decimal? InterimDialysis { get; set; }
    public decimal? InterimSpecialist { get; set; }
    public decimal? InterimPhysio { get; set; }
    public decimal? InterimTransport { get; set; }
    public decimal? InterimAccomodation { get; set; }
    public decimal? InterimScript { get; set; }
}

/// <summary>
/// DTO for updating an existing CaseNote
/// </summary>
public class UpdateCaseNoteDto
{
    public string? Note { get; set; }
    public DateTime? DateCreated { get; set; }
    public decimal? InterimAmount { get; set; }
    public string? CaseNumber { get; set; }
    public decimal? InterimHospital { get; set; }
    public decimal? InterimRadiology { get; set; }
    public decimal? InterimDialysis { get; set; }
    public decimal? InterimSpecialist { get; set; }
    public decimal? InterimPhysio { get; set; }
    public decimal? InterimTransport { get; set; }
    public decimal? InterimAccomodation { get; set; }
    public decimal? InterimScript { get; set; }
}
