namespace MedManage.Core.DTOs.Member;

/// <summary>
/// Search criteria for members with pagination
/// </summary>
public class MemberSearchRequest
{
    public string? MemberNumber { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Idnumber { get; set; }
    public int? MedicalAidId { get; set; }
    public int? MemberStatusId { get; set; }
    public bool? HasMedicalAid { get; set; }
    public bool? Suspended { get; set; }
    public bool? Deceased { get; set; }
    public bool? IncludeDeleted { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
