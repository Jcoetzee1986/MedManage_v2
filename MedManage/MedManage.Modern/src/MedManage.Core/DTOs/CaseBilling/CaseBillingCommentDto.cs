namespace MedManage.Core.DTOs.CaseBilling;

public class CaseBillingCommentDto
{
    public int CaseBillingCommentId { get; set; }
    public int? CaseBillingId { get; set; }
    public string? Comment { get; set; }
    public DateTime? DateInserted { get; set; }
    public string? UserID { get; set; }
}

public class CreateCaseBillingCommentDto
{
    public int? CaseBillingId { get; set; }
    public string? Comment { get; set; }
}
