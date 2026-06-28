using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Tariff", Schema = "Tariff")]
[Index("TariffNameId", "BaseTariffId", "StartDate", Name = "idx_Tariff_TariffNameIDBaseTariffIDStartDateEndDate")]
public partial class Tariff : BaseEntity
{
    [Key]
    [Column("TariffID")]
    public int TariffId { get; set; }

    [Column("BaseTariffID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? BaseTariffId { get; set; }

    [Column("TariffNameID")]
    public int? TariffNameId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TariffAmount { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Metric { get; set; }

    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Quantity { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? TariffPeriodName { get; set; }
}
