namespace MedManage.Core.DTOs.ReferenceData;

public class MemberStatusDto
{
    public int MemberStatusId { get; set; }
    public string? MemberStatus { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateMemberStatusDto
{
    public string? MemberStatus { get; set; }
}

public class UpdateMemberStatusDto
{
    public int MemberStatusId { get; set; }
    public string? MemberStatus { get; set; }
}
