using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Speciality", Schema = "shared")]
public partial class Speciality : BaseEntity
{
    [Key]
    [Column("SpecialityID")]
    public int SpecialityId { get; set; }

    [Column("Speciality")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Speciality1 { get; set; }

    [InverseProperty("Speciality")]
    public virtual ICollection<ServiceProvider> ServiceProviders { get; set; } = new List<ServiceProvider>();
}
