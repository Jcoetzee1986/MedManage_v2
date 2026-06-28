using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("ChronicIllness", Schema = "shared")]
public partial class ChronicIllness1 : BaseEntity
{
    [Key]
    [Column("ChronicIllnessID")]
    public int ChronicIllnessId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? ChronicIllnessName { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? ChronicIllnessDescription { get; set; }

    public DateTime? DateAdded { get; set; }

    [InverseProperty("ChronicIllness")]
    public virtual ICollection<MemberChronicIllness> MemberChronicIllnesses { get; set; } = new List<MemberChronicIllness>();
}
