using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("MemberNote", Schema = "shared")]
public partial class MemberNote : BaseEntity
{
    [Key]
    [Column("MemberNoteID")]
    public int MemberNoteId { get; set; }

    [Column("MemberNote")]
    [Unicode(false)]
    public string? MemberNote1 { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    public DateTime? DateCreated { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("MemberNotes")]
    public virtual Member? Member { get; set; }
}
