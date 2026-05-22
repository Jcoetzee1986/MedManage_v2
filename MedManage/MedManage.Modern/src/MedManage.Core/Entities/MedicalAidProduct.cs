using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("MedicalAidProduct", Schema = "shared")]
public partial class MedicalAidProduct : BaseEntity
{
    [Key]
    [Column("MedAidProductID")]
    public int MedAidProductId { get; set; }

    [Column("MainClientID")]
    public int? MainClientId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MedAidProductName { get; set; }

    public bool? AllowServices { get; set; }
}
