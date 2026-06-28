using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("CaseNote", Schema = "CaseManagement")]
[Index("CaseId", Name = "idx_CaseManagement_CaseNote_FKs")]
public partial class CaseNote : BaseEntity
{
    [Key]
    [Column("CaseNoteID")]
    public int CaseNoteId { get; set; }

    [Column("CaseNote")]
    [Unicode(false)]
    public string? CaseNote1 { get; set; }

    public DateTime? DateCreated { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimAmount { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? CaseNumber { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimHospital { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimRadiology { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimDialysis { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimSpecialist { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimPhysio { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimTransport { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimAccomodation { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? InterimScript { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CaseNotes")]
    public virtual Case? Case { get; set; }
}
