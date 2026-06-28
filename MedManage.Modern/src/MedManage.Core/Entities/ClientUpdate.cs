using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("ClientUpdates", Schema = "Updates")]
public partial class ClientUpdate : BaseEntity
{
    [Key]
    [Column("ClientUpdateID")]
    public int ClientUpdateId { get; set; }

    [Column("UpdateID")]
    public int? UpdateId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ClientComputerName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }
}
