using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("PeriodInCountry", Schema = "shared")]
public partial class PeriodInCountry : BaseEntity
{
    [Key]
    [Column("PeriodInCountryID")]
    public int PeriodInCountryId { get; set; }

    [Column("PeriodInCountry")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PeriodInCountry1 { get; set; }

    [InverseProperty("PeriodInCountry")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
