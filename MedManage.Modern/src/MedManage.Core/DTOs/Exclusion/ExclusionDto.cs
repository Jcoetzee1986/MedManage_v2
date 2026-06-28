namespace MedManage.Core.DTOs.Exclusion;

public class ExclusionDto
{
    public int ExclusionId { get; set; }
    public string? Exclusion { get; set; }
    public string? ExclusionDescription { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateExclusionDto
{
    public string? Exclusion { get; set; }
    public string? ExclusionDescription { get; set; }
}

public class UpdateExclusionDto
{
    public int ExclusionId { get; set; }
    public string? Exclusion { get; set; }
    public string? ExclusionDescription { get; set; }
}
