using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("Case_Tariff", Schema = "Tariff")]
[Index("CaseIdTariffId", Name = "_dta_index_Case_Tariff_CaseID_TariffID")]
[Index("CaseId", Name = "idx_Case_Tariff__CaseID")]
public partial class CaseTariff : BaseEntity
{
    [Column("CaseID_TariffID")]
    public long CaseIdTariffId { get; set; }

    [Column("CaseID")]
    public int CaseId { get; set; }

    [Column("TariffID")]
    public int TariffId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Value { get; set; }

    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? AgreedRateOverride { get; set; }

    public bool? ValueIsTotal { get; set; }

    public bool? Rejected { get; set; }

    public DateOnly DateOfProcedure { get; set; }
}
