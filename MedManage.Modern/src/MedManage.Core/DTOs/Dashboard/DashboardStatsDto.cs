namespace MedManage.Core.DTOs.Dashboard;

/// <summary>
/// Dashboard statistics summary
/// </summary>
public class DashboardStatsDto
{
    public int TotalCases { get; set; }
    public int TotalMembers { get; set; }
    public int PendingBillingCount { get; set; }
    public int ActiveCases { get; set; }
    public int RecentCases { get; set; }
}
