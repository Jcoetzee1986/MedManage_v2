using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("MainClient", Schema = "shared")]
public partial class MainClient : BaseEntity
{
    [Key]
    [Column("MainClientID")]
    public int MainClientId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MainClientName { get; set; }

    public byte[]? MainClientLogo { get; set; }

    [Column("VAT", TypeName = "decimal(10, 2)")]
    public decimal? Vat { get; set; }
}
