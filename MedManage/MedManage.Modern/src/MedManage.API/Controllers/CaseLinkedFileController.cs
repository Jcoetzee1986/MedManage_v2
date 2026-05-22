using Microsoft.AspNetCore.Mvc;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.CaseLinkedFile;
using MedManage.Core.DTOs.Common;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseLinkedFileController : ControllerBase
{
    private readonly ICaseLinkedFileService _caseLinkedFileService;
    private readonly ILogger<CaseLinkedFileController> _logger;

    public CaseLinkedFileController(ICaseLinkedFileService caseLinkedFileService, ILogger<CaseLinkedFileController> logger)
    {
        _caseLinkedFileService = caseLinkedFileService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseLinkedFileDto>>>> GetAll()
    {
        try
        {
            var files = await _caseLinkedFileService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CaseLinkedFileDto>>.SuccessResponse(files));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all case linked files");
            return StatusCode(500, ApiResponse<IEnumerable<CaseLinkedFileDto>>.ErrorResponse("An error occurred while retrieving case linked files"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CaseLinkedFileDto>>> GetById(int id)
    {
        try
        {
            var file = await _caseLinkedFileService.GetByIdAsync(id);
            if (file == null)
                return NotFound(ApiResponse<CaseLinkedFileDto>.ErrorResponse($"CaseLinkedFile with ID {id} not found"));

            return Ok(ApiResponse<CaseLinkedFileDto>.SuccessResponse(file));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving case linked file {Id}", id);
            return StatusCode(500, ApiResponse<CaseLinkedFileDto>.ErrorResponse("An error occurred while retrieving the case linked file"));
        }
    }

    [HttpGet("case/{caseId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseLinkedFileDto>>>> GetByCaseId(int caseId)
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

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CaseLinkedFileDto>>>> GetByMemberId(int memberId)
    {
        try
        {
            var files = await _caseLinkedFileService.GetByMemberIdAsync(memberId);
            return Ok(ApiResponse<IEnumerable<CaseLinkedFileDto>>.SuccessResponse(files));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving files for member {MemberId}", memberId);
            return StatusCode(500, ApiResponse<IEnumerable<CaseLinkedFileDto>>.ErrorResponse("An error occurred while retrieving member files"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CaseLinkedFileDto>>> Create([FromBody] CreateCaseLinkedFileDto dto)
    {
        try
        {
            var file = await _caseLinkedFileService.CreateAsync(dto);
            return Ok(ApiResponse<CaseLinkedFileDto>.SuccessResponse(file));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating case linked file");
            return StatusCode(500, ApiResponse<CaseLinkedFileDto>.ErrorResponse("An error occurred while creating the case linked file"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CaseLinkedFileDto>>> Update(int id, [FromBody] UpdateCaseLinkedFileDto dto)
    {
        try
        {
            var file = await _caseLinkedFileService.UpdateAsync(id, dto);
            return Ok(ApiResponse<CaseLinkedFileDto>.SuccessResponse(file));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CaseLinkedFileDto>.ErrorResponse($"CaseLinkedFile with ID {id} not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating case linked file {Id}", id);
            return StatusCode(500, ApiResponse<CaseLinkedFileDto>.ErrorResponse("An error occurred while updating the case linked file"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var result = await _caseLinkedFileService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"CaseLinkedFile with ID {id} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "CaseLinkedFile deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting case linked file {Id}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the case linked file"));
        }
    }
}
