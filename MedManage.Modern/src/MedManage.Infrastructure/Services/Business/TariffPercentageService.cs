using System.Collections.Concurrent;
using System.Threading.Channels;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service implementation for Tariff Percentage management operations.
/// Handles CRUD with validation, overlap detection, and status-based access control.
/// </summary>
public class TariffPercentageService : ITariffPercentageService
{
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ChannelWriter<TariffUpdateJob> _channelWriter;

    /// <summary>
    /// In-memory store for tracking job statuses. Shared across all instances via static field.
    /// </summary>
    private static readonly ConcurrentDictionary<string, TariffUpdateJobStatus> _jobStatuses = new();

    /// <summary>
    /// Provides access to the job status dictionary for the background processor to update statuses.
    /// </summary>
    public static ConcurrentDictionary<string, TariffUpdateJobStatus> JobStatuses => _jobStatuses;

    public TariffPercentageService(
        MedManageDbContext context,
        ICurrentUserService currentUserService,
        ChannelWriter<TariffUpdateJob> channelWriter)
    {
        _context = context;
        _currentUserService = currentUserService;
        _channelWriter = channelWriter;
    }

    public async Task<IEnumerable<TariffPercentageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _context.Set<TariffPercentage>()
            .Where(r => r.DateDeleted == null)
            .OrderByDescending(r => r.TariffPeriodName)
            .ThenByDescending(r => r.StartActiveDate)
            .ToListAsync(cancellationToken);

