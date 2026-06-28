using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[PrimaryKey("UserId", "RoleId")]
[Table("aspnet_UsersInRoles")]
[Index("RoleId", Name = "aspnet_UsersInRoles_index")]
public partial class AspnetUsersInRole
{
    [Key]
    public Guid UserId { get; set; }

    [Key]
    public Guid RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("AspnetUsersInRoles")]
    public virtual AspnetRole Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AspnetUsersInRoles")]
    public virtual AspnetUser User { get; set; } = null!;
}
