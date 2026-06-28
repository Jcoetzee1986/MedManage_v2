using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseLetterNote", Schema = "CaseManagement")]
public partial class CaseLetterNote : BaseEntity
{
    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }

    [Unicode(false)]
    public string? Note { get; set; }

    public bool? IncludeDischargeForm { get; set; }

    public bool? IncludeReferralLetter { get; set; }
}
