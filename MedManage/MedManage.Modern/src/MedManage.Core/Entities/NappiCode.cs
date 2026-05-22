using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("NappiCodes", Schema = "shared")]
public partial class NappiCode : BaseEntity
{
    [Key]
    [Column("NappiID")]
    public int NappiId { get; set; }

    [Column("NappiCode")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Code { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Description { get; set; }

    public int? Units { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Measure { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price1 { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price2 { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
