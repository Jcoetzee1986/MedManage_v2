using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Case_CPT", Schema = "CaseManagement")]
[Index("PrimaryCode", "CaseId", "CaseIdCptid", "Cptid", Name = "_dta_index_Case_CPT_7_1634820886__K5_K2_K1_K3")]
[Index("PrimaryCode", "Cptid", "CaseId", Name = "_dta_index_Case_CPT_PrimaryCode_CPTID_CaseID")]
[Index("CaseId", "Cptid", Name = "idx_CaseManagement_Case_FacilityType_FKs")]
public partial class CaseCpt : BaseEntity
{
    [Key]
    [Column("CaseID_CPTID")]
    public int CaseIdCptid { get; set; }

    [Column("CaseID")]
    public int CaseId { get; set; }

    [Column("CPTID")]
    public int Cptid { get; set; }

    public DateOnly? DateOfProcedure { get; set; }

    public bool? PrimaryCode { get; set; }

    public bool? SecondaryCode { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseCpts")]
    public virtual Case Case { get; set; } = null!;

    [ForeignKey("Cptid")]
    [InverseProperty("CaseCpts")]
    public virtual Cpt Cpt { get; set; } = null!;
}
