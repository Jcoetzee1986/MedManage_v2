using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace MedManage.Core.Entities;

[Table("aspnet_Applications")]
[Index("LoweredApplicationName", Name = "UQ__aspnet_A__17477DE46CEB3894", IsUnique = true)]
[Index("ApplicationName", Name = "UQ__aspnet_A__30910331C28A4DF3", IsUnique = true)]
public partial class AspnetApplication
{
    [StringLength(256)]
    public string ApplicationName { get; set; } = null!;

    [StringLength(256)]
    public string LoweredApplicationName { get; set; } = null!;

    [Key]
    public Guid ApplicationId { get; set; }

    [StringLength(256)]
    public string? Description { get; set; }

    [InverseProperty("Application")]
    public virtual ICollection<AspnetMembership> AspnetMemberships { get; set; } = new List<AspnetMembership>();

    [InverseProperty("Application")]
    public virtual ICollection<AspnetPath> AspnetPaths { get; set; } = new List<AspnetPath>();

    [InverseProperty("Application")]
    public virtual ICollection<AspnetRole> AspnetRoles { get; set; } = new List<AspnetRole>();

    [InverseProperty("Application")]
    public virtual ICollection<AspnetUser> AspnetUsers { get; set; } = new List<AspnetUser>();
}
