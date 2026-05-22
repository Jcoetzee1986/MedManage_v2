namespace MedManage.Core.DTOs.ReferenceData;

public class CaseStatusDto
{
    public int CaseStatusId { get; set; }
    public string? CaseStatus { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCaseStatusDto
{
    public string? CaseStatus { get; set; }
}

public class UpdateCaseStatusDto
{
    public int CaseStatusId { get; set; }
    public string? CaseStatus { get; set; }
}
