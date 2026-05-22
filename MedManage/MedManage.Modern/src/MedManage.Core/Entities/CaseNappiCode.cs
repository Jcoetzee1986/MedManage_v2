using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Case_NappiCodes", Schema = "CaseManagement")]
public partial class CaseNappiCode : BaseEntity
{
    [Key]
    [Column("CaseID_NappiID")]
    public int CaseIdNappiId { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [Column("NappiID")]
    public int? NappiId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Value { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Quantity { get; set; }

    public bool? Dispensary { get; set; }

    public bool? Ward { get; set; }

    public bool? Theater { get; set; }

    [Column("TTO")]
    public bool? Tto { get; set; }

    [Column("0201")]
    public bool? _0201 { get; set; }

    public DateOnly? Date { get; set; }
}
