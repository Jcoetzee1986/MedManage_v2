using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

/// <summary>
/// Refresh token entity for JWT token rotation
/// </summary>
[Table("RefreshTokens")]
[Index(nameof(Token), IsUnique = true)]
[Index(nameof(UserId))]
[Index(nameof(ExpiresAt))]
public partial class RefreshToken
{
    [Key]
    public Guid TokenId { get; set; }

    /// <summary>
    /// User ID this token belongs to
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The refresh token string
    /// </summary>
    [StringLength(500)]
    public string Token { get; set; } = null!;

    /// <summary>
    /// When the token expires
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// When the token was created
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When the token was revoked (null if still valid)
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Whether the token has been used (for one-time use rotation)
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// IP address when token was created
    /// </summary>
    [MaxLength(45)] // IPv6 max length
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent when token was created
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Token that replaced this one (for rotation chain)
    /// </summary>
    public Guid? ReplacedByTokenId { get; set; }

    /// <summary>
    /// Reason token was revoked
    /// </summary>
    [MaxLength(200)]
    public string? RevocationReason { get; set; }

    // Custom audit fields (not using BaseEntity to avoid conflicts)
    [Column(TypeName = "datetime")]
    public DateTime DateInserted { get; set; }

    [Column("CreatedByUserID")]
    [StringLength(256)]
    public string CreatedByUserID { get; set; } = string.Empty;

    [Column(TypeName = "datetime")]
    public DateTime? DateUpdated { get; set; }

    [Column("UpdatedByUserID")]
    [StringLength(256)]
    public string? UpdatedByUserID { get; set; }

    // Computed properties
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [NotMapped]
    public bool IsRevoked => RevokedAt != null;

    [NotMapped]
    public bool IsActive => !IsRevoked && !IsExpired && !IsUsed;

    // Navigation properties
    public virtual AspnetUser User { get; set; } = null!;
}
