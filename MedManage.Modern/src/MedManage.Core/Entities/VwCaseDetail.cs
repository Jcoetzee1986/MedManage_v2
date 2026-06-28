using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Keyless]
public partial class VwCaseDetail
{
    [StringLength(200)]
    [Unicode(false)]
    public string? CaseStatus { get; set; }

    public bool? HasBooking { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MainClientName { get; set; }

    [Column("MainClientID")]
    public int MainClientId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AuthNumber { get; set; }

    public DateOnly? AdmissionDate { get; set; }

    public DateOnly? DischargeDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AccountNr { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? MemberNumber { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Surname { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Initials { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ReferTo { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ReferFrom { get; set; }

    public TimeOnly? AdmissionTime { get; set; }

    public TimeOnly? DischargeTime { get; set; }

    [Column("PrimaryICD")]
    [StringLength(50)]
    [Unicode(false)]
    public string PrimaryIcd { get; set; } = null!;

    [Column("PrimaryCPT")]
    [StringLength(20)]
    [Unicode(false)]
    public string PrimaryCpt { get; set; } = null!;

    [Column("PrimaryICDDesc")]
    [Unicode(false)]
    public string PrimaryIcddesc { get; set; } = null!;

    [Column("PrimaryCPTDesc")]
    [StringLength(255)]
    [Unicode(false)]
    public string PrimaryCptdesc { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? ReferToPracticeNumber { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? ReferFromPracticeNumber { get; set; }

    [Column("UserID")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UserId { get; set; }

    public DateOnly? DateCreated { get; set; }

    [Column("CaseID")]
    public int CaseId { get; set; }

    [Column("MedicalAidID")]
    public int? MedicalAidId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MedAidProductName { get; set; }

    [Column("MBOD_RMA")]
    public bool? MbodRma { get; set; }

    public DateTime? ChangeToCaseDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? FinalInvoiceAmountUpdated { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? FinalInvoiceAmount { get; set; }

    public int ParentCase { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }
}
