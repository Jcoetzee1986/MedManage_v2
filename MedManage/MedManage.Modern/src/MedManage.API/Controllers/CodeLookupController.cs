using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.CodeLookup;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// API endpoints for searching CPT, ICD, and NAPPI codes with typeahead support.
/// Used by the Angular code lookup dialog for autocomplete.
/// </summary>
[ApiController]
[Route("api/codes")]
[Authorize]
public class CodeLookupController : ControllerBase
{
    private readonly ICodeLookupService _codeLookupService;
    private readonly ILogger<CodeLookupController> _logger;

    public CodeLookupController(ICodeLookupService codeLookupService, ILogger<CodeLookupController> logger)
    {
        _codeLookupService = codeLookupService;
        _logger = logger;
    }

    /// <summary>
    /// Search CPT codes by code or description (typeahead)
    /// </summary>
    [HttpGet("cpt")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CodeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCpt([FromQuery] string? q, [FromQuery] string? code, [FromQuery] string? description, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(q) && string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
        {
            return Ok(ApiResponse<IEnumerable<CodeLookupDto>>.SuccessResponse(Enumerable.Empty<CodeLookupDto>()));
        }

        var results = await _codeLookupService.SearchCptAsync(q, 20, cancellationToken, code, description);
        return Ok(ApiResponse<IEnumerable<CodeLookupDto>>.SuccessResponse(results));
    }

    /// <summary>
    /// Search ICD codes by code or description (typeahead)
    /// </summary>
    [HttpGet("icd")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CodeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchIcd([FromQuery] string? q, [FromQuery] string? code, [FromQuery] string? description, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(q) && string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
        {
            return Ok(ApiResponse<IEnumerable<CodeLookupDto>>.SuccessResponse(Enumerable.Empty<CodeLookupDto>()));
        }

        var results = await _codeLookupService.SearchIcdAsync(q, 20, cancellationToken, code, description);
        return Ok(ApiResponse<IEnumerable<CodeLookupDto>>.SuccessResponse(results));
    }

    /// <summary>
    /// Search NAPPI codes by code or description (typeahead), optionally filtered by effective date
    /// </summary>
    [HttpGet("nappi")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NappiCodeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchNappi([FromQuery] string? q, [FromQuery] string? code, [FromQuery] string? description, [FromQuery] DateTime? effectiveDate, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(q) && string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
        {
            return Ok(ApiResponse<IEnumerable<NappiCodeLookupDto>>.SuccessResponse(Enumerable.Empty<NappiCodeLookupDto>()));
        }

        var results = await _codeLookupService.SearchNappiAsync(q, effectiveDate, 20, cancellationToken, code, description);
        return Ok(ApiResponse<IEnumerable<NappiCodeLookupDto>>.SuccessResponse(results));
    }
}
