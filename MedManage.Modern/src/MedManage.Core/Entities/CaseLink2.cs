using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("CaseLink", Schema = "stg")]
public partial class CaseLink2 : BaseEntity
{
    public int ParentCase { get; set; }

    public int ChildCase { get; set; }

    public int OrigParent { get; set; }
}
