using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Table("PasswordResetTokens")]
[Index(nameof(Token), IsUnique = true)]
[Index(nameof(Pin), IsUnique = true)]
[Index(nameof(UserId))]
public partial class PasswordResetToken
{
    [Key]
    public Guid TokenId { get; set; }

    public Guid UserId { get; set; }

    [StringLength(100)]
    public string Token { get; set; } = null!;

    [StringLength(6)]
    public string Pin { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    // Custom audit fields (not using BaseEntity to avoid conflicts)
    [Column(TypeName = "datetime")]
    public DateTime DateInserted { get; set; }

    [Column("CreatedByUserID")]
    public int? CreatedByUserID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateUpdated { get; set; }

    [Column("UpdatedByUserID")]
    public int? UpdatedByUserID { get; set; }

    // Navigation properties
    public virtual AspnetUser User { get; set; } = null!;
}
