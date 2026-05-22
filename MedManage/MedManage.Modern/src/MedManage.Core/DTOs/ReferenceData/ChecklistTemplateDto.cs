namespace MedManage.Core.DTOs.ReferenceData;

public class ChecklistTemplateDto
{
    public int ChecklistTemplateId { get; set; }
    public string? ChecklistPrompt { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateChecklistTemplateDto
{
    public string? ChecklistPrompt { get; set; }
}

public class UpdateChecklistTemplateDto
{
    public int ChecklistTemplateId { get; set; }
    public string? ChecklistPrompt { get; set; }
}
