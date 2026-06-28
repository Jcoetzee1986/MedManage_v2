using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("MedicalAid", Schema = "shared")]
public partial class MedicalAid : BaseEntity
{
    [Key]
    [Column("MedicalAidID")]
    public int MedicalAidId { get; set; }

    [Column("MainClientID")]
    public int? MainClientId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? MedicalAidName { get; set; }

    public DateOnly? MedicalAidInitiationDate { get; set; }

    public DateOnly? MedicalAidReinstatedDate { get; set; }

    public DateOnly? MedicalAidTerminatedDate { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? CasePrefix { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ReportTemplate { get; set; }

    [InverseProperty("MedicalAid")]
    public virtual ICollection<MedicalAidExclusion> MedicalAidExclusions { get; set; } = new List<MedicalAidExclusion>();

    [InverseProperty("MedicalAid")]
    public virtual ICollection<MedicalAidTariff> MedicalAidTariffs { get; set; } = new List<MedicalAidTariff>();

    [InverseProperty("MedicalAid")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
