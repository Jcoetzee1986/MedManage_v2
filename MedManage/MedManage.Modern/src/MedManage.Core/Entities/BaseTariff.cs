using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("BaseTariff", Schema = "shared")]
[Index("SpecialityId", Name = "idx_Tariff_SpecialityID")]
public partial class BaseTariff : BaseEntity
{
    [Key]
    [Column("BaseTariffID")]
    [StringLength(100)]
    [Unicode(false)]
    public string BaseTariffId { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? TariffCode { get; set; }

    [Column("SpecialityID")]
    public int? SpecialityId { get; set; }

    [Unicode(false)]
    public string? TariffDescription { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? ChargeCodes { get; set; }

    [InverseProperty("BaseTariff")]
    public virtual ICollection<MedicalAidExclusion> MedicalAidExclusions { get; set; } = new List<MedicalAidExclusion>();
}
