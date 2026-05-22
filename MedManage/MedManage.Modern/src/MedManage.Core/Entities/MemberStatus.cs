using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("MemberStatus", Schema = "shared")]
public partial class MemberStatus : BaseEntity
{
    [Key]
    [Column("MemberStatusID")]
    public int MemberStatusId { get; set; }

    [Column("MemberStatus")]
    [StringLength(200)]
    [Unicode(false)]
    public string? MemberStatus1 { get; set; }

    [InverseProperty("MemberStatus")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
