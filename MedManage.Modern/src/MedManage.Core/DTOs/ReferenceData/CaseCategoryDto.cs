namespace MedManage.Core.DTOs.ReferenceData;

public class CaseCategoryDto
{
    public int CaseCategoryId { get; set; }
    public string? CaseCategory { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCaseCategoryDto
{
    public string? CaseCategory { get; set; }
}

public class UpdateCaseCategoryDto
{
    public string? CaseCategory { get; set; }
}
