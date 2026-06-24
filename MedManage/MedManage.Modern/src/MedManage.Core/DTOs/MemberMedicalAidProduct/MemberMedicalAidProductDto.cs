namespace MedManage.Core.DTOs.MemberMedicalAidProduct;

/// <summary>
/// DTO for MemberMedicalAidProduct entity
/// </summary>
public class MemberMedicalAidProductDto
{
    public int MedAidProductIdMemberId { get; set; }
    public int? MedAidProductId { get; set; }
    public int? MemberId { get; set; }
    public string StartDate { get; set; } = null!;
    public DateOnly? EndDate { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateDeleted { get; set; }
}

/// <summary>
/// DTO for creating a member medical aid product history record
/// </summary>
public class CreateMemberMedicalAidProductDto
{
    public int? MedAidProductId { get; set; }
    public string StartDate { get; set; } = null!;
    public DateOnly? EndDate { get; set; }
}

/// <summary>
/// DTO for updating a member medical aid product history record
/// </summary>
public class UpdateMemberMedicalAidProductDto
{
    public int? MedAidProductId { get; set; }
    public string? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
