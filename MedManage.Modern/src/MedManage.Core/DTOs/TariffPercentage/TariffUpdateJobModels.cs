namespace MedManage.Core.DTOs.TariffPercentage;

/// <summary>
/// Internal model representing a tariff update job queued for background processing.
/// </summary>
public class TariffUpdateJob
{
    public string JobId { get; set; } = Guid.NewGuid().ToString();
    public int TariffPercentageId { get; set; }
    public decimal PercentageAdded { get; set; }
    public int TariffPeriodName { get; set; }
    public DateOnly StartActiveDate { get; set; }
    public DateOnly? EndActiveDate { get; set; }
    public DateTime QueuedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Model representing the current status of a tariff update job.
/// </summary>
public class TariffUpdateJobStatus
{
    public string JobId { get; set; } = null!;

    /// <summary>
    /// Current job status: Queued, Processing, Completed, or Failed.
    /// </summary>
    public string Status { get; set; } = null!;

    public int? RecordsAffected { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
