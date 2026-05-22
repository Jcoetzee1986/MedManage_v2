using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("ChecklistTemplate", Schema = "CaseManagement")]
public partial class ChecklistTemplate : BaseEntity
{
    [Key]
    [Column("ChecklistTemplateID")]
    public int ChecklistTemplateId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ChecklistPrompt { get; set; }

    [InverseProperty("ChecklistTemplate")]
    public virtual ICollection<CaseChecklist> CaseChecklists { get; set; } = new List<CaseChecklist>();
}
