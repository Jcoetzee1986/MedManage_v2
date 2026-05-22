using System.ComponentModel.DataAnnotations.Schema;

namespace MedManage.Core.Entities;

/// <summary>
/// Base entity with common properties for all domain entities
/// </summary>
public abstract class BaseEntity
{
    [Column(TypeName = "datetime")]
    public DateTime? DateInserted { get; set; }
    
    [Column("UserID")]
    public string? UserID { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime? DateUpdated { get; set; }
    
    [Column("UpdatedUserID")]
    public string? UpdatedUserID { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime? DateDeleted { get; set; }
}

/// <summary>
/// Base entity with MainClientId for multi-tenant support
/// </summary>
public abstract class TenantEntity : BaseEntity
{
    public int MainClientID { get; set; }
}
