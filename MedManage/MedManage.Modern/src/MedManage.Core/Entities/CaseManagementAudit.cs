using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseManagement_Audit", Schema = "audit")]
public partial class CaseManagementAudit : BaseEntity
{
    [Key]
    [Column("AuditID")]
    public int AuditId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? UserName { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ObjectName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? EventType { get; set; }

    public DateTime? EventTime { get; set; }

    [Column("ID1")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Id1 { get; set; }

    [Column("ID2")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Id2 { get; set; }

    [Column("ID3")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Id3 { get; set; }
}
