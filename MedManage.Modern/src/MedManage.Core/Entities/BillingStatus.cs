using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("BillingStatus", Schema = "finance")]
public partial class BillingStatus : BaseEntity
{
    [Key]
    [Column("BillingStatusID")]
    public int BillingStatusId { get; set; }

    [Column("BillingStatus")]
    [StringLength(50)]
    [Unicode(false)]
    public string? BillingStatus1 { get; set; }
}
