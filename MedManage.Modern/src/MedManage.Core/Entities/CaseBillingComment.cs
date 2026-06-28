using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Case_Billing_Comment", Schema = "CaseManagement")]
public partial class CaseBillingComment : BaseEntity
{
    [Key]
    [Column("Case_Billing_CommentID")]
    public int CaseBillingCommentId { get; set; }

    [Column("Case_BillingID")]
    public int? CaseBillingId { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Comment { get; set; }
}
