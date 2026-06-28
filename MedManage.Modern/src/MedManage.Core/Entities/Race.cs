using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Race", Schema = "shared")]
public partial class Race : BaseEntity
{
    [Key]
    [Column("RaceID")]
    public int RaceId { get; set; }

    [Column("Race")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Race1 { get; set; }

    [InverseProperty("MemberRace")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
