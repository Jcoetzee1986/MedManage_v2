using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("ServiceProvider_MainClient_Discount", Schema = "finance")]
[Index("MainClientId", Name = "idx_ServiceProvider_MainClient_Discount_MainClientID")]
public partial class ServiceProviderMainClientDiscount : BaseEntity
{
    [Column("ServiceProviderID")]
    public int ServiceProviderId { get; set; }

    [Column("MainClientID")]
    public int MainClientId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Discount { get; set; }
}
