namespace MedManage.Core.DTOs.ReferenceData;

public class RaceDto
{
    public int RaceId { get; set; }
    public string? Race { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateRaceDto
{
    public string? Race { get; set; }
}

public class UpdateRaceDto
{
    public string? Race { get; set; }
}
