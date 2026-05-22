using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("CaseId", "Icdid")]
[Table("Case_ICD", Schema = "CaseManagement")]
[Index("PrimaryCode", "CaseId", Name = "_dta_index_Case_ICD_PrimaryCode_CaseID")]
public partial class CaseIcd : BaseEntity
{
    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }

    [Key]
    [Column("ICDID")]
    public int Icdid { get; set; }

    public DateOnly? DateOfProcedure { get; set; }

    public bool? PrimaryCode { get; set; }

    public bool? SecondaryCode { get; set; }

    public bool? CoMorbidityCode { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseIcds")]
    public virtual Case Case { get; set; } = null!;

    [ForeignKey("Icdid")]
    [InverseProperty("CaseIcds")]
    public virtual Icd Icd { get; set; } = null!;
}
