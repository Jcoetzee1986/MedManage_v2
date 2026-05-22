using System;

namespace MedManage.Core.DTOs.CaseExclusion;

public class CreateCaseExclusionDto
{
    public int CaseId { get; set; }
    public int ExclusionId { get; set; }
    public string? Comment { get; set; }
}

public class UpdateCaseExclusionDto
{
    public string? Comment { get; set; }
}

public class CaseExclusionDto
{
    public int CaseId { get; set; }
    public int ExclusionId { get; set; }
    public string? Comment { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}
