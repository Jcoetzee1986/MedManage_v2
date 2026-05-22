using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CPT", Schema = "shared")]
[Index("Cptid", Name = "_dta_index_CPT_CPTID")]
[Index("Code", Name = "idx_Shared_CPT_Code")]
public partial class Cpt : BaseEntity
{
    [Key]
    [Column("CPTID")]
    public int Cptid { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Chapter { get; set; }

    [Column("CODE")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Code { get; set; }

    [Column("SHORT_DESCR")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ShortDescr { get; set; }

    [Column("MEDIUM_DESCR")]
    [StringLength(255)]
    [Unicode(false)]
    public string? MediumDescr { get; set; }

    [Column("LONG_DESCR")]
    [Unicode(false)]
    public string? LongDescr { get; set; }

    [InverseProperty("Cpt")]
    public virtual ICollection<CaseCpt> CaseCpts { get; set; } = new List<CaseCpt>();
}
