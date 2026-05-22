using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ServiceProvider;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceProvidersController : ControllerBase
{
    private readonly IServiceProviderService _serviceProviderService;
    private readonly IValidator<CreateServiceProviderRequest> _createValidator;
    private readonly IValidator<UpdateServiceProviderRequest> _updateValidator;

    public ServiceProvidersController(
        IServiceProviderService serviceProviderService,
        IValidator<CreateServiceProviderRequest> createValidator,
        IValidator<UpdateServiceProviderRequest> updateValidator)
    {
        _serviceProviderService = serviceProviderService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a service provider by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var serviceProvider = await _serviceProviderService.GetByIdAsync(id, cancellationToken);
        
        if (serviceProvider == null)
        {
            return NotFound(ApiResponse<ServiceProviderDto>.ErrorResponse($"Service provider with ID {id} not found"));
        }

        return Ok(ApiResponse<ServiceProviderDto>.SuccessResponse(serviceProvider));
    }

    /// <summary>
    /// Search service providers with optional filters and pagination
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceProviderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] ServiceProviderSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _serviceProviderService.SearchAsync(request, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceProviderDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new service provider
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateServiceProviderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<ServiceProviderDto>.ErrorResponse("Validation failed", errors));
        }

        var serviceProvider = await _serviceProviderService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById), 
            new { id = serviceProvider.ServiceProviderId }, 
            ApiResponse<ServiceProviderDto>.SuccessResponse(serviceProvider, "Service provider created successfully"));
    }

    /// <summary>
    /// Update an existing service provider
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceProviderRequest request, CancellationToken cancellationToken)
    {
        if (id != request.ServiceProviderId)
        {
            return BadRequest(ApiResponse<ServiceProviderDto>.ErrorResponse("ID mismatch between route and body"));
        }

        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<ServiceProviderDto>.ErrorResponse("Validation failed", errors));
        }

        try
        {
            var serviceProvider = await _serviceProviderService.UpdateAsync(request, cancellationToken);
            return Ok(ApiResponse<ServiceProviderDto>.SuccessResponse(serviceProvider, "Service provider updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<ServiceProviderDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Soft delete a service provider
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceProviderService.DeleteAsync(id, cancellationToken);
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Check if a service provider exists
    /// </summary>
    [HttpHead("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken)
    {
        var exists = await _serviceProviderService.ExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }
}
