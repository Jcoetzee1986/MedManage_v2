using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("ServiceProvider_Tariff", Schema = "Tariff")]
[Index("ServiceProviderId", "MainClientId", Name = "idx_ServiceProvider_Tariff_ServiceProviderID_MainClientID")]
public partial class ServiceProviderTariff : BaseEntity
{
    [Column("ServiceProvider_TariffID")]
    public long ServiceProviderTariffId { get; set; }

    [Column("ServiceProviderID")]
    public int ServiceProviderId { get; set; }

    [Column("TariffNameID")]
    public int? TariffNameId { get; set; }

    [Column("MainClientID")]
    public int? MainClientId { get; set; }

    public DateOnly? StartActiveDate { get; set; }

    public DateOnly? EndActiveDate { get; set; }

    public int? TariffPeriodName { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? PercentageAdded { get; set; }
}
