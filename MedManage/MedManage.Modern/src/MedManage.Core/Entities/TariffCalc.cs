using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("TariffCalc", Schema = "Overnight")]
public partial class TariffCalc : BaseEntity
{
    [Key]
    [Column("TariffCalcID")]
    public int TariffCalcId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TariffCode { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Value { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? AgreedRate { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(22, 8)")]
    public decimal? DiscountValue { get; set; }

    [Column(TypeName = "decimal(23, 8)")]
    public decimal? TotalPayable { get; set; }

    [Column(TypeName = "decimal(11, 2)")]
    public decimal? TotalOvercharged { get; set; }

    [Column(TypeName = "decimal(5, 1)")]
    public decimal Qty { get; set; }

    public DateOnly DateOfProcedure { get; set; }

    [Unicode(false)]
    public string? TariffDescription { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [Column("TariffID")]
    public int? TariffId { get; set; }

    public int Seq { get; set; }

    [Column("BaseTariffID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? BaseTariffId { get; set; }

    public bool? Rejected { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string Colour { get; set; } = null!;
}
