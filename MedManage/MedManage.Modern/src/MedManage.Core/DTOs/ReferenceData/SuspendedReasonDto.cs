namespace MedManage.Core.DTOs.ReferenceData;

public class SuspendedReasonDto
{
    public int SuspendedReasonId { get; set; }
    public string? SuspendedReason { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateSuspendedReasonDto
{
    public string? SuspendedReason { get; set; }
}

public class UpdateSuspendedReasonDto
{
    public string? SuspendedReason { get; set; }
}
