using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetByCaseIdAsync(int caseId)
    {
        return await _dbSet
            .Where(b => b.CaseId == caseId && b.DateDeleted == null)
            .OrderBy(b => b.TravelDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByMemberNumberAsync(string memberNumber)
    {
        // Note: Member navigation not scaffolded - query by MemberId directly via Member table join
        return await _dbSet
            .Where(b => b.MemberId != null && b.DateDeleted == null)
            .Join(_context.Set<Member>(),
                  b => b.MemberId,
                  m => m.MemberId,
                  (b, m) => new { Booking = b, Member = m })
            .Where(x => x.Member.MemberNumber == memberNumber)
            .Select(x => x.Booking)
            .OrderByDescending(b => b.TravelDate)
            .ToListAsync();
    }

    public async Task<Booking?> GetByBookingIdAsync(int bookingId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.DateDeleted == null);
    }

    public async Task<IEnumerable<Booking>> SearchByFiltersAsync(
        DateTime? dateFrom,
        DateTime? dateTo,
        int? serviceProviderId,
        string? memberNumber)
    {
        var query = _dbSet
            .Where(b => b.DateDeleted == null);

        if (dateFrom.HasValue)
        {
            query = query.Where(b => b.TravelDate >= DateOnly.FromDateTime(dateFrom.Value));
        }

        if (dateTo.HasValue)
        {
            query = query.Where(b => b.TravelDate <= DateOnly.FromDateTime(dateTo.Value));
        }

        if (serviceProviderId.HasValue)
        {
            query = query.Where(b => b.ReferringPracticeId == serviceProviderId.Value);
        }

        if (!string.IsNullOrWhiteSpace(memberNumber))
        {
            // Join with Member table to filter by MemberNumber
            var memberQuery = _context.Set<Member>()
                .Where(m => m.MemberNumber != null && m.MemberNumber.Contains(memberNumber))
                .Select(m => m.MemberId);
            query = query.Where(b => b.MemberId.HasValue && memberQuery.Contains(b.MemberId.Value));
        }

        return await query
            .OrderByDescending(b => b.TravelDate)
            .ToListAsync();
    }
}
