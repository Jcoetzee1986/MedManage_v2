namespace MedManage.Core.DTOs.MemberChronicIllness;

/// <summary>
/// DTO for MemberChronicIllness entity (junction table)
/// </summary>
public class MemberChronicIllnessDto
{
    public int MemberId { get; set; }
    public int ChronicIllnessId { get; set; }
    public string? ChronicIllnessName { get; set; }
    public string? ChronicIllnessDescription { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateDeleted { get; set; }
}

/// <summary>
/// DTO for assigning a chronic illness to a member
/// </summary>
public class CreateMemberChronicIllnessDto
{
    public int ChronicIllnessId { get; set; }
}

