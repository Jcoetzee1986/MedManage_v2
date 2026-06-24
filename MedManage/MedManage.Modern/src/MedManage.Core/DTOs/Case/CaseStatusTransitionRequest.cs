namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Request to transition a case from one status to another (e.g., Booking → Case)
/// </summary>
public class CaseStatusTransitionRequest
{
    /// <summary>The target status ID to transition the case to</summary>
    public int TargetStatusId { get; set; }

    /// <summary>Optional: the target status name (alternative to ID, e.g. "Case", "Booking")</summary>
    public string? TargetStatusName { get; set; }
}
