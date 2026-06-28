using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Keyless]
[Table("xHospitalType", Schema = "shared")]
public partial class XHospitalType : BaseEntity
{
    [Column("HospitalTypeID")]
    public int HospitalTypeId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? HospitalTypeName { get; set; }
}
