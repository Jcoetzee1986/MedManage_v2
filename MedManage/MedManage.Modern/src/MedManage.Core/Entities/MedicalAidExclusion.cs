using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("MedicalAidId", "BaseTariffId")]
[Table("MedicalAid_Exclusion", Schema = "shared")]
public partial class MedicalAidExclusion : BaseEntity
{
    [Key]
    [Column("MedicalAidID")]
    public int MedicalAidId { get; set; }

    [Key]
    [Column("BaseTariffID")]
    [StringLength(100)]
    [Unicode(false)]
    public string BaseTariffId { get; set; } = null!;

    [ForeignKey("BaseTariffId")]
    [InverseProperty("MedicalAidExclusions")]
    public virtual BaseTariff BaseTariff { get; set; } = null!;

    [ForeignKey("MedicalAidId")]
    [InverseProperty("MedicalAidExclusions")]
    public virtual MedicalAid MedicalAid { get; set; } = null!;
}
