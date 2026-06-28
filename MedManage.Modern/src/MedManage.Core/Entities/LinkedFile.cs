using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("LinkedFile", Schema = "shared")]
public partial class LinkedFile : BaseEntity
{
    [Key]
    [Column("LinkedFileID")]
    public int LinkedFileId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EntityType { get; set; } = null!;

    [Column("EntityID")]
    public int? EntityId { get; set; }

    [Unicode(false)]
    public string? FilePath { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? FileName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdded { get; set; }
}
