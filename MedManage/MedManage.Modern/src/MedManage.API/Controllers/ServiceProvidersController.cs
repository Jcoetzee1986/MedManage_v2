using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.ServiceProvider;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/service-providers")]
[Authorize]
public class ServiceProvidersController : ControllerBase
{
    private readonly IServiceProviderService _serviceProviderService;
    private readonly IValidator<CreateServiceProviderRequest> _createValidator;
    private readonly IValidator<UpdateServiceProviderRequest> _updateValidator;
    private readonly ILogger<ServiceProvidersController> _logger;

    public ServiceProvidersController(
        IServiceProviderService serviceProviderService,
        IValidator<CreateServiceProviderRequest> createValidator,
        IValidator<UpdateServiceProviderRequest> updateValidator,
        ILogger<ServiceProvidersController> logger)
    {
        _serviceProviderService = serviceProviderService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    #region Core CRUD

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

    #endregion

    #region Autocomplete

    /// <summary>
    /// Autocomplete/typeahead endpoint for searching service providers by name, surname, practice name, or practice number
    /// </summary>
    [HttpGet("autocomplete")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServiceProviderAutocompleteDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Autocomplete([FromQuery] string q, CancellationToken cancellationToken)
    {
        var results = await _serviceProviderService.AutocompleteAsync(q, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ServiceProviderAutocompleteDto>>.SuccessResponse(results));
    }

    #endregion

    #region Tariff Assignment CRUD

    /// <summary>
    /// Get all tariff assignments for a service provider
    /// </summary>
    [HttpGet("{providerId}/tariffs")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServiceProviderTariffDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTariffs(int providerId, CancellationToken cancellationToken)
    {
        var tariffs = await _serviceProviderService.GetTariffsAsync(providerId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ServiceProviderTariffDto>>.SuccessResponse(tariffs));
    }

    /// <summary>
    /// Get a specific tariff assignment
    /// </summary>
    [HttpGet("{providerId}/tariffs/{tariffId}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTariffById(int providerId, long tariffId, CancellationToken cancellationToken)
    {
        var tariff = await _serviceProviderService.GetTariffByIdAsync(providerId, tariffId, cancellationToken);
        if (tariff == null)
        {
            return NotFound(ApiResponse<ServiceProviderTariffDto>.ErrorResponse($"Tariff assignment {tariffId} not found for provider {providerId}"));
        }
        return Ok(ApiResponse<ServiceProviderTariffDto>.SuccessResponse(tariff));
    }

    /// <summary>
    /// Create a tariff assignment for a service provider
    /// </summary>
    [HttpPost("{providerId}/tariffs")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTariff(int providerId, [FromBody] CreateServiceProviderTariffRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tariff = await _serviceProviderService.CreateTariffAsync(providerId, request, cancellationToken);
            return CreatedAtAction(nameof(GetTariffById), new { providerId, tariffId = tariff.ServiceProviderTariffId },
                ApiResponse<ServiceProviderTariffDto>.SuccessResponse(tariff, "Tariff assignment created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tariff assignment for provider {ProviderId}", providerId);
            return StatusCode(500, ApiResponse<ServiceProviderTariffDto>.ErrorResponse("An error occurred while creating the tariff assignment"));
        }
    }

    /// <summary>
    /// Update a tariff assignment for a service provider
    /// </summary>
    [HttpPut("{providerId}/tariffs/{tariffId}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTariff(int providerId, long tariffId, [FromBody] UpdateServiceProviderTariffRequest request, CancellationToken cancellationToken)
    {
        if (tariffId != request.ServiceProviderTariffId)
        {
            return BadRequest(ApiResponse<ServiceProviderTariffDto>.ErrorResponse("ID mismatch between route and body"));
        }

        try
        {
            var tariff = await _serviceProviderService.UpdateTariffAsync(providerId, request, cancellationToken);
            return Ok(ApiResponse<ServiceProviderTariffDto>.SuccessResponse(tariff, "Tariff assignment updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<ServiceProviderTariffDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tariff assignment {TariffId} for provider {ProviderId}", tariffId, providerId);
            return StatusCode(500, ApiResponse<ServiceProviderTariffDto>.ErrorResponse("An error occurred while updating the tariff assignment"));
        }
    }

    /// <summary>
    /// Delete a tariff assignment for a service provider
    /// </summary>
    [HttpDelete("{providerId}/tariffs/{tariffId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTariff(int providerId, long tariffId, CancellationToken cancellationToken)
    {
        var deleted = await _serviceProviderService.DeleteTariffAsync(providerId, tariffId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    #endregion

    #region Custom Tariff CRUD

    /// <summary>
    /// Get all custom tariffs for a service provider
    /// </summary>
    [HttpGet("{providerId}/custom-tariffs")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServiceProviderTariffCustomDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomTariffs(int providerId, CancellationToken cancellationToken)
    {
        var customTariffs = await _serviceProviderService.GetCustomTariffsAsync(providerId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ServiceProviderTariffCustomDto>>.SuccessResponse(customTariffs));
    }

    /// <summary>
    /// Get a specific custom tariff
    /// </summary>
    [HttpGet("{providerId}/custom-tariffs/{customTariffId}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffCustomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffCustomDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomTariffById(int providerId, long customTariffId, CancellationToken cancellationToken)
    {
        var customTariff = await _serviceProviderService.GetCustomTariffByIdAsync(providerId, customTariffId, cancellationToken);
        if (customTariff == null)
        {
            return NotFound(ApiResponse<ServiceProviderTariffCustomDto>.ErrorResponse($"Custom tariff {customTariffId} not found for provider {providerId}"));
        }
        return Ok(ApiResponse<ServiceProviderTariffCustomDto>.SuccessResponse(customTariff));
    }

    /// <summary>
    /// Create a custom tariff for a service provider
    /// </summary>
    [HttpPost("{providerId}/custom-tariffs")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffCustomDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomTariff(int providerId, [FromBody] CreateServiceProviderTariffCustomRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customTariff = await _serviceProviderService.CreateCustomTariffAsync(providerId, request, cancellationToken);
            return CreatedAtAction(nameof(GetCustomTariffById), new { providerId, customTariffId = customTariff.ServiceProviderTariffCustomId },
                ApiResponse<ServiceProviderTariffCustomDto>.SuccessResponse(customTariff, "Custom tariff created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating custom tariff for provider {ProviderId}", providerId);
            return StatusCode(500, ApiResponse<ServiceProviderTariffCustomDto>.ErrorResponse("An error occurred while creating the custom tariff"));
        }
    }

    /// <summary>
    /// Update a custom tariff for a service provider
    /// </summary>
    [HttpPut("{providerId}/custom-tariffs/{customTariffId}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffCustomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderTariffCustomDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomTariff(int providerId, long customTariffId, [FromBody] UpdateServiceProviderTariffCustomRequest request, CancellationToken cancellationToken)
    {
        if (customTariffId != request.ServiceProviderTariffCustomId)
        {
            return BadRequest(ApiResponse<ServiceProviderTariffCustomDto>.ErrorResponse("ID mismatch between route and body"));
        }

        try
        {
            var customTariff = await _serviceProviderService.UpdateCustomTariffAsync(providerId, request, cancellationToken);
            return Ok(ApiResponse<ServiceProviderTariffCustomDto>.SuccessResponse(customTariff, "Custom tariff updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<ServiceProviderTariffCustomDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating custom tariff {CustomTariffId} for provider {ProviderId}", customTariffId, providerId);
            return StatusCode(500, ApiResponse<ServiceProviderTariffCustomDto>.ErrorResponse("An error occurred while updating the custom tariff"));
        }
    }

    /// <summary>
    /// Delete a custom tariff for a service provider
    /// </summary>
    [HttpDelete("{providerId}/custom-tariffs/{customTariffId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomTariff(int providerId, long customTariffId, CancellationToken cancellationToken)
    {
        var deleted = await _serviceProviderService.DeleteCustomTariffAsync(providerId, customTariffId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    #endregion

    #region Discount CRUD (per MainClient)

    /// <summary>
    /// Get all discounts for a service provider
    /// </summary>
    [HttpGet("{providerId}/discounts")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServiceProviderDiscountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDiscounts(int providerId, CancellationToken cancellationToken)
    {
        var discounts = await _serviceProviderService.GetDiscountsAsync(providerId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<ServiceProviderDiscountDto>>.SuccessResponse(discounts));
    }

    /// <summary>
    /// Get a specific discount by MainClient
    /// </summary>
    [HttpGet("{providerId}/discounts/{mainClientId}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDiscountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDiscountDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiscountByClient(int providerId, int mainClientId, CancellationToken cancellationToken)
    {
        var discount = await _serviceProviderService.GetDiscountByClientAsync(providerId, mainClientId, cancellationToken);
        if (discount == null)
        {
            return NotFound(ApiResponse<ServiceProviderDiscountDto>.ErrorResponse($"Discount not found for provider {providerId} and client {mainClientId}"));
        }
        return Ok(ApiResponse<ServiceProviderDiscountDto>.SuccessResponse(discount));
    }

    /// <summary>
    /// Create a discount for a service provider per MainClient
    /// </summary>
    [HttpPost("{providerId}/discounts")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDiscountDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDiscount(int providerId, [FromBody] CreateServiceProviderDiscountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var discount = await _serviceProviderService.CreateDiscountAsync(providerId, request, cancellationToken);
            return CreatedAtAction(nameof(GetDiscountByClient), new { providerId, mainClientId = discount.MainClientId },
                ApiResponse<ServiceProviderDiscountDto>.SuccessResponse(discount, "Discount created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating discount for provider {ProviderId}", providerId);
            return StatusCode(500, ApiResponse<ServiceProviderDiscountDto>.ErrorResponse("An error occurred while creating the discount"));
        }
    }

    /// <summary>
    /// Update a discount for a service provider per MainClient
    /// </summary>
    [HttpPut("{providerId}/discounts/{mainClientId}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDiscountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ServiceProviderDiscountDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDiscount(int providerId, int mainClientId, [FromBody] UpdateServiceProviderDiscountRequest request, CancellationToken cancellationToken)
    {
        if (mainClientId != request.MainClientId)
        {
            return BadRequest(ApiResponse<ServiceProviderDiscountDto>.ErrorResponse("MainClientId mismatch between route and body"));
        }

        try
        {
            var discount = await _serviceProviderService.UpdateDiscountAsync(providerId, request, cancellationToken);
            return Ok(ApiResponse<ServiceProviderDiscountDto>.SuccessResponse(discount, "Discount updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<ServiceProviderDiscountDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating discount for provider {ProviderId} and client {MainClientId}", providerId, mainClientId);
            return StatusCode(500, ApiResponse<ServiceProviderDiscountDto>.ErrorResponse("An error occurred while updating the discount"));
        }
    }

    /// <summary>
    /// Delete a discount for a service provider per MainClient
    /// </summary>
    [HttpDelete("{providerId}/discounts/{mainClientId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDiscount(int providerId, int mainClientId, CancellationToken cancellationToken)
    {
        var deleted = await _serviceProviderService.DeleteDiscountAsync(providerId, mainClientId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    #endregion
}
