using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("FacilityType", Schema = "shared")]
public partial class FacilityType : BaseEntity
{
    [Key]
    [Column("FacilityTypeID")]
    public int FacilityTypeId { get; set; }

    [Column("FacilityType")]
    [StringLength(100)]
    [Unicode(false)]
    public string? FacilityType1 { get; set; }

    [InverseProperty("FacilityType")]
    public virtual ICollection<CaseFacilityType> CaseFacilityTypes { get; set; } = new List<CaseFacilityType>();
}
