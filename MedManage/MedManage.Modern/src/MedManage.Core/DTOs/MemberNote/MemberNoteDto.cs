namespace MedManage.Core.DTOs.MemberNote;

/// <summary>
/// DTO for MemberNote entity
/// </summary>
public class MemberNoteDto
{
    public int MemberNoteId { get; set; }
    public string? Note { get; set; }
    public int? MemberId { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

/// <summary>
/// DTO for creating a new MemberNote
/// </summary>
public class CreateMemberNoteDto
{
    public string? Note { get; set; }
    public int? MemberId { get; set; }
    public DateTime? DateCreated { get; set; }
}

/// <summary>
/// DTO for updating an existing MemberNote
/// </summary>
public class UpdateMemberNoteDto
{
    public string? Note { get; set; }
    public DateTime? DateCreated { get; set; }
}
