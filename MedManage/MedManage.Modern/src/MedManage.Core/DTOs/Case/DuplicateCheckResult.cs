namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Result of a duplicate case check
/// </summary>
public class DuplicateCheckResult
{
    public bool HasDuplicates { get; set; }
    public List<CaseDto> PossibleDuplicates { get; set; } = new();
    public string? Message { get; set; }
}
