namespace MedManage.Core.DTOs.Document;

/// <summary>
/// Represents a document linked to an entity (case or member)
/// </summary>
public class DocumentDto
{
    public int Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? FileType { get; set; }
    public long FileSize { get; set; }
    public string? ContentType { get; set; }
    public bool IsImage { get; set; }
    public bool HasThumbnail { get; set; }
    public DateTime DateUploaded { get; set; }
    public string? UploadedBy { get; set; }
}

/// <summary>
/// Request model for uploading a document
/// </summary>
public class UploadDocumentRequest
{
    /// <summary>
    /// Entity type: "case" or "member"
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the related entity
    /// </summary>
    public int EntityId { get; set; }
}
