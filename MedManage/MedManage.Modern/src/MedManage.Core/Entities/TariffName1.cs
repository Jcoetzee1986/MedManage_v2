using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("TariffName", Schema = "Tariff")]
public partial class TariffName1 : BaseEntity
{
    [Column("TariffNameID")]
    public int TariffNameId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? TariffName { get; set; }

    public bool? Visible { get; set; }
}
