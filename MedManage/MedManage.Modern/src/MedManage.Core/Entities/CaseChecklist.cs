using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("CaseId", "ChecklistTemplateId")]
[Table("Case_Checklist", Schema = "CaseManagement")]
public partial class CaseChecklist : BaseEntity
{
    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }

    [Key]
    [Column("ChecklistTemplateID")]
    public int ChecklistTemplateId { get; set; }

    public bool? Checked { get; set; }

    public DateTime? Date { get; set; }

    [Unicode(false)]
    public string? Comments { get; set; }

    public bool? NotApplicable { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseChecklists")]
    public virtual Case Case { get; set; } = null!;

    [ForeignKey("ChecklistTemplateId")]
    [InverseProperty("CaseChecklists")]
    public virtual ChecklistTemplate ChecklistTemplate { get; set; } = null!;
}
