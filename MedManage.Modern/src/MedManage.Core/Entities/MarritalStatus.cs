using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("MarritalStatus", Schema = "shared")]
public partial class MarritalStatus : BaseEntity
{
    [Key]
    [Column("MarritalStatusID")]
    public int MarritalStatusId { get; set; }

    [Column("MarritalStatus")]
    [StringLength(50)]
    [Unicode(false)]
    public string? MarritalStatus1 { get; set; }

    [InverseProperty("MarritalStatus")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
