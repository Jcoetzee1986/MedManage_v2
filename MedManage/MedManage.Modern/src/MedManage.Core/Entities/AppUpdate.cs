using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("AppUpdates", Schema = "Updates")]
public partial class AppUpdate : BaseEntity
{
    [Key]
    [Column("UpdateID")]
    public int UpdateId { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? UpdatePath { get; set; }

    [Unicode(false)]
    public string? UpdateComments { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AppVersion { get; set; }
}
