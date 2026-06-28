using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[PrimaryKey("MainClientId", "MedicalAidProductId")]
[Table("MainClient_MedicalAidProduct", Schema = "shared")]
public partial class MainClientMedicalAidProduct : BaseEntity
{
    [Key]
    [Column("MainClientID")]
    public int MainClientId { get; set; }

    [Key]
    [Column("MedicalAidProductID")]
    public int MedicalAidProductId { get; set; }

    public int Sorting { get; set; }
}
