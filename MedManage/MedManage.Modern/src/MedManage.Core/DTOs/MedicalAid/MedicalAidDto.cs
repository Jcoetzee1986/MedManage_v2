namespace MedManage.Core.DTOs.MedicalAid;

public class MedicalAidDto
{
    public int MedicalAidId { get; set; }
    public int? MainClientId { get; set; }
    public string? MedicalAidName { get; set; }
    public DateOnly? MedicalAidInitiationDate { get; set; }
    public DateOnly? MedicalAidReinstatedDate { get; set; }
    public DateOnly? MedicalAidTerminatedDate { get; set; }
    public string? CasePrefix { get; set; }
    public string? ReportTemplate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateMedicalAidDto
{
    public int? MainClientId { get; set; }
    public string? MedicalAidName { get; set; }
    public DateOnly? MedicalAidInitiationDate { get; set; }
    public DateOnly? MedicalAidReinstatedDate { get; set; }
    public DateOnly? MedicalAidTerminatedDate { get; set; }
    public string? CasePrefix { get; set; }
    public string? ReportTemplate { get; set; }
}

public class UpdateMedicalAidDto
{
    public int MedicalAidId { get; set; }
    public int? MainClientId { get; set; }
    public string? MedicalAidName { get; set; }
    public DateOnly? MedicalAidInitiationDate { get; set; }
    public DateOnly? MedicalAidReinstatedDate { get; set; }
    public DateOnly? MedicalAidTerminatedDate { get; set; }
    public string? CasePrefix { get; set; }
    public string? ReportTemplate { get; set; }
}

public class MedicalAidSearchFilters
{
    public string? MedicalAidName { get; set; }
    public bool? ActiveOnly { get; set; } = true;
    public bool IncludeDeleted { get; set; } = false;
}
