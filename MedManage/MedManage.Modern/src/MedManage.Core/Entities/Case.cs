using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Cases", Schema = "CaseManagement")]
[Index("UserID", "MemberId", "CaseId", "StatusId", "ReferToId", "ReferFromId", "DateCreated", Name = "_dta_index_Cases_7_493960836__K19_K4_K1_K17_K5_K6_K23_2_3_7_8_9_10_24_25")]
[Index("MemberId", "AuthNumber", "ReferToId", "AuthTypeId", "ReferFromId", "StatusId", "CaseId", "DateCreated", Name = "_dta_index_Cases_7_493960836__K4_K2_K5_K11_K6_K17_K1_K23_3_7_8_9_10_19_24_25")]
[Index("AdmissionDate", "AuthNumber", "MemberId", "CaseId", "ReferToId", "ReferFromId", "StatusId", "DateCreated", Name = "_dta_index_Cases_7_493960836__K7_K2_K4_K1_K5_K6_K17_K23_3_8_9_10_13_14_15_19_24_25")]
[Index("AuthNumber", "StatusId", Name = "_dta_index_Cases_AuthNumber_StatusID")]
[Index("DateCreated", Name = "_dta_index_Cases_DateCreated")]
public partial class Case : BaseEntity
{
    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AuthNumber { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AccountNr { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    [Column("ReferToID")]
    public int? ReferToId { get; set; }

    [Column("ReferFromID")]
    public int? ReferFromId { get; set; }

    public DateOnly? AdmissionDate { get; set; }

    public TimeOnly? AdmissionTime { get; set; }

    public DateOnly? DischargeDate { get; set; }

    public TimeOnly? DischargeTime { get; set; }

    /// <summary>
    /// Speciality
    /// </summary>
    [Column("AuthTypeID")]
    public int? AuthTypeId { get; set; }

    [Column("WCA_IOD")]
    public bool? WcaIod { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalLengthOfStay { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? FinalInvoiceAmount { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? FinalInvoiceAmountUpdated { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [Unicode(false)]
    public string? CaseDescription { get; set; }

    [Unicode(false)]
    public string? Changes { get; set; }

    [Unicode(false)]
    public string? Limits { get; set; }

    [Unicode(false)]
    public string? Exclusions { get; set; }

    public DateOnly? DateCreated { get; set; }

    public bool? HasBooking { get; set; }

    public DateTime? ChangeToCaseDate { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? PenaltyPercentage { get; set; }

    [Column("CaseCategoryID")]
    public int? CaseCategoryId { get; set; }

    [InverseProperty("Case")]
    public virtual ICollection<CaseChecklist> CaseChecklists { get; set; } = new List<CaseChecklist>();

    [InverseProperty("Case")]
    public virtual ICollection<CaseComment> CaseComments { get; set; } = new List<CaseComment>();

    [InverseProperty("Case")]
    public virtual ICollection<CaseCpt> CaseCpts { get; set; } = new List<CaseCpt>();

    [InverseProperty("Case")]
    public virtual ICollection<CaseExclusion> CaseExclusions { get; set; } = new List<CaseExclusion>();

    [InverseProperty("Case")]
    public virtual ICollection<CaseFacilityType> CaseFacilityTypes { get; set; } = new List<CaseFacilityType>();

    [InverseProperty("Case")]
    public virtual ICollection<CaseIcd> CaseIcds { get; set; } = new List<CaseIcd>();

    [InverseProperty("Case")]
    public virtual ICollection<CaseNote> CaseNotes { get; set; } = new List<CaseNote>();

    [ForeignKey("MemberId")]
    [InverseProperty("Cases")]
    public virtual Member? Member { get; set; }

    [ForeignKey("ReferFromId")]
    [InverseProperty("CaseReferFroms")]
    public virtual ServiceProvider? ReferFrom { get; set; }

    [ForeignKey("ReferToId")]
    [InverseProperty("CaseReferTos")]
    public virtual ServiceProvider? ReferTo { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Cases")]
    public virtual CaseStatus? Status { get; set; }

    [ForeignKey("AuthTypeId")]
    public virtual CaseType? AuthType { get; set; }
}
