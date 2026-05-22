using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("Case_Discount", Schema = "finance")]
public partial class CaseDiscount : BaseEntity
{
    [Column("CaseID")]
    public int? CaseId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Discount { get; set; }
}
