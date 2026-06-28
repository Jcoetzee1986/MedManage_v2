using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.Import;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/imports")]
[Authorize(Roles = "System Administrator,Imports")]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;
    private readonly ILogger<ImportController> _logger;

    public ImportController(IImportService importService, ILogger<ImportController> logger)
    {
        _importService = importService;
        _logger = logger;
    }

    /// <summary>
    /// Import DRD member file (CSV/delimited) - parse, validate, and merge into Members table
    /// </summary>
    [HttpPost("members")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<ImportResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ImportResultDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportMembers(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse("No file provided"));

            if (!IsValidImportFile(file))
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse("Invalid file type. Supported: .csv, .txt, .dat"));

            using var stream = file.OpenReadStream();
            var result = await _importService.ImportMembersAsync(stream, file.FileName);

            if (result.Status == "Failed")
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse(result.ErrorDetails ?? "Import failed", new List<string>()));

            return Ok(ApiResponse<ImportResultDto>.SuccessResponse(result, $"Import completed: {result.ImportedRecords} imported, {result.SkippedRecords} skipped, {result.ErrorRecords} errors"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing members file");
            return StatusCode(500, ApiResponse<ImportResultDto>.ErrorResponse("An error occurred during member import"));
        }
    }

    /// <summary>
    /// Import NAPPI code file - parse, validate, and update NappiCodes table
    /// </summary>
    [HttpPost("nappi")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<ImportResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ImportResultDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportNappiCodes(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse("No file provided"));

            if (!IsValidImportFile(file))
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse("Invalid file type. Supported: .csv, .txt, .dat"));

            using var stream = file.OpenReadStream();
            var result = await _importService.ImportNappiCodesAsync(stream, file.FileName);

            if (result.Status == "Failed")
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse(result.ErrorDetails ?? "Import failed", new List<string>()));

            return Ok(ApiResponse<ImportResultDto>.SuccessResponse(result, $"Import completed: {result.ImportedRecords} imported, {result.SkippedRecords} skipped, {result.ErrorRecords} errors"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing NAPPI codes file");
            return StatusCode(500, ApiResponse<ImportResultDto>.ErrorResponse("An error occurred during NAPPI code import"));
        }
    }

    /// <summary>
    /// Import billing file - parse, validate, and create billing records
    /// </summary>
    [HttpPost("billing")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<ImportResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ImportResultDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportBilling(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse("No file provided"));

            if (!IsValidImportFile(file))
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse("Invalid file type. Supported: .csv, .txt, .dat"));

            using var stream = file.OpenReadStream();
            var result = await _importService.ImportBillingAsync(stream, file.FileName);

            if (result.Status == "Failed")
                return BadRequest(ApiResponse<ImportResultDto>.ErrorResponse(result.ErrorDetails ?? "Import failed", new List<string>()));

            return Ok(ApiResponse<ImportResultDto>.SuccessResponse(result, $"Import completed: {result.ImportedRecords} imported, {result.SkippedRecords} skipped, {result.ErrorRecords} errors"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing billing file");
            return StatusCode(500, ApiResponse<ImportResultDto>.ErrorResponse("An error occurred during billing import"));
        }
    }

    /// <summary>
    /// Get import history - list past imports with results
    /// </summary>
    [HttpGet("history")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ImportHistoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory([FromQuery] string? importType = null, [FromQuery] int? limit = 50)
    {
        try
        {
            var history = await _importService.GetImportHistoryAsync(importType, limit);
            return Ok(ApiResponse<IEnumerable<ImportHistoryDto>>.SuccessResponse(history));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving import history");
            return StatusCode(500, ApiResponse<IEnumerable<ImportHistoryDto>>.ErrorResponse("An error occurred while retrieving import history"));
        }
    }

    private static bool IsValidImportFile(IFormFile file)
    {
        var validExtensions = new[] { ".csv", ".txt", ".dat" };
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        return validExtensions.Contains(extension);
    }
}
