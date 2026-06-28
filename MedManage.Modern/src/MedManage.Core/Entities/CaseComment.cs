using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseComment", Schema = "CaseManagement")]
[Index("CaseId", Name = "_dta_index_CaseComment_CaseID")]
public partial class CaseComment : BaseEntity
{
    [Key]
    [Column("CaseCommentID")]
    public int CaseCommentId { get; set; }

    [Column("CaseComment")]
    [Unicode(false)]
    public string? CaseComment1 { get; set; }

    public DateTime? DateCreated { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseComments")]
    public virtual Case? Case { get; set; }
}
