using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("ChronicIllness")]
public partial class ChronicIllness : BaseEntity
{
    [Column("ChronicIllnessID")]
    public double? ChronicIllnessId { get; set; }

    public string? ChronicIllnessName { get; set; }

    public string? ChronicIllnessDescription { get; set; }

    [StringLength(255)]
    public string? DateAdded { get; set; }
}
