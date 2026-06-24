using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedManage.Core.Configuration;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Background;

/// <summary>
/// Background service that periodically releases case locks that have exceeded
/// the inactivity timeout. Runs every N minutes (configurable via CaseLock:CleanupIntervalMinutes).
/// </summary>
public class ExpiredLockCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredLockCleanupService> _logger;
    private readonly TimeSpan _interval;

    public ExpiredLockCleanupService(
        IServiceProvider serviceProvider,
        IOptions<CaseLockSettings> settings,
        ILogger<ExpiredLockCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _interval = TimeSpan.FromMinutes(settings.Value.CleanupIntervalMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "ExpiredLockCleanupService started. Cleanup interval: {Interval} minutes",
            _interval.TotalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var lockService = scope.ServiceProvider.GetRequiredService<ICaseLockService>();
                var released = await lockService.ReleaseExpiredLocksAsync(stoppingToken);

                if (released > 0)
                {
                    _logger.LogInformation("ExpiredLockCleanup: released {Count} expired locks", released);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during expired lock cleanup");
            }
        }

        _logger.LogInformation("ExpiredLockCleanupService stopped.");
    }
}
