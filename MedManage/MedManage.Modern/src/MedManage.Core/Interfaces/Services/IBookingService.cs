using MedManage.Core.DTOs.Booking;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;

namespace MedManage.Core.Interfaces.Services;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetByMemberNumberAsync(string memberNumber, CancellationToken cancellationToken = default);
    Task<PagedResult<BookingDto>> SearchAsync(BookingSearchFilters filters, CancellationToken cancellationToken = default);
    Task<BookingDto> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
    Task<BookingDto> UpdateAsync(UpdateBookingDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Converts a booking to a case by transitioning the linked case's status from Booking to Case.
    /// Sets HasBooking=true and ChangeToCaseDate on the case.
    /// </summary>
    Task<CaseDto> ConvertToCaseAsync(int bookingId, CancellationToken cancellationToken = default);
}
