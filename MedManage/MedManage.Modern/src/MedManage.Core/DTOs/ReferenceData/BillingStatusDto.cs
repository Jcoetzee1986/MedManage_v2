namespace MedManage.Core.DTOs.ReferenceData;

public class BillingStatusDto
{
    public int BillingStatusId { get; set; }
    public string? BillingStatus { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateBillingStatusDto
{
    public string? BillingStatus { get; set; }
}

public class UpdateBillingStatusDto
{
    public int BillingStatusId { get; set; }
    public string? BillingStatus { get; set; }
}
