namespace MedManage.Core.DTOs.CaseComment;

public class CaseCommentDto
{
    public int CaseCommentId { get; set; }
    public string? Comment { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CaseId { get; set; }
    public string? UserID { get; set; }
    public string? UserName { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCaseCommentDto
{
    public string? Comment { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CaseId { get; set; }
}

public class UpdateCaseCommentDto
{
    public string? Comment { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CaseId { get; set; }
}
