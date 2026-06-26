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

    public async Task<DashboardStatsDto> GetStatsAsync(int? mainClientId = null, CancellationToken cancellationToken = default)
    {
        // Base queries — filter by mainClientId through Member → MedicalAid → MainClientId
        var casesQuery = _context.Cases
            .Where(c => c.DateDeleted == null);

        var membersQuery = _context.Members
            .Where(m => m.DateDeleted == null);

        var billingQuery = _context.CaseBillings
            .Where(b => b.DateDeleted == null && b.Paid != true);

        if (mainClientId.HasValue)
        {
            // Cases: filter through Member → MedicalAid → MainClientId
            casesQuery = casesQuery.Where(c => 
                c.Member!.MedicalAid!.MainClientId == mainClientId.Value);

            // Members: filter through MedicalAid → MainClientId
            membersQuery = membersQuery.Where(m => 
                m.MedicalAid!.MainClientId == mainClientId.Value);

            // Billing: filter through Cases that belong to this client
            var clientCaseIds = _context.Cases
                .Where(c => c.DateDeleted == null && 
                    c.Member!.MedicalAid!.MainClientId == mainClientId.Value)
                .Select(c => (int?)c.CaseId);

            billingQuery = billingQuery.Where(b => clientCaseIds.Contains(b.CaseId));
        }

        var totalCases = await casesQuery.CountAsync(cancellationToken);
        var totalMembers = await membersQuery.CountAsync(cancellationToken);
        var pendingBillingCount = await billingQuery.CountAsync(cancellationToken);

        // Active cases (StatusId == 1 is "Open")
        var activeCases = await casesQuery
            .CountAsync(c => c.StatusId == 1, cancellationToken);

        // Recent cases (last 30 days)
        var thirtyDaysAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var recentCases = await casesQuery
            .CountAsync(c => c.DateCreated >= thirtyDaysAgo, cancellationToken);

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
