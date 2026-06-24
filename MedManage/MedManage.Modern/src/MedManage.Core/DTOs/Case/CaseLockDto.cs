namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Represents the lock state of a case
/// </summary>
public class CaseLockDto
{
    /// <summary>Whether the case is currently locked (and the lock is still active/not expired)</summary>
    public bool IsLocked { get; set; }

    /// <summary>The case ID</summary>
    public int CaseId { get; set; }

    /// <summary>The user ID holding the lock (null if not locked)</summary>
    public string? LockedByUserId { get; set; }

    /// <summary>The username holding the lock (null if not locked)</summary>
    public string? LockedByUserName { get; set; }

    /// <summary>When the lock was originally acquired</summary>
    public DateTime? LockedAt { get; set; }

    /// <summary>When the lock was last refreshed (heartbeat). Used to determine expiry.</summary>
    public DateTime? LastActivity { get; set; }

    /// <summary>When the lock will expire if not refreshed (informational)</summary>
    public DateTime? ExpiresAt { get; set; }
}
