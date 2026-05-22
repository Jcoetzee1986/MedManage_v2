using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Episode", Schema = "CaseManagement")]
public partial class Episode : BaseEntity
{
    [Key]
    [Column("EpisodeID")]
    public int EpisodeId { get; set; }

    [Unicode(false)]
    public string? EpisodeDescription { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    public DateOnly? DateCreated { get; set; }
}
