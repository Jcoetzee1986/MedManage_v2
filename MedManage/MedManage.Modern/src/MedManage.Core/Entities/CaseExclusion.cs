using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("CaseId", "ExclusionId")]
[Table("Case_Exclusion", Schema = "CaseManagement")]
public partial class CaseExclusion : BaseEntity
{
    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }

    [Key]
    [Column("ExclusionID")]
    public int ExclusionId { get; set; }

    [Unicode(false)]
    public string? Comment { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseExclusions")]
    public virtual Case Case { get; set; } = null!;

    [ForeignKey("ExclusionId")]
    [InverseProperty("CaseExclusions")]
    public virtual Exclusion Exclusion { get; set; } = null!;
}
