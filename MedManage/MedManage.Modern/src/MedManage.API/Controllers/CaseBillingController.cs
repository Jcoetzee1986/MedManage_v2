using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseBillingController : ControllerBase
{
    private readonly ICaseBillingService _service;

    public CaseBillingController(ICaseBillingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var billings = await _service.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(billings));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var billing = await _service.GetByIdAsync(id);
        if (billing == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Case billing not found"));

        return Ok(ApiResponse<object>.SuccessResponse(billing));
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(int caseId)
    {
        var billings = await _service.GetByCaseIdAsync(caseId);
        return Ok(ApiResponse<object>.SuccessResponse(billings));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCaseBillingDto dto)
    {
        var billing = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = billing.CaseBillingId },
            ApiResponse<object>.SuccessResponse(billing));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseBillingDto dto)
    {
        try
        {
            var billing = await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<object>.SuccessResponse(billing));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Case billing not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null));
    }
}
