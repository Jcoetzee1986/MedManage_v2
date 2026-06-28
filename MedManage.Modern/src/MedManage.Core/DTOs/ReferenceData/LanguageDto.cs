namespace MedManage.Core.DTOs.ReferenceData;

public class LanguageDto
{
    public int LanguageId { get; set; }
    public string? Language { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateLanguageDto
{
    public string? Language { get; set; }
}

public class UpdateLanguageDto
{
    public string? Language { get; set; }
}
