using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Admin;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// API controller for system configuration data management
/// </summary>
[ApiController]
[Route("api/system-data")]
[Authorize(Roles = "Admin")]
public class SystemDataController : ControllerBase
{
    private readonly ISystemDataService _service;
    private readonly ILogger<SystemDataController> _logger;

    public SystemDataController(ISystemDataService service, ILogger<SystemDataController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get the current system configuration
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _service.GetAsync(cancellationToken);
        if (result == null)
            return Ok(ApiResponse<SystemDataDto?>.SuccessResponse(null, "No system configuration found"));
        return Ok(ApiResponse<SystemDataDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Get system configuration by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound(ApiResponse<SystemDataDto>.ErrorResponse($"System data with ID {id} not found"));
        return Ok(ApiResponse<SystemDataDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create new system configuration
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSystemDataRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<SystemDataDto>.ErrorResponse("Invalid request"));

        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.SystemDataId },
            ApiResponse<SystemDataDto>.SuccessResponse(result, "System configuration created successfully"));
    }

    /// <summary>
    /// Update existing system configuration
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSystemDataRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<SystemDataDto>.ErrorResponse("Invalid request"));

        request.SystemDataId = id;

        try
        {
            var result = await _service.UpdateAsync(request, cancellationToken);
            return Ok(ApiResponse<SystemDataDto>.SuccessResponse(result, "System configuration updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<SystemDataDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Delete system configuration
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"System data with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "System configuration deleted successfully"));
    }

    /// <summary>
    /// Upload system logo
    /// </summary>
    [HttpPost("{id}/logo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadLogo(int id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse<bool>.ErrorResponse("No file provided"));

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var logoData = memoryStream.ToArray();

        var result = await _service.UpdateLogoAsync(id, logoData, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"System data with ID {id} not found"));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Logo uploaded successfully"));
    }

    /// <summary>
    /// Get system logo
    /// </summary>
    [HttpGet("{id}/logo")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLogo(int id, CancellationToken cancellationToken)
    {
        var logoData = await _service.GetLogoAsync(id, cancellationToken);
        if (logoData == null || logoData.Length == 0)
            return NotFound();

        return File(logoData, "image/png");
    }
}
