using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Dashboard;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class DashboardService : IDashboardService
{
    private readonly MedManageDbContext _context;

    public DashboardService(MedManageDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        var totalCases = await _context.Cases
            .CountAsync(c => c.DateDeleted == null, cancellationToken);

        var totalMembers = await _context.Members
            .CountAsync(m => m.DateDeleted == null, cancellationToken);

        var pendingBillingCount = await _context.CaseBillings
            .CountAsync(b => b.DateDeleted == null && b.Paid != true, cancellationToken);

        // Active cases: cases with a status that indicates active work (StatusId == 1 is typically "Open/Active")
        // We consider cases created in the last 90 days that are not deleted as "active"
        var activeCases = await _context.Cases
            .CountAsync(c => c.DateDeleted == null && c.StatusId == 1, cancellationToken);

        // Recent cases: created in the last 30 days
        var thirtyDaysAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var recentCases = await _context.Cases
            .CountAsync(c => c.DateDeleted == null && c.DateCreated >= thirtyDaysAgo, cancellationToken);

        return new DashboardStatsDto
        {
            TotalCases = totalCases,
            TotalMembers = totalMembers,
            PendingBillingCount = pendingBillingCount,
            ActiveCases = activeCases,
            RecentCases = recentCases
        };
    }
}
