using MedManage.Core.DTOs.Document;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service interface for generic document management (linked to case or member)
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Get documents for a specific entity
    /// </summary>
    Task<IEnumerable<DocumentDto>> GetByEntityAsync(string entityType, int entityId);

    /// <summary>
    /// Get document metadata by ID
    /// </summary>
    Task<DocumentDto?> GetByIdAsync(int id);

    /// <summary>
    /// Upload a document and link to an entity
    /// </summary>
    Task<DocumentDto> UploadAsync(Stream fileStream, string fileName, string contentType, long fileSize, string entityType, int entityId, string? userId);

    /// <summary>
    /// Get the raw file stream for download
    /// </summary>
    Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int id);

    /// <summary>
    /// Get thumbnail stream for image files
    /// </summary>
    Task<(Stream stream, string contentType)?> GetThumbnailAsync(int id);

    /// <summary>
    /// Delete a document
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
