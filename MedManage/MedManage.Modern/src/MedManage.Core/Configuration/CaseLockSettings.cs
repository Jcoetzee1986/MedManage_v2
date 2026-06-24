namespace MedManage.Core.Configuration;

/// <summary>
/// Configuration for case lock behavior.
/// Locks automatically expire after a configurable period of inactivity.
/// </summary>
public class CaseLockSettings
{
    public const string SectionName = "CaseLock";

    /// <summary>
    /// Number of hours a lock can remain without activity before it is automatically released.
    /// Default: 5 hours.
    /// </summary>
    public double InactivityTimeoutHours { get; set; } = 5;

    /// <summary>
    /// Interval in minutes between cleanup sweeps that release expired locks.
    /// Default: 15 minutes.
    /// </summary>
    public int CleanupIntervalMinutes { get; set; } = 15;

    /// <summary>
    /// Whether to release all locks for a user on logout.
    /// Default: true.
    /// </summary>
    public bool ReleaseLocksOnLogout { get; set; } = true;
}
