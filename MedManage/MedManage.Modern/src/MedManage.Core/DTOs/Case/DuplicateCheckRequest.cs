namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Request model for checking duplicate cases.
/// A duplicate is defined as: same member + same provider + same admission date.
/// </summary>
public class DuplicateCheckRequest
{
    public int MemberId { get; set; }
    public int? ReferToId { get; set; }
    public DateOnly AdmissionDate { get; set; }

    /// <summary>
    /// Optional: exclude this case ID from results (useful when editing an existing case)
    /// </summary>
    public int? ExcludeCaseId { get; set; }
}
