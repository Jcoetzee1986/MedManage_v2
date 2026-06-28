using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/letter-templates")]
[Authorize]
public class LetterTemplateController : ControllerBase
{
    private readonly ILetterTemplateService _service;

    public LetterTemplateController(ILetterTemplateService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "System Administrator")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var templates = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<LetterTemplate>>.SuccessResponse(templates));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "System Administrator")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var template = await _service.GetByIdAsync(id, cancellationToken);
        if (template == null) return NotFound();
        return Ok(ApiResponse<LetterTemplate>.SuccessResponse(template));
    }

    [HttpPost]
    [Authorize(Roles = "System Administrator")]
    public async Task<IActionResult> Create([FromBody] LetterTemplate template, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(template, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.LetterTemplateId }, ApiResponse<LetterTemplate>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "System Administrator")]
    public async Task<IActionResult> Update(int id, [FromBody] LetterTemplate template, CancellationToken cancellationToken)
    {
        template.LetterTemplateId = id;
        var updated = await _service.UpdateAsync(template, cancellationToken);
        return Ok(ApiResponse<LetterTemplate>.SuccessResponse(updated));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "System Administrator")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result) return NotFound();
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    /// <summary>
    /// Generate a case letter PDF for a specific case using the appropriate template
    /// </summary>
    [HttpGet("render/{caseId}")]
    public async Task<IActionResult> RenderCaseLetter(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var pdf = await _service.RenderCaseLetterAsync(caseId, cancellationToken);
            return File(pdf, "application/pdf", $"CaseLetter_{caseId}_{DateTime.Now:yyyyMMdd}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Preview — render the template with case data and return HTML (for template editing preview)
    /// </summary>
    [HttpGet("preview/{caseId}")]
    [Authorize(Roles = "System Administrator")]
    public async Task<IActionResult> PreviewHtml(int caseId, CancellationToken cancellationToken)
    {
        try
        {
            var template = await _service.GetForCaseAsync(caseId, "CaseLetter", cancellationToken);
            if (template == null) return NotFound(ApiResponse<object>.ErrorResponse("No template found"));
            // For preview we'd render the HTML without converting to PDF
            return Ok(ApiResponse<object>.SuccessResponse(new { html = template.HtmlContent }));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
