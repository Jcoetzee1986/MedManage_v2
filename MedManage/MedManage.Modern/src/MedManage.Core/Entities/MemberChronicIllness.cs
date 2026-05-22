using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("MemberId", "ChronicIllnessId")]
[Table("Member_ChronicIllness", Schema = "shared")]
public partial class MemberChronicIllness : BaseEntity
{
    [Key]
    [Column("MemberID")]
    public int MemberId { get; set; }

    [Key]
    [Column("ChronicIllnessID")]
    public int ChronicIllnessId { get; set; }

    [ForeignKey("ChronicIllnessId")]
    [InverseProperty("MemberChronicIllnesses")]
    public virtual ChronicIllness1 ChronicIllness { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("MemberChronicIllnesses")]
    public virtual Member Member { get; set; } = null!;
}
