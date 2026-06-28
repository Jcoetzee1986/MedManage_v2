using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("SuspendedReason", Schema = "shared")]
public partial class SuspendedReason : BaseEntity
{
    [Key]
    [Column("SuspendedReasonID")]
    public int SuspendedReasonId { get; set; }

    [Column("SuspendedReason")]
    [StringLength(200)]
    [Unicode(false)]
    public string? SuspendedReason1 { get; set; }
}
