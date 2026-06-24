namespace MedManage.Core.DTOs.CodeLookup;

/// <summary>
/// Lightweight DTO for typeahead code lookup results
/// </summary>
public class CodeLookupDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Extended DTO for NAPPI code results (includes date range)
/// </summary>
public class NappiCodeLookupDto : CodeLookupDto
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
