using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Case_FacilityType", Schema = "CaseManagement")]
[Index("CaseId", "FacilityTypeId", "DateAdmitted", Name = "_dta_index_Case_FacilityType_7_2129442660__K3_K2_K4_1_5_6_7_8_9_11")]
public partial class CaseFacilityType : BaseEntity
{
    [Key]
    [Column("CaseID_FacilityTypeID")]
    public int CaseIdFacilityTypeId { get; set; }

    [Column("FacilityTypeID")]
    public int FacilityTypeId { get; set; }

    [Column("CaseID")]
    public int CaseId { get; set; }

    [Precision(0)]
    public DateTime DateAdmitted { get; set; }

    [Precision(0)]
    public DateTime? DateDischarged { get; set; }

    [Column("LOS", TypeName = "decimal(10, 1)")]
    public decimal? Los { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? FacilityTypeCode { get; set; }

    public int? MinutesOnVentilator { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Comments { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseFacilityTypes")]
    public virtual Case Case { get; set; } = null!;

    [ForeignKey("FacilityTypeId")]
    [InverseProperty("CaseFacilityTypes")]
    public virtual FacilityType FacilityType { get; set; } = null!;
}
