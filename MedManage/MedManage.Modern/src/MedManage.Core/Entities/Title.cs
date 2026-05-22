using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Title", Schema = "shared")]
public partial class Title : BaseEntity
{
    [Key]
    [Column("TitleID")]
    public int TitleId { get; set; }

    [Column("Title")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Title1 { get; set; }

    [InverseProperty("Title")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
