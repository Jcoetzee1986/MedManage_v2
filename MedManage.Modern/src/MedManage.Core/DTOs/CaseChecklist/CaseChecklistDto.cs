using System;

namespace MedManage.Core.DTOs.CaseChecklist;

public class CreateCaseChecklistDto
{
    public int CaseId { get; set; }
    public int ChecklistTemplateId { get; set; }
    public bool? Checked { get; set; }
    public DateTime? Date { get; set; }
    public string? Comments { get; set; }
    public bool? NotApplicable { get; set; }
}

public class UpdateCaseChecklistDto
{
    public bool? Checked { get; set; }
    public DateTime? Date { get; set; }
    public string? Comments { get; set; }
    public bool? NotApplicable { get; set; }
}

public class CaseChecklistDto
{
    public int CaseId { get; set; }
    public int ChecklistTemplateId { get; set; }
    public string? TemplateName { get; set; }
    public bool? Checked { get; set; }
    public DateTime? Date { get; set; }
    public string? Comments { get; set; }
    public bool? NotApplicable { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}
