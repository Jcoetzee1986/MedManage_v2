using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseManagement_AuditDetail", Schema = "audit")]
public partial class CaseManagementAuditDetail : BaseEntity
{
    [Key]
    [Column("AuditDetailID")]
    public int AuditDetailId { get; set; }

    [Column("AuditID")]
    public int? AuditId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? FieldName { get; set; }

    [Unicode(false)]
    public string? OldValue { get; set; }

    [Unicode(false)]
    public string? NewValue { get; set; }
}
