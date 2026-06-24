using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseLinkedFile;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/cases/{caseId}/files")]
[Authorize]
public class CaseLinkedFileController : ControllerBase
{
    private readonly ICaseLinkedFileService _caseLinkedFileService;
    private readonly ILogger<CaseLinkedFileController> _logger;

    public CaseLinkedFileController(ICaseLinkedFileService caseLinkedFileService, ILogger<CaseLinkedFileController> logger)
    {
        _caseLinkedFileService = caseLinkedFileService;
        _logger = logger;
    }

    /// <summary>
    /// Get all linked files for a case
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseLinkedFileDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        try
        {
            var files = await _caseLinkedFileService.GetByCaseIdAsync(caseId);
            return Ok(ApiResponse<IEnumerable<CaseLinkedFileDto>>.SuccessResponse(files));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving files for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseLinkedFileDto>>.ErrorResponse("An error occurred while retrieving case files"));
        }
    }

    /// <summary>
    /// Get a specific linked file by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseLinkedFileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseLinkedFileDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int caseId, int id)
    {
        try
        {
            var file = await _caseLinkedFileService.GetByIdAsync(id);
            if (file == null || file.CaseId != caseId)
                return NotFound(ApiResponse<CaseLinkedFileDto>.ErrorResponse($"File with ID {id} not found for case {caseId}"));

            return Ok(ApiResponse<CaseLinkedFileDto>.SuccessResponse(file));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving file {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<CaseLinkedFileDto>.ErrorResponse("An error occurred while retrieving the file"));
        }
    }

    /// <summary>
    /// Upload a file and link it to a case
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(ApiResponse<CaseLinkedFileDto>), StatusCodes.Status201Created)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(int caseId, IFormFile file, [FromForm] int? memberId = null)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<CaseLinkedFileDto>.ErrorResponse("No file provided"));

            var dto = new CreateCaseLinkedFileDto
            {
                CaseId = caseId,
                MemberId = memberId,
                FileType = Path.GetExtension(file.FileName)?.TrimStart('.'),
                FileName = file.FileName,
                DateAdded = DateTime.UtcNow
            };

            // Store the file - use configured path or default
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "cases", caseId.ToString());
            Directory.CreateDirectory(uploadsPath);
            var filePath = Path.Combine(uploadsPath, $"{Guid.NewGuid()}_{file.FileName}");
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            dto.FilePath = filePath;

            var created = await _caseLinkedFileService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { caseId, id = created.CaseLinkedFileId }, ApiResponse<CaseLinkedFileDto>.SuccessResponse(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLinkedFileDto>.ErrorResponse("An error occurred while uploading the file"));
        }
    }

    /// <summary>
    /// Create a file link record (metadata only, no file upload)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CaseLinkedFileDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(int caseId, [FromBody] CreateCaseLinkedFileDto dto)
    {
        try
        {
            dto.CaseId = caseId;
            var created = await _caseLinkedFileService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { caseId, id = created.CaseLinkedFileId }, ApiResponse<CaseLinkedFileDto>.SuccessResponse(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating file link for case {CaseId}", caseId);
            return StatusCode(500, ApiResponse<CaseLinkedFileDto>.ErrorResponse("An error occurred while creating the file link"));
        }
    }

    /// <summary>
    /// Download a linked file
    /// </summary>
    [HttpGet("{id}/download")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(int caseId, int id)
    {
        try
        {
            var file = await _caseLinkedFileService.GetByIdAsync(id);
            if (file == null || file.CaseId != caseId)
                return NotFound(ApiResponse<object>.ErrorResponse($"File with ID {id} not found for case {caseId}"));

            if (string.IsNullOrEmpty(file.FilePath) || !System.IO.File.Exists(file.FilePath))
                return NotFound(ApiResponse<object>.ErrorResponse("File not found on disk"));

            var bytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
            var contentType = GetContentType(file.FileName ?? "file");
            return File(bytes, contentType, file.FileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while downloading the file"));
        }
    }

    /// <summary>
    /// Delete a linked file (soft delete metadata, optionally remove file from disk)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int caseId, int id)
    {
        try
        {
            var file = await _caseLinkedFileService.GetByIdAsync(id);
            if (file == null || file.CaseId != caseId)
                return NotFound(ApiResponse<bool>.ErrorResponse($"File with ID {id} not found for case {caseId}"));

            var result = await _caseLinkedFileService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"File with ID {id} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "File deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {Id} for case {CaseId}", id, caseId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the file"));
        }
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
        return ext switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }
}
