namespace MedManage.Core.DTOs.CaseLinkedFile;

public class CaseLinkedFileDto
{
    public int CaseLinkedFileId { get; set; }
    public int? CaseId { get; set; }
    public int? MemberId { get; set; }
    public string? FileType { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public DateTime? DateAdded { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCaseLinkedFileDto
{
    public int? CaseId { get; set; }
    public int? MemberId { get; set; }
    public string? FileType { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public DateTime? DateAdded { get; set; }
}

public class UpdateCaseLinkedFileDto
{
    public int? CaseId { get; set; }
    public int? MemberId { get; set; }
    public string? FileType { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public DateTime? DateAdded { get; set; }
}
