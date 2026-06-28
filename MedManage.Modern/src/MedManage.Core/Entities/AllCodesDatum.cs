using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

/// <summary>
/// PLACEHOLDER: AllCodesData table does not exist in the database.
/// This entity is kept to avoid build errors. Remove or regenerate when table is available.
/// </summary>
[Table("AllCodesData")]
public partial class AllCodesDatum : BaseEntity
{
    [Key]
    public int RecordKey { get; set; }
    
    [StringLength(3)]
    public string? CasePrefix { get; set; }
}
