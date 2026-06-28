using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Keyless]
public partial class VwCaseDetailWithAmount
{
    [StringLength(200)]
    [Unicode(false)]
    public string? CaseStatus { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AuthNumber { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AccountNr { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? CasePrefix { get; set; }

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

    public DateOnly? AdmissionDate { get; set; }

    public DateOnly? DischargeDate { get; set; }

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

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalLengthOfStay { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? FinalInvoiceAmount { get; set; }

    [Column("MBOD_RMA")]
    public bool? MbodRma { get; set; }

    public bool? HasBooking { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? CaseType { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? PenaltyPercentage { get; set; }

    [Column(TypeName = "decimal(21, 8)")]
    public decimal? FinalInvoiceAmountWithPenalty { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimHospital { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimRadiology { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimDialysis { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimSpecialist { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimPhysio { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimTransport { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimAccomodation { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimScript { get; set; }

    public int? ParentCase { get; set; }

    [Column("CaseID")]
    public int CaseId { get; set; }
}
