using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("CountryVAT", Schema = "shared")]
public partial class CountryVat : BaseEntity
{
    [Column("CountryID")]
    public int CountryId { get; set; }

    [Column("VAT", TypeName = "decimal(5, 2)")]
    public decimal? Vat { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
