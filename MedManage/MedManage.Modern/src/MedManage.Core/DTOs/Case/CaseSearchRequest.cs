namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Request model for searching cases
/// </summary>
public class CaseSearchRequest
{
    public string? AuthNumber { get; set; }
    public int? MemberId { get; set; }
    public int? StatusId { get; set; }
    public int? ReferToId { get; set; }
    public int? ReferFromId { get; set; }
    public DateOnly? AdmissionDateFrom { get; set; }
    public DateOnly? AdmissionDateTo { get; set; }
    public DateOnly? DischargeDateFrom { get; set; }
    public DateOnly? DischargeDateTo { get; set; }
    public int? CaseCategoryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
