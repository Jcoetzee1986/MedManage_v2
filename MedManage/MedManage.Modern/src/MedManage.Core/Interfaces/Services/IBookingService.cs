using MedManage.Core.DTOs.Booking;

namespace MedManage.Core.Interfaces.Services;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetByMemberNumberAsync(string memberNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> SearchAsync(BookingSearchFilters filters, CancellationToken cancellationToken = default);
    Task<BookingDto> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
    Task<BookingDto> UpdateAsync(UpdateBookingDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
