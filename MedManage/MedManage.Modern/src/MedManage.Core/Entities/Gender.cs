using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Gender", Schema = "shared")]
public partial class Gender : BaseEntity
{
    [Key]
    [Column("GenderID")]
    public int GenderId { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? GenderCode { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? GenderDescription { get; set; }

    [InverseProperty("Gender")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
