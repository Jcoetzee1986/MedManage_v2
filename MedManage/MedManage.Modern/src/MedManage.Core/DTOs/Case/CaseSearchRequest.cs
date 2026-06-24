namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Request model for searching cases with 14+ filter parameters
/// </summary>
public class CaseSearchRequest
{
    // Core case filters
    public string? AuthNumber { get; set; }
    public int? MemberId { get; set; }
    public string? MemberNumber { get; set; }
    public string? MemberSurname { get; set; }
    public int? StatusId { get; set; }
    public int? CaseCategoryId { get; set; }
    public int? AuthTypeId { get; set; }

    // Provider filters
    public int? ReferToId { get; set; }
    public int? ReferFromId { get; set; }

    // Date filters
    public DateOnly? AdmissionDateFrom { get; set; }
    public DateOnly? AdmissionDateTo { get; set; }
    public DateOnly? DischargeDateFrom { get; set; }
    public DateOnly? DischargeDateTo { get; set; }
    public DateOnly? DateCreatedFrom { get; set; }
    public DateOnly? DateCreatedTo { get; set; }

    // Medical code filters
    public string? IcdCode { get; set; }
    public string? CptCode { get; set; }

    // Medical aid filter
    public int? MedicalAidId { get; set; }

    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    // Sort
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}
