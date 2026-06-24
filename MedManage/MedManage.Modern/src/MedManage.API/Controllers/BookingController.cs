using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Booking;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var items = await _service.GetAllAsync(includeDeleted, cancellationToken);
        return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(items));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(ApiResponse<BookingDto>.ErrorResponse($"Booking with ID {id} not found"));
        return Ok(ApiResponse<BookingDto>.SuccessResponse(item));
    }

    [HttpGet("case/{caseId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCaseId(int caseId, CancellationToken cancellationToken)
    {
        var items = await _service.GetByCaseIdAsync(caseId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(items));
    }

    [HttpGet("member/{memberNumber}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMemberNumber(string memberNumber, CancellationToken cancellationToken)
    {
        var items = await _service.GetByMemberNumberAsync(memberNumber, cancellationToken);
        return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(items));
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] BookingSearchFilters filters, CancellationToken cancellationToken)
    {
        var items = await _service.SearchAsync(filters, cancellationToken);
        return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(items));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<BookingDto>.ErrorResponse("Invalid booking data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.BookingId }, ApiResponse<BookingDto>.SuccessResponse(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.BookingId)
            return BadRequest(ApiResponse<BookingDto>.ErrorResponse("ID mismatch"));

        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<BookingDto>.ErrorResponse("Invalid booking data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

        try
        {
            var updated = await _service.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse<BookingDto>.SuccessResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<BookingDto>.ErrorResponse($"Booking with ID {id} not found"));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse($"Booking with ID {id} not found"));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    /// <summary>
    /// Converts a booking to a case by transitioning the linked case status from Booking to Case.
    /// Sets HasBooking=true and ChangeToCaseDate on the case.
    /// </summary>
    [HttpPost("{id}/convert-to-case")]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CaseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConvertToCase(int id, CancellationToken cancellationToken)
    {
        try
        {
            var updatedCase = await _service.ConvertToCaseAsync(id, cancellationToken);
            return Ok(ApiResponse<CaseDto>.SuccessResponse(updatedCase, "Booking successfully converted to case"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<CaseDto>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CaseDto>.ErrorResponse(ex.Message));
        }
    }
}
