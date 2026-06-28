using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("MedicalAidId", "TariffNameId")]
[Table("MedicalAid_Tariff", Schema = "shared")]
public partial class MedicalAidTariff : BaseEntity
{
    [Key]
    [Column("MedicalAidID")]
    public int MedicalAidId { get; set; }

    [Key]
    [Column("TariffNameID")]
    public int TariffNameId { get; set; }

    [ForeignKey("MedicalAidId")]
    [InverseProperty("MedicalAidTariffs")]
    public virtual MedicalAid MedicalAid { get; set; } = null!;

    [ForeignKey("TariffNameId")]
    [InverseProperty("MedicalAidTariffs")]
    public virtual TariffName TariffName { get; set; } = null!;
}
