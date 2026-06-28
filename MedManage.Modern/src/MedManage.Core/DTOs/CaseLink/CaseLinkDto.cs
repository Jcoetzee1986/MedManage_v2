namespace MedManage.Core.DTOs.CaseLink;

public class CaseLinkDto
{
    public int ParentCase { get; set; }
    public int ChildCase { get; set; }
    public DateTime? DateInserted { get; set; }
}

public class CreateCaseLinkDto
{
    public int ChildCase { get; set; }
}

public class LinkedCaseDto
{
    public int CaseId { get; set; }
    public string? Relationship { get; set; } // "parent" or "child"
    public string? AuthNumber { get; set; }
    public string? AdmissionDate { get; set; }
    public string? CaseStatus { get; set; }
    public string? CaseType { get; set; }
    public string? CaseCategory { get; set; }
    public string? ReferToPractice { get; set; }
    public string? MemberName { get; set; }
}
