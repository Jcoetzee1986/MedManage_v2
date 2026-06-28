using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Table("LetterTemplate", Schema = "shared")]
public class LetterTemplate : BaseEntity
{
    [Key]
    [Column("LetterTemplateID")]
    public int LetterTemplateId { get; set; }

    [Column("MainClientID")]
    public int? MainClientId { get; set; }

    [StringLength(100)]
    public string TemplateName { get; set; } = null!;

    [StringLength(50)]
    public string TemplateType { get; set; } = "CaseLetter"; // CaseLetter, DischargeForm, ReferralLetter

    [Column(TypeName = "nvarchar(max)")]
    public string? HtmlContent { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? HeaderHtml { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? FooterHtml { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? LogoBase64 { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDefault { get; set; } = false;
}
