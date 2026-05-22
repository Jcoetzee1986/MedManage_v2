using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("EpisodeId", "CaseId")]
[Table("Episode_Case", Schema = "CaseManagement")]
public partial class EpisodeCase : BaseEntity
{
    [Key]
    [Column("EpisodeID")]
    public int EpisodeId { get; set; }

    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }

    public DateOnly? DateCreated { get; set; }
}
