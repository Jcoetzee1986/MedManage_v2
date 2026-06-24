using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.MedicalAid;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/medical-aids")]
[Authorize]
public class MedicalAidController : ControllerBase
{
    private readonly IMedicalAidService _service;
    private readonly ILogger<MedicalAidController> _logger;

    public MedicalAidController(IMedicalAidService service, ILogger<MedicalAidController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // ─── Medical Aid CRUD ──────────────────────────────────────────

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidDto>>.SuccessResponse(items));
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var items = await _service.GetActiveAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<MedicalAidDto>.ErrorResponse($"Medical aid with ID {id} not found"));
        return Ok(ApiResponse<MedicalAidDto>.SuccessResponse(item));
    }

    [HttpGet("{id}/with-details")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithDetails(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdWithDetailsAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<MedicalAidDto>.ErrorResponse($"Medical aid with ID {id} not found"));
        return Ok(ApiResponse<MedicalAidDto>.SuccessResponse(item));
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] MedicalAidSearchFilters filters, CancellationToken cancellationToken)
    {
        var items = await _service.SearchAsync(filters, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidDto>>.SuccessResponse(items));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMedicalAidDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MedicalAidDto>.ErrorResponse("Invalid medical aid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.MedicalAidId }, ApiResponse<MedicalAidDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalAidDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.MedicalAidId)
            return BadRequest(ApiResponse<MedicalAidDto>.ErrorResponse("ID mismatch"));

        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MedicalAidDto>.ErrorResponse("Invalid medical aid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var updated = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<MedicalAidDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<MedicalAidDto>.ErrorResponse($"Medical aid with ID {id} not found"));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Medical aid with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    // ─── Product CRUD ──────────────────────────────────────────────

    [HttpGet("{id}/products")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(int id, CancellationToken cancellationToken)
    {
        var items = await _service.GetProductsByMedicalAidIdAsync(id, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidProductDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}/products/{productId}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidProductDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id, int productId, CancellationToken cancellationToken)
    {
        var item = await _service.GetProductByIdAsync(productId, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<MedicalAidProductDto>.ErrorResponse($"Product with ID {productId} not found"));
        return Ok(ApiResponse<MedicalAidProductDto>.SuccessResponse(item));
    }

    [HttpPost("{id}/products")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidProductDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(int id, [FromBody] CreateMedicalAidProductDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateProductAsync(id, dto, cancellationToken);
            return CreatedAtAction(nameof(GetProductById), new { id, productId = created.MedAidProductId }, ApiResponse<MedicalAidProductDto>.SuccessResponse(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product for medical aid {MedicalAidId}", id);
            return StatusCode(500, ApiResponse<MedicalAidProductDto>.ErrorResponse("An error occurred while creating the product"));
        }
    }

    [HttpPut("{id}/products/{productId}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidProductDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, int productId, [FromBody] UpdateMedicalAidProductDto dto, CancellationToken cancellationToken)
    {
        if (productId != dto.MedAidProductId)
            return BadRequest(ApiResponse<MedicalAidProductDto>.ErrorResponse("Product ID mismatch"));

        try
        {
            var updated = await _service.UpdateProductAsync(dto, cancellationToken);
            return Ok(ApiResponse<MedicalAidProductDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<MedicalAidProductDto>.ErrorResponse($"Product with ID {productId} not found"));
        }
    }

    [HttpDelete("{id}/products/{productId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id, int productId, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteProductAsync(productId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Product with ID {productId} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Product deleted successfully"));
    }

    // ─── Exclusion CRUD ────────────────────────────────────────────

    [HttpGet("{id}/exclusions")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidExclusionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExclusions(int id, CancellationToken cancellationToken)
    {
        var items = await _service.GetExclusionsByMedicalAidIdAsync(id, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidExclusionDto>>.SuccessResponse(items));
    }

    [HttpPost("{id}/exclusions")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidExclusionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidExclusionDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddExclusion(int id, [FromBody] CreateMedicalAidExclusionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.AddExclusionAsync(id, dto, cancellationToken);
            return CreatedAtAction(nameof(GetExclusions), new { id }, ApiResponse<MedicalAidExclusionDto>.SuccessResponse(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding exclusion to medical aid {MedicalAidId}", id);
            return StatusCode(500, ApiResponse<MedicalAidExclusionDto>.ErrorResponse("An error occurred while adding the exclusion"));
        }
    }

    [HttpDelete("{id}/exclusions/{baseTariffId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveExclusion(int id, string baseTariffId, CancellationToken cancellationToken)
    {
        var result = await _service.RemoveExclusionAsync(id, baseTariffId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Exclusion for tariff {baseTariffId} not found on medical aid {id}"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Exclusion removed successfully"));
    }

    // ─── Tariff Association ────────────────────────────────────────

    [HttpGet("{id}/tariffs")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalAidTariffDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTariffs(int id, CancellationToken cancellationToken)
    {
        var items = await _service.GetTariffsByMedicalAidIdAsync(id, cancellationToken);
        return Ok(ApiResponse<IEnumerable<MedicalAidTariffDto>>.SuccessResponse(items));
    }

    [HttpPost("{id}/tariffs")]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidTariffDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MedicalAidTariffDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTariff(int id, [FromBody] CreateMedicalAidTariffDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.AddTariffAsync(id, dto, cancellationToken);
            return CreatedAtAction(nameof(GetTariffs), new { id }, ApiResponse<MedicalAidTariffDto>.SuccessResponse(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding tariff to medical aid {MedicalAidId}", id);
            return StatusCode(500, ApiResponse<MedicalAidTariffDto>.ErrorResponse("An error occurred while adding the tariff"));
        }
    }

    [HttpDelete("{id}/tariffs/{tariffNameId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTariff(int id, int tariffNameId, CancellationToken cancellationToken)
    {
        var result = await _service.RemoveTariffAsync(id, tariffNameId, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Tariff {tariffNameId} not found on medical aid {id}"));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Tariff removed successfully"));
    }
}
