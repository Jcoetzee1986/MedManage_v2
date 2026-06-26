using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Table("aspnet_Membership")]
public partial class AspnetMembership
{
    public Guid ApplicationId { get; set; }

    [Key]
    public Guid UserId { get; set; }

    [StringLength(128)]
    public string Password { get; set; } = null!;

    public int PasswordFormat { get; set; }

    [StringLength(128)]
    public string PasswordSalt { get; set; } = null!;

    [Column("MobilePIN")]
    [StringLength(16)]
    public string? MobilePin { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }

    [StringLength(256)]
    public string? LoweredEmail { get; set; }

    [StringLength(256)]
    public string? PasswordQuestion { get; set; }

    [StringLength(128)]
    public string? PasswordAnswer { get; set; }

    public bool IsApproved { get; set; }

    public bool IsLockedOut { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastLoginDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastPasswordChangedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastLockoutDate { get; set; }

    public int FailedPasswordAttemptCount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FailedPasswordAttemptWindowStart { get; set; }

    public int FailedPasswordAnswerAttemptCount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

    [Column(TypeName = "ntext")]
    public string? Comment { get; set; }

    /// <summary>
    /// Indicates whether the user has been permanently blocked/deactivated by an administrator.
    /// Unlike IsLockedOut (which is temporary from failed attempts), this is a permanent deactivation.
    /// </summary>
    public bool IsPermanentlyBlocked { get; set; }

    // Audit columns
    [Column(TypeName = "datetime")]
    public DateTime DateInserted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateUpdated { get; set; }

    [Column("UpdatedUserID")]
    [StringLength(256)]
    public string? UpdatedUserID { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("AspnetMemberships")]
    public virtual AspnetApplication Application { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AspnetMembership")]
    public virtual AspnetUser User { get; set; } = null!;
}
