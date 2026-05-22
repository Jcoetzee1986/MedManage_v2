using System.Threading.Tasks;
using MedManage.Core.DTOs.EpisodeCase;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EpisodeCaseController : ControllerBase
{
    private readonly IEpisodeCaseService _service;

    public EpisodeCaseController(IEpisodeCaseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var episodeCases = await _service.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(episodeCases));
    }

    [HttpGet("episode/{episodeId}/case/{caseId}")]
    public async Task<IActionResult> GetById(int episodeId, int caseId)
    {
        var episodeCase = await _service.GetByIdAsync(episodeId, caseId);
        if (episodeCase == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Episode case not found"));

        return Ok(ApiResponse<object>.SuccessResponse(episodeCase));
    }

    [HttpGet("episode/{episodeId}")]
    public async Task<IActionResult> GetByEpisodeId(int episodeId)
    {
        var episodeCases = await _service.GetByEpisodeIdAsync(episodeId);
        return Ok(ApiResponse<object>.SuccessResponse(episodeCases));
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var episodeCases = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(episodeCases));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEpisodeCaseDto dto)
    {
        var episodeCase = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), 
            new { episodeId = episodeCase.EpisodeId, caseId = episodeCase.CaseId },
            ApiResponse<object>.SuccessResponse(episodeCase));
    }

    [HttpPut("episode/{episodeId}/case/{caseId}")]
    public async Task<IActionResult> Update(int episodeId, int caseId, [FromBody] UpdateEpisodeCaseDto dto)
    {
        try
        {
            var episodeCase = await _service.UpdateAsync(episodeId, caseId, dto);
            return Ok(ApiResponse<object>.SuccessResponse(episodeCase));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("episode/{episodeId}/case/{caseId}")]
    public async Task<IActionResult> Delete(int episodeId, int caseId)
    {
        var result = await _service.DeleteAsync(episodeId, caseId);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Episode case not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!));
    }
}
