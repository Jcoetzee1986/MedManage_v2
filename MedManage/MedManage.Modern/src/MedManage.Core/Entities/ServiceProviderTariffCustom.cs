using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("ServiceProvider_Tariff_Custom", Schema = "Tariff")]
[Index("ServiceProviderId", "MainClientId", "StartDate", Name = "idx_ServiceProvider_Tariff_Custom__BaseTariffID_TariffAmount_EndDate")]
[Index("ServiceProviderId", "BaseTariffId", "MainClientId", "StartDate", Name = "idx_ServiceProvider_Tariff_Custom__ServiceProviderID_BaseTariffID_MainClientID_StartDate")]
public partial class ServiceProviderTariffCustom : BaseEntity
{
    [Column("ServiceProvider_Tariff_CustomID")]
    public long ServiceProviderTariffCustomId { get; set; }

    [Column("ServiceProviderID")]
    public int ServiceProviderId { get; set; }

    [Column("BaseTariffID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? BaseTariffId { get; set; }

    [Column("MainClientID")]
    public int? MainClientId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TariffAmount { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
