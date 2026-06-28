using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// API controller for managing tariff percentages (System Administrator only).
/// </summary>
[ApiController]
[Route("api/admin/tariff-percentages")]
[Authorize(Roles = "System Administrator")]
public class TariffPercentageController : ControllerBase
{
    private readonly ITariffPercentageService _service;
    private readonly ILogger<TariffPercentageController> _logger;

    public TariffPercentageController(ITariffPercentageService service, ILogger<TariffPercentageController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all tariff percentages.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TariffPercentageDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var percentages = await _service.GetAllAsync(cancellationToken);
            return Ok(ApiResponse<IEnumerable<TariffPercentageDto>>.SuccessResponse(percentages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tariff percentages");
            return StatusCode(500, ApiResponse<IEnumerable<TariffPercentageDto>>.ErrorResponse("An error occurred while retrieving tariff percentages"));
        }
    }

    /// <summary>
    /// Get a tariff percentage by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var percentage = await _service.GetByIdAsync(id, cancellationToken);
            if (percentage == null)
                return NotFound(ApiResponse<TariffPercentageDto>.ErrorResponse($"Tariff percentage with ID {id} not found"));

            return Ok(ApiResponse<TariffPercentageDto>.SuccessResponse(percentage));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tariff percentage {Id}", id);
            return StatusCode(500, ApiResponse<TariffPercentageDto>.ErrorResponse("An error occurred while retrieving the tariff percentage"));
        }
    }

    /// <summary>
    /// Create a new tariff percentage.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateTariffPercentageDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<TariffPercentageDto>.ErrorResponse("Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var result = await _service.CreateAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, ApiResponse<TariffPercentageDto>.SuccessResponse(result, "Tariff percentage created successfully"));
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            return Conflict(ApiResponse<TariffPercentageDto>.ErrorResponse(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<TariffPercentageDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tariff percentage");
            return StatusCode(500, ApiResponse<TariffPercentageDto>.ErrorResponse("An error occurred while creating the tariff percentage"));
        }
    }

    /// <summary>
    /// Update an existing tariff percentage.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TariffPercentageDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTariffPercentageDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<TariffPercentageDto>.ErrorResponse("Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var result = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(ApiResponse<TariffPercentageDto>.SuccessResponse(result, "Tariff percentage updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<TariffPercentageDto>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            return Conflict(ApiResponse<TariffPercentageDto>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TariffPercentageDto>.ErrorResponse(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<TariffPercentageDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tariff percentage {Id}", id);
            return StatusCode(500, ApiResponse<TariffPercentageDto>.ErrorResponse("An error occurred while updating the tariff percentage"));
        }
    }

    /// <summary>
    /// Soft-delete a tariff percentage.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.DeleteAsync(id, cancellationToken);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"Tariff percentage with ID {id} not found"));

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tariff percentage {Id}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the tariff percentage"));
        }
    }

    /// <summary>
    /// Trigger propagation of a tariff percentage to ServiceProvider_Tariff records.
    /// Returns 202 Accepted with the job status for polling.
    /// </summary>
    [HttpPost("{id}/apply")]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Apply(int id, CancellationToken cancellationToken)
    {
        try
        {
            var jobStatus = await _service.ApplyPercentageAsync(id, cancellationToken);
            return StatusCode(StatusCodes.Status202Accepted, ApiResponse<TariffUpdateJobStatus>.SuccessResponse(jobStatus, "Propagation job queued successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<TariffUpdateJobStatus>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already in progress"))
        {
            return Conflict(ApiResponse<TariffUpdateJobStatus>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TariffUpdateJobStatus>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying tariff percentage {Id}", id);
            return StatusCode(500, ApiResponse<TariffUpdateJobStatus>.ErrorResponse("An error occurred while triggering the propagation job"));
        }
    }

    /// <summary>
    /// Get the status of a propagation job by job ID.
    /// </summary>
    [HttpGet("jobs/{jobId}")]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TariffUpdateJobStatus>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobStatus(string jobId, CancellationToken cancellationToken)
    {
        // Validate jobId is a valid GUID format
        if (!Guid.TryParse(jobId, out _))
            return BadRequest(ApiResponse<TariffUpdateJobStatus>.ErrorResponse("The jobId format is invalid. It must be a valid GUID."));

        try
        {
            var status = await _service.GetJobStatusAsync(jobId, cancellationToken);
            return Ok(ApiResponse<TariffUpdateJobStatus>.SuccessResponse(status));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<TariffUpdateJobStatus>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job status for {JobId}", jobId);
            return StatusCode(500, ApiResponse<TariffUpdateJobStatus>.ErrorResponse("An error occurred while retrieving the job status"));
        }
    }

    /// <summary>
    /// Get active tariff percentages for case letter generation (most recent 2 years).
    /// </summary>
    [HttpGet("active-for-letter")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TariffPercentageDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveForLetter(CancellationToken cancellationToken)
    {
        try
        {
            var percentages = await _service.GetActivePercentagesForLetterAsync(cancellationToken);
            return Ok(ApiResponse<IEnumerable<TariffPercentageDto>>.SuccessResponse(percentages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active tariff percentages for letter");
            return StatusCode(500, ApiResponse<IEnumerable<TariffPercentageDto>>.ErrorResponse("An error occurred while retrieving active tariff percentages"));
        }
    }
}
