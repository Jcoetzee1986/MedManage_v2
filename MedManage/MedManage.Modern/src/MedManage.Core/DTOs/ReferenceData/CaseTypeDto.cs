namespace MedManage.Core.DTOs.ReferenceData;

public class CaseTypeDto
{
    public int CaseTypeId { get; set; }
    public string? CaseType { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCaseTypeDto
{
    public string? CaseType { get; set; }
}

public class UpdateCaseTypeDto
{
    public int CaseTypeId { get; set; }
    public string? CaseType { get; set; }
}
