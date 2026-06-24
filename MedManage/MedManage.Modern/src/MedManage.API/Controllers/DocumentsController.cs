using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.Document;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

/// <summary>
/// Generic document management controller.
/// Supports file upload/download linked to cases or members, with image thumbnail generation.
/// </summary>
[ApiController]
[Route("api/documents")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentsController> _logger;

    private static readonly HashSet<string> ValidEntityTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "case", "member"
    };

    public DocumentsController(IDocumentService documentService, ILogger<DocumentsController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    /// <summary>
    /// List documents for a given entity (case or member)
    /// </summary>
    /// <param name="entityType">Entity type: "case" or "member"</param>
    /// <param name="entityId">The entity ID</param>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<DocumentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEntity([FromQuery] string entityType, [FromQuery] int entityId)
    {
        try
        {
            if (string.IsNullOrEmpty(entityType) || !ValidEntityTypes.Contains(entityType))
                return BadRequest(ApiResponse<IEnumerable<DocumentDto>>.ErrorResponse("entityType must be 'case' or 'member'"));

            if (entityId <= 0)
                return BadRequest(ApiResponse<IEnumerable<DocumentDto>>.ErrorResponse("entityId must be a positive integer"));

            var documents = await _documentService.GetByEntityAsync(entityType, entityId);
            return Ok(ApiResponse<IEnumerable<DocumentDto>>.SuccessResponse(documents));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving documents for {EntityType} {EntityId}", entityType, entityId);
            return StatusCode(500, ApiResponse<IEnumerable<DocumentDto>>.ErrorResponse("An error occurred while retrieving documents"));
        }
    }

    /// <summary>
    /// Get document metadata by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(int id)
    {
        try
        {
            var result = await _documentService.DownloadAsync(id);
            if (result == null)
                return NotFound(ApiResponse<object>.ErrorResponse($"Document with ID {id} not found"));

            var (stream, contentType, fileName) = result.Value;
            return File(stream, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading document {Id}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while downloading the document"));
        }
    }

    /// <summary>
    /// Get thumbnail for an image document
    /// </summary>
    [HttpGet("{id:int}/thumbnail")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThumbnail(int id)
    {
        try
        {
            var result = await _documentService.GetThumbnailAsync(id);
            if (result == null)
                return NotFound(ApiResponse<object>.ErrorResponse($"Thumbnail not available for document {id}"));

            var (stream, contentType) = result.Value;
            return File(stream, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving thumbnail for document {Id}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the thumbnail"));
        }
    }

    /// <summary>
    /// Upload a document with metadata (multipart/form-data)
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<DocumentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Upload(IFormFile file, [FromForm] string entityType, [FromForm] int entityId)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<DocumentDto>.ErrorResponse("No file provided"));

            if (string.IsNullOrEmpty(entityType) || !ValidEntityTypes.Contains(entityType))
                return BadRequest(ApiResponse<DocumentDto>.ErrorResponse("entityType must be 'case' or 'member'"));

            if (entityId <= 0)
                return BadRequest(ApiResponse<DocumentDto>.ErrorResponse("entityId must be a positive integer"));

            var userId = User.Identity?.Name;

            using var stream = file.OpenReadStream();
            var document = await _documentService.UploadAsync(
                stream, file.FileName, file.ContentType, file.Length,
                entityType, entityId, userId);

            return CreatedAtAction(nameof(Download), new { id = document.Id }, ApiResponse<DocumentDto>.SuccessResponse(document));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document for {EntityType} {EntityId}", entityType, entityId);
            return StatusCode(500, ApiResponse<DocumentDto>.ErrorResponse("An error occurred while uploading the document"));
        }
    }

    /// <summary>
    /// Delete a document (soft delete)
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _documentService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"Document with ID {id} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Document deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document {Id}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the document"));
        }
    }
}
