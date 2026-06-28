using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("ParentCase", "ChildCase")]
[Table("Case_Link", Schema = "CaseManagement")]
public partial class CaseLink : BaseEntity
{
    [Key]
    public int ParentCase { get; set; }

    [Key]
    public int ChildCase { get; set; }
}
