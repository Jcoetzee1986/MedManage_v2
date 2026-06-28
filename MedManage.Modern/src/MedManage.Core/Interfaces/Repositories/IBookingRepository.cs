using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByCaseIdAsync(int caseId);
    Task<IEnumerable<Booking>> GetByMemberNumberAsync(string memberNumber);
    Task<Booking?> GetByBookingIdAsync(int bookingId);
    Task<IEnumerable<Booking>> SearchByFiltersAsync(
        DateTime? dateFrom,
        DateTime? dateTo,
        int? serviceProviderId,
        string? memberNumber);
}