        return records.Select(MapToDto);
    }

    public async Task<TariffPercentageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var record = await _context.Set<TariffPercentage>()
            .FirstOrDefaultAsync(r => r.TariffPercentageId == id && r.DateDeleted == null, cancellationToken);

        if (record == null)
        {
            return null;
        }

        return MapToDto(record);
    }

    public async Task<TariffPercentageDto> CreateAsync(CreateTariffPercentageDto dto, CancellationToken cancellationToken = default)
    {
        // Validate input
        ValidateCreateDto(dto);

        // Check for overlapping date ranges for same TariffPeriodName
        await ValidateNoOverlap(dto.TariffPeriodName, dto.StartActiveDate, dto.EndActiveDate, excludeId: null, cancellationToken);

        var entity = new TariffPercentage
        {
            PercentageAdded = dto.PercentageAdded,
            TariffPeriodName = dto.TariffPeriodName,
            StartActiveDate = dto.StartActiveDate,
            EndActiveDate = dto.EndActiveDate,
            Status = "Pending",
            Notes = dto.Notes,
            DateInserted = DateTime.UtcNow,
            UserID = _currentUserService.UserId
        };

        _context.Set<TariffPercentage>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(entity);
    }

    public async Task<TariffPercentageDto> UpdateAsync(int id, UpdateTariffPercentageDto dto, CancellationToken cancellationToken = default)
    {
        var record = await _context.Set<TariffPercentage>()
            .FirstOrDefaultAsync(r => r.TariffPercentageId == id && r.DateDeleted == null, cancellationToken);

        if (record == null)
        {
            throw new KeyNotFoundException($"TariffPercentage with ID {id} not found.");
        }

        // Only Pending or Failed records can be updated
        if (record.Status != "Pending" && record.Status != "Failed")
        {
            throw new InvalidOperationException(
                $"Cannot modify a tariff percentage record with status '{record.Status}'. Only records with status 'Pending' or 'Failed' can be updated.");
        }

        // Apply changes
        if (dto.PercentageAdded.HasValue)
        {
            ValidatePercentage(dto.PercentageAdded.Value);
            record.PercentageAdded = dto.PercentageAdded.Value;
        }

        if (dto.StartActiveDate.HasValue)
        {
            record.StartActiveDate = dto.StartActiveDate.Value;
        }

        if (dto.EndActiveDate.HasValue)
        {
            record.EndActiveDate = dto.EndActiveDate.Value;
        }

        if (dto.Notes != null)
        {
            record.Notes = dto.Notes;
        }

        // Validate date range
        if (record.EndActiveDate.HasValue && record.EndActiveDate.Value < record.StartActiveDate)
        {
            throw new ArgumentException("EndActiveDate must be on or after StartActiveDate.");
        }

        // Check for overlapping date ranges (excluding this record)
        await ValidateNoOverlap(record.TariffPeriodName, record.StartActiveDate, record.EndActiveDate, excludeId: id, cancellationToken);

        record.DateUpdated = DateTime.UtcNow;
        record.UpdatedUserID = _currentUserService.UserId;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(record);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var record = await _context.Set<TariffPercentage>()
            .FirstOrDefaultAsync(r => r.TariffPercentageId == id && r.DateDeleted == null, cancellationToken);

        if (record == null)
        {
            return false;
        }

        // Only Pending or Failed records can be deleted
        if (record.Status != "Pending" && record.Status != "Failed")
        {
            throw new InvalidOperationException(
                $"Cannot delete a tariff percentage record with status '{record.Status}'. Only records with status 'Pending' or 'Failed' can be deleted.");
        }

        // Soft-delete
        record.DateDeleted = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<TariffUpdateJobStatus> ApplyPercentageAsync(int tariffPercentageId, CancellationToken cancellationToken = default)
    {
        var record = await _context.Set<TariffPercentage>()
            .FirstOrDefaultAsync(r => r.TariffPercentageId == tariffPercentageId && r.DateDeleted == null, cancellationToken);

        if (record == null)
        {
            throw new KeyNotFoundException($"TariffPercentage with ID {tariffPercentageId} not found.");
        }

        // Reject if record is already "Completed"
        if (record.Status == "Completed")
        {
            throw new InvalidOperationException(
                "Cannot apply a tariff percentage that has already been successfully applied.");
        }

        // Reject if record is currently "Processing"
        if (record.Status == "Processing")
        {
            throw new InvalidOperationException(
                "A propagation job is already in progress for this record.");
        }

        // Only allow "Pending" or "Failed" records to be applied
        if (record.Status != "Pending" && record.Status != "Failed")
        {
            throw new InvalidOperationException(
                $"Cannot apply a tariff percentage record with status '{record.Status}'. Only records with status 'Pending' or 'Failed' can be applied.");
        }

        // Check if another job is currently Processing for the same TariffPeriodName
        var processingForSamePeriod = await _context.Set<TariffPercentage>()
            .AnyAsync(r => r.TariffPeriodName == record.TariffPeriodName
                        && r.TariffPercentageId != tariffPercentageId
                        && r.Status == "Processing"
                        && r.DateDeleted == null, cancellationToken);

        if (processingForSamePeriod)
        {
            throw new InvalidOperationException(
                "A propagation job is already in progress for this period.");
        }

        // Set Status to "Processing" BEFORE writing to channel (requirement 5.6)
        record.Status = "Processing";
        record.DateUpdated = DateTime.UtcNow;
        record.UpdatedUserID = _currentUserService.UserId;
        await _context.SaveChangesAsync(cancellationToken);

        // Create the job
        var job = new TariffUpdateJob
        {
            TariffPercentageId = record.TariffPercentageId,
            PercentageAdded = record.PercentageAdded,
            TariffPeriodName = record.TariffPeriodName,
            StartActiveDate = record.StartActiveDate,
            EndActiveDate = record.EndActiveDate,
            QueuedAt = DateTime.UtcNow
        };

        // Track job status in-memory
        var jobStatus = new TariffUpdateJobStatus
        {
            JobId = job.JobId,
            Status = "Queued",
            StartedAt = null,
            CompletedAt = null,
            RecordsAffected = null,
            ErrorMessage = null
        };
        _jobStatuses[job.JobId] = jobStatus;

        // Write job to channel
        await _channelWriter.WriteAsync(job, cancellationToken);

        return jobStatus;
    }

    public Task<TariffUpdateJobStatus> GetJobStatusAsync(string jobId, CancellationToken cancellationToken = default)
    {
        if (_jobStatuses.TryGetValue(jobId, out var status))
        {
            return Task.FromResult(status);
        }

        throw new KeyNotFoundException($"Job with ID '{jobId}' not found.");
    }

    public async Task<IEnumerable<TariffPercentageDto>> GetActivePercentagesForLetterAsync(CancellationToken cancellationToken = default)
    {
        var allCompleted = await _context.Set<TariffPercentage>()
            .Where(tp => tp.Status == "Completed" && tp.DateDeleted == null)
            .ToListAsync(cancellationToken);

        if (allCompleted.Count == 0)
        {
            return Enumerable.Empty<TariffPercentageDto>();
        }

        var grouped = allCompleted
            .GroupBy(tp => tp.TariffPeriodName)
            .OrderByDescending(g => g.Key)
            .Take(2);

        var result = new List<TariffPercentageDto>();
        foreach (var yearGroup in grouped)
        {
            var latest = yearGroup
                .OrderByDescending(tp => tp.EndActiveDate == null ? DateOnly.MaxValue : tp.EndActiveDate.Value)
                .First();

            result.Add(MapToDto(latest));
        }

        return result;
    }

    #region Private Helpers

    private static void ValidateCreateDto(CreateTariffPercentageDto dto)
    {
        ValidatePercentage(dto.PercentageAdded);

        if (dto.TariffPeriodName < 2000 || dto.TariffPeriodName > 2100)
        {
            throw new ArgumentException("TariffPeriodName must be a 4-digit year between 2000 and 2100.");
        }

        if (dto.EndActiveDate.HasValue && dto.EndActiveDate.Value < dto.StartActiveDate)
        {
            throw new ArgumentException("EndActiveDate must be on or after StartActiveDate.");
        }
    }

    private static void ValidatePercentage(decimal percentage)
    {
        if (percentage < 0.0001m || percentage > 9999.9999m)
        {
            throw new ArgumentException("PercentageAdded must be between 0.0001 and 9999.9999.");
        }
    }

    private async Task ValidateNoOverlap(int tariffPeriodName, DateOnly startDate, DateOnly? endDate, int? excludeId, CancellationToken cancellationToken)
    {
        // Find all non-deleted records for the same TariffPeriodName
        var existingRecords = await _context.Set<TariffPercentage>()
            .Where(r => r.TariffPeriodName == tariffPeriodName && r.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var existing in existingRecords)
        {
            // Skip the record being updated
            if (excludeId.HasValue && existing.TariffPercentageId == excludeId.Value)
            {
                continue;
            }

            if (DateRangesOverlap(startDate, endDate, existing.StartActiveDate, existing.EndActiveDate))
            {
                throw new InvalidOperationException(
                    "A tariff percentage already exists for this period and date range.");
            }
        }
    }

    /// <summary>
    /// Checks if two date ranges overlap. A null end date is treated as open-ended (infinite).
    /// Two ranges [s1, e1] and [s2, e2] overlap if s1 &lt;= e2 AND s2 &lt;= e1.
    /// With null end dates treated as DateOnly.MaxValue.
    /// </summary>
    private static bool DateRangesOverlap(DateOnly start1, DateOnly? end1, DateOnly start2, DateOnly? end2)
    {
        var effectiveEnd1 = end1 ?? DateOnly.MaxValue;
        var effectiveEnd2 = end2 ?? DateOnly.MaxValue;

        return start1 <= effectiveEnd2 && start2 <= effectiveEnd1;
    }

    private static TariffPercentageDto MapToDto(TariffPercentage entity)
    {
        return new TariffPercentageDto
        {
            TariffPercentageId = entity.TariffPercentageId,
            PercentageAdded = entity.PercentageAdded,
            TariffPeriodName = entity.TariffPeriodName,
            StartActiveDate = entity.StartActiveDate,
            EndActiveDate = entity.EndActiveDate,
            Status = entity.Status,
            RecordsAffected = entity.RecordsAffected,
            Notes = entity.Notes,
            DateInserted = entity.DateInserted,
            UserID = entity.UserID
        };
    }

    #endregion
}
