using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("SystemData", Schema = "shared")]
public partial class SystemDatum : BaseEntity
{
    [Key]
    [Column("SystemDataID")]
    public int SystemDataId { get; set; }

    [Column("SystemCountryID")]
    public int? SystemCountryId { get; set; }

    public Guid? SystemUniqueIdentifier { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? SystemEmailAddress { get; set; }

    [Column("SMTPServer")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Smtpserver { get; set; }

    [Column("SSL")]
    public bool? Ssl { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Username { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column("SpecialICU")]
    public int? SpecialIcu { get; set; }

    [Column("ICU")]
    public int? Icu { get; set; }

    public int? HighCare { get; set; }

    public int? NeuroWard { get; set; }

    public int? InIsolation { get; set; }

    public int? GeneralWard { get; set; }

    public int? Paediatric { get; set; }

    public int? Maternity { get; set; }

    public int? DayCase { get; set; }

    public int? StepDown { get; set; }

    public int? Psychiatric { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Address1 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Address2 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Address3 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Address4 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? AddressCode { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Fax { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Telephone { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Website { get; set; }

    public byte[]? Logo { get; set; }

    [Column("DefaultProviderID")]
    public int? DefaultProviderId { get; set; }
}
