using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Case_Billing", Schema = "CaseManagement")]
public partial class CaseBilling : BaseEntity
{
    [Key]
    [Column("Case_BillingID")]
    public int CaseBillingId { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [Column("ServiceProviderID")]
    public int? ServiceProviderId { get; set; }

    public DateOnly? AccountDate { get; set; }

    public DateOnly? AccountToDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? AccountNumber { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? InvoiceNumber { get; set; }

    public DateOnly? DateReceived { get; set; }

    public bool? Submitted { get; set; }

    public DateOnly? DateSubmitted { get; set; }

    public bool? Reported { get; set; }

    public DateOnly? DateReported { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ReceivedByName { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? AmountDue { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Penalty { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Rejected { get; set; }

    public bool? Paid { get; set; }

    public DateOnly? DatePaid { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Remittance { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? FinalInvoiceAmountDue { get; set; }

    [Column("BillingStatusID")]
    public int? BillingStatusId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PatientName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PatientInitials { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PatientSurname { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Comment { get; set; }
}
