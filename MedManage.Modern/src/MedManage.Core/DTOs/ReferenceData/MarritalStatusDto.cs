namespace MedManage.Core.DTOs.ReferenceData;

public class MarritalStatusDto
{
    public int MarritalStatusId { get; set; }
    public string? MarritalStatus { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateMarritalStatusDto
{
    public string? MarritalStatus { get; set; }
}

public class UpdateMarritalStatusDto
{
    public int MarritalStatusId { get; set; }
    public string? MarritalStatus { get; set; }
}
