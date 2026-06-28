using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("MainClient_Tariff", Schema = "shared")]
public partial class MainClientTariff : BaseEntity
{
    [Column("MainClientID")]
    public int MainClientId { get; set; }

    [Column("TariffNameID")]
    public int TariffNameId { get; set; }

    public int? TariffPeriodName { get; set; }
}
