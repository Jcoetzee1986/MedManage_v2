using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Episode;
using MedManage.Core.DTOs.EpisodeCase;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/episodes")]
[Authorize]
public class EpisodeController : ControllerBase
{
    private readonly IEpisodeService _service;
    private readonly IEpisodeCaseService _episodeCaseService;

    public EpisodeController(IEpisodeService service, IEpisodeCaseService episodeCaseService)
    {
        _service = service;
        _episodeCaseService = episodeCaseService;
    }

    // ─── Episode CRUD ─────────────────────────────────────────

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EpisodeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<EpisodeDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<EpisodeDto>.ErrorResponse($"Episode with ID {id} not found"));
        return Ok(ApiResponse<EpisodeDto>.SuccessResponse(item));
    }

    [HttpGet("{id}/with-cases")]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithCases(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdWithCasesAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<EpisodeDto>.ErrorResponse($"Episode with ID {id} not found"));
        return Ok(ApiResponse<EpisodeDto>.SuccessResponse(item));
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EpisodeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] EpisodeSearchFilters filters, CancellationToken cancellationToken)
    {
        var items = await _service.SearchAsync(filters, cancellationToken);
        return Ok(ApiResponse<IEnumerable<EpisodeDto>>.SuccessResponse(items));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEpisodeDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<EpisodeDto>.ErrorResponse("Invalid episode data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.EpisodeId }, ApiResponse<EpisodeDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<EpisodeDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEpisodeDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.EpisodeId)
            return BadRequest(ApiResponse<EpisodeDto>.ErrorResponse("ID mismatch"));

        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<EpisodeDto>.ErrorResponse("Invalid episode data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var updated = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<EpisodeDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<EpisodeDto>.ErrorResponse($"Episode with ID {id} not found"));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Episode with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    // ─── Episode-Case Linking (sub-entity) ────────────────────

    [HttpGet("{id}/cases")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EpisodeCaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCases(int id)
    {
        var cases = await _episodeCaseService.GetByEpisodeIdAsync(id);
        return Ok(ApiResponse<IEnumerable<EpisodeCaseDto>>.SuccessResponse(cases));
    }

    [HttpPost("{id}/cases")]
    [ProducesResponseType(typeof(ApiResponse<EpisodeCaseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<EpisodeCaseDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LinkCase(int id, [FromBody] LinkCaseToEpisodeDto dto)
    {
        var createDto = new CreateEpisodeCaseDto
        {
            EpisodeId = id,
            CaseId = dto.CaseId,
            DateCreated = dto.DateCreated ?? DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var created = await _episodeCaseService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetCases), new { id }, ApiResponse<EpisodeCaseDto>.SuccessResponse(created));
    }

    [HttpDelete("{id}/cases/{caseId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnlinkCase(int id, int caseId)
    {
        var result = await _episodeCaseService.DeleteAsync(id, caseId);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Case {caseId} is not linked to Episode {id}"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}

/// <summary>
/// DTO for linking a case to an episode via POST /api/episodes/{id}/cases
/// </summary>
public class LinkCaseToEpisodeDto
{
    public int CaseId { get; set; }
    public DateOnly? DateCreated { get; set; }
}
