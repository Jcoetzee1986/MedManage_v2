using System.Threading.Channels;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Entities;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Services.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MedManage.Infrastructure.Services.Background;

/// <summary>
/// Background service that continuously reads TariffUpdateJob items from the channel
/// and processes them by propagating tariff percentages to the ServiceProvider_Tariff table.
/// Each job closes prior period records and inserts new records within a single transaction.
/// </summary>
public class TariffPercentageProcessor : BackgroundService
{
    private readonly ChannelReader<TariffUpdateJob> _channelReader;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TariffPercentageProcessor> _logger;

    public TariffPercentageProcessor(
        ChannelReader<TariffUpdateJob> channelReader,
        IServiceScopeFactory scopeFactory,
        ILogger<TariffPercentageProcessor> logger)
    {
        _channelReader = channelReader;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TariffPercentageProcessor started. Waiting for jobs...");

        try
        {
            await foreach (var job in _channelReader.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation(
                    "Processing tariff update job {JobId} for TariffPercentageId {TariffPercentageId}, Period {Period}",
                    job.JobId, job.TariffPercentageId, job.TariffPeriodName);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    await ProcessTariffUpdateJob(job, scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex,
                        "Unhandled error processing tariff update job {JobId}",
                        job.JobId);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when stoppingToken is cancelled
        }

        _logger.LogInformation("TariffPercentageProcessor stopped.");
    }

    private async Task ProcessTariffUpdateJob(
        TariffUpdateJob job,
        IServiceProvider serviceProvider,
        CancellationToken ct)
    {
        var context = serviceProvider.GetRequiredService<MedManageDbContext>();

        // Set extended command timeout for bulk operations (300 seconds)
        context.Database.SetCommandTimeout(300);

        var tariffPercentage = await context.Set<TariffPercentage>()
            .FirstOrDefaultAsync(tp => tp.TariffPercentageId == job.TariffPercentageId, ct);

        if (tariffPercentage == null)
        {
            _logger.LogWarning(
                "TariffPercentage record {Id} not found for job {JobId}. Skipping.",
                job.TariffPercentageId, job.JobId);

            UpdateJobStatus(job.JobId, "Failed", errorMessage: "TariffPercentage record not found.");
            return;
        }

        // Update status to Processing
        tariffPercentage.Status = "Processing";
        tariffPercentage.DateUpdated = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);

        UpdateJobStatus(job.JobId, "Processing", startedAt: DateTime.UtcNow);

        using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            int priorYear = job.TariffPeriodName - 1;
            var endDate = job.StartActiveDate.AddDays(-1);

            // Step 1: Close out existing active records for the prior period
            // Set EndActiveDate = StartActiveDate - 1 day WHERE TariffPeriodName = priorYear AND EndActiveDate IS NULL
            int closedCount = await context.Database.ExecuteSqlRawAsync(
                @"UPDATE [Tariff].[ServiceProvider_Tariff]
                  SET EndActiveDate = {0}
                  WHERE EndActiveDate IS NULL 
                    AND TariffPeriodName = {1}",
                endDate, priorYear);

            _logger.LogInformation(
                "Job {JobId}: Closed {ClosedCount} prior period records for year {PriorYear}",
                job.JobId, closedCount, priorYear);

            // Step 2: Insert new records from prior period template
            // Skip duplicates per requirement 11.2 using NOT EXISTS check
            int insertedCount = await context.Database.ExecuteSqlRawAsync(
                @"INSERT INTO [Tariff].[ServiceProvider_Tariff]
                    (ServiceProviderID, TariffNameID, MainClientID, 
                     StartActiveDate, EndActiveDate, TariffPeriodName, PercentageAdded)
                  SELECT ServiceProviderID, TariffNameID, MainClientID,
                    {0}, {1}, {2}, {3}
                  FROM [Tariff].[ServiceProvider_Tariff]
                  WHERE TariffPeriodName = {4} 
                    AND EndActiveDate = {5}
                  AND NOT EXISTS (
                    SELECT 1 FROM [Tariff].[ServiceProvider_Tariff] existing
                    WHERE existing.ServiceProviderID = [Tariff].[ServiceProvider_Tariff].ServiceProviderID
                      AND existing.TariffNameID = [Tariff].[ServiceProvider_Tariff].TariffNameID
                      AND existing.MainClientID = [Tariff].[ServiceProvider_Tariff].MainClientID
                      AND existing.TariffPeriodName = {2}
                  )",
                job.StartActiveDate, job.EndActiveDate,
                job.TariffPeriodName, job.PercentageAdded,
                priorYear, endDate);

            _logger.LogInformation(
                "Job {JobId}: Inserted {InsertedCount} new records for period {Period}",
                job.JobId, insertedCount, job.TariffPeriodName);

            await transaction.CommitAsync(ct);

            // Update TariffPercentage status to Completed
            tariffPercentage.Status = "Completed";
            tariffPercentage.RecordsAffected = insertedCount;
            tariffPercentage.DateUpdated = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);

            UpdateJobStatus(job.JobId, "Completed",
                recordsAffected: insertedCount,
                completedAt: DateTime.UtcNow);

            _logger.LogInformation(
                "Job {JobId} completed successfully. Records affected: {RecordsAffected}",
                job.JobId, insertedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job {JobId} failed during processing", job.JobId);

            await transaction.RollbackAsync(ct);

            // Update TariffPercentage status to Failed with truncated error message
            tariffPercentage.Status = "Failed";
            tariffPercentage.Notes = ex.Message[..Math.Min(ex.Message.Length, 500)];
            tariffPercentage.DateUpdated = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);

            UpdateJobStatus(job.JobId, "Failed",
                errorMessage: ex.Message[..Math.Min(ex.Message.Length, 500)],
                completedAt: DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Updates the in-memory job status dictionary shared with TariffPercentageService.
    /// </summary>
    private static void UpdateJobStatus(
        string jobId,
        string status,
        int? recordsAffected = null,
        string? errorMessage = null,
        DateTime? startedAt = null,
        DateTime? completedAt = null)
    {
        if (TariffPercentageService.JobStatuses.TryGetValue(jobId, out var jobStatus))
        {
            jobStatus.Status = status;

            if (recordsAffected.HasValue)
                jobStatus.RecordsAffected = recordsAffected;

            if (errorMessage != null)
                jobStatus.ErrorMessage = errorMessage;

            if (startedAt.HasValue)
                jobStatus.StartedAt = startedAt;

            if (completedAt.HasValue)
                jobStatus.CompletedAt = completedAt;
        }
    }
}
