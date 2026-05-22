using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseType", Schema = "CaseManagement")]
public partial class CaseType : BaseEntity
{
    [Key]
    [Column("CaseTypeID")]
    public int CaseTypeId { get; set; }

    [Column("CaseType")]
    [StringLength(200)]
    [Unicode(false)]
    public string? CaseType1 { get; set; }
}
