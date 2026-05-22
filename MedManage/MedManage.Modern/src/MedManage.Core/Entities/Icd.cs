using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("ICD", Schema = "shared")]
[Index("Icdid", Name = "_dta_index_ICD_ICDID")]
[Index("DiagnosisCode", Name = "idx_Shared_ICD_Code")]
public partial class Icd : BaseEntity
{
    [Key]
    [Column("ICDID")]
    public int Icdid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? GroupCode { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? GroupDesc { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DiagnosisCode { get; set; }

    [Unicode(false)]
    public string? DiagnosisDesc { get; set; }

    public bool? Dagger { get; set; }

    public bool? Asterisk { get; set; }

    public bool? ValidPrimaryCode { get; set; }

    public bool? ValidForClinicalUse { get; set; }

    [InverseProperty("Icd")]
    public virtual ICollection<CaseIcd> CaseIcds { get; set; } = new List<CaseIcd>();
}
