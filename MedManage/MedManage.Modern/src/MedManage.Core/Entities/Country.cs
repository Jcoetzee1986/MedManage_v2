using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Country", Schema = "shared")]
public partial class Country : BaseEntity
{
    [Key]
    [Column("CountryID")]
    public int CountryId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CountryName { get; set; }

    [Column("CountryISOCode")]
    [StringLength(3)]
    [Unicode(false)]
    public string? CountryIsocode { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? CountryCurrencyCode { get; set; }

    [Column("VAT", TypeName = "decimal(5, 2)")]
    public decimal? Vat { get; set; }

    [InverseProperty("MemberCountry")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
