using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Member_MedicalAidProduct", Schema = "shared")]
public partial class MemberMedicalAidProduct : BaseEntity
{
    [Key]
    [Column("MedAidProductID_MemberID")]
    public int MedAidProductIdMemberId { get; set; }

    [Column("MedAidProductID")]
    public int? MedAidProductId { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string StartDate { get; set; } = null!;

    public DateOnly? EndDate { get; set; }
}
