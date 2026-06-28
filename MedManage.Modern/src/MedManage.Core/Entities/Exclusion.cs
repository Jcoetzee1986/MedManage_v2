using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Exclusion", Schema = "shared")]
public partial class Exclusion : BaseEntity
{
    [Key]
    [Column("ExclusionID")]
    public int ExclusionId { get; set; }

    [Column("Exclusion")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Exclusion1 { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? ExclusionDescription { get; set; }

    [InverseProperty("Exclusion")]
    public virtual ICollection<CaseExclusion> CaseExclusions { get; set; } = new List<CaseExclusion>();
}
