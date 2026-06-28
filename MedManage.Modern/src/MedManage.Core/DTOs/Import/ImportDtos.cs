namespace MedManage.Core.DTOs.Import;

/// <summary>
/// Result of a file import operation
/// </summary>
public class ImportResultDto
{
    public int ImportHistoryId { get; set; }
    public string ImportType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; }
    public string? ImportedBy { get; set; }
    public int TotalRecords { get; set; }
    public int ImportedRecords { get; set; }
    public int SkippedRecords { get; set; }
    public int ErrorRecords { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorDetails { get; set; }
    public List<ImportValidationError> ValidationErrors { get; set; } = new();
}

/// <summary>
/// Validation error for a specific row in the import file
/// </summary>
public class ImportValidationError
{
    public int Row { get; set; }
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Value { get; set; }
}

/// <summary>
/// Import history entry for listing past imports
/// </summary>
public class ImportHistoryDto
{
    public int ImportHistoryId { get; set; }
    public string ImportType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; }
    public string? ImportedBy { get; set; }
    public int TotalRecords { get; set; }
    public int ImportedRecords { get; set; }
    public int SkippedRecords { get; set; }
    public int ErrorRecords { get; set; }
    public string Status { get; set; } = string.Empty;
}
