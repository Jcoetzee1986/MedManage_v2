using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Language", Schema = "shared")]
public partial class Language : BaseEntity
{
    [Key]
    [Column("LanguageID")]
    public int LanguageId { get; set; }

    [Column("Language")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Language1 { get; set; }

    [InverseProperty("MemberLanguage")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
