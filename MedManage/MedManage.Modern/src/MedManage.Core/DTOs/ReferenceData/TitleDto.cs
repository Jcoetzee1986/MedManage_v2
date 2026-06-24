namespace MedManage.Core.DTOs.ReferenceData;

public class TitleDto
{
    public int TitleId { get; set; }
    public string? Title { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateTitleDto
{
    public string? Title { get; set; }
}

public class UpdateTitleDto
{
    public string? Title { get; set; }
}
