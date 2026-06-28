using MedManage.Core.DTOs.Dashboard;

namespace MedManage.Core.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetStatsAsync(int? mainClientId = null, CancellationToken cancellationToken = default);
}
