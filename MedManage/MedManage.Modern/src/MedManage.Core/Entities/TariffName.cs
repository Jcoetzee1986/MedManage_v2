using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("TariffName", Schema = "shared")]
public partial class TariffName : BaseEntity
{
    [Key]
    [Column("TariffNameID")]
    public int TariffNameId { get; set; }

    [Column("TariffName")]
    [StringLength(200)]
    [Unicode(false)]
    public string? TariffName1 { get; set; }

    public bool? Visible { get; set; }

    [InverseProperty("TariffName")]
    public virtual ICollection<MedicalAidTariff> MedicalAidTariffs { get; set; } = new List<MedicalAidTariff>();
}
