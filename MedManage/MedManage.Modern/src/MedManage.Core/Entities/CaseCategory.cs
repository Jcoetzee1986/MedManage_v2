using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseCategory", Schema = "CaseManagement")]
public partial class CaseCategory : BaseEntity
{
    [Key]
    [Column("CaseCategoryID")]
    public int CaseCategoryId { get; set; }

    [Column("CaseCategory")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CaseCategory1 { get; set; }
}
