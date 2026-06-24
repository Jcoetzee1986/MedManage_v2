namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Optional configuration for which sub-entities to include in the copy.
/// If not specified, all sub-entities are copied by default.
/// </summary>
public class CaseCopyRequest
{
    /// <summary>Copy CPT codes. Defaults to true.</summary>
    public bool IncludeCptCodes { get; set; } = true;

    /// <summary>Copy ICD codes. Defaults to true.</summary>
    public bool IncludeIcdCodes { get; set; } = true;

    /// <summary>Copy tariffs. Defaults to true.</summary>
    public bool IncludeTariffs { get; set; } = true;

    /// <summary>Copy facility types. Defaults to true.</summary>
    public bool IncludeFacilityTypes { get; set; } = true;

    /// <summary>Copy exclusions. Defaults to true.</summary>
    public bool IncludeExclusions { get; set; } = true;

    /// <summary>Copy checklist items. Defaults to true.</summary>
    public bool IncludeChecklist { get; set; } = true;

    /// <summary>Copy notes. Defaults to true.</summary>
    public bool IncludeNotes { get; set; } = true;

    /// <summary>Copy comments. Defaults to true.</summary>
    public bool IncludeComments { get; set; } = true;

    /// <summary>Copy NAPPI codes. Defaults to true.</summary>
    public bool IncludeNappiCodes { get; set; } = true;

    /// <summary>Copy letter notes. Defaults to true.</summary>
    public bool IncludeLetterNotes { get; set; } = true;
}
