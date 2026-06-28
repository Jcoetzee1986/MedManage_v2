using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseStatus", Schema = "CaseManagement")]
public partial class CaseStatus : BaseEntity
{
    [Key]
    [Column("CaseStatusID")]
    public int CaseStatusId { get; set; }

    [Column("CaseStatus")]
    [StringLength(200)]
    [Unicode(false)]
    public string? CaseStatus1 { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
}
