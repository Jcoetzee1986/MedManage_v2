using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Session_User_Case", Schema = "CaseManagement")]
public partial class SessionUserCase : BaseEntity
{
    [Key]
    [Column("CaseID")]
    public int CaseId { get; set; }
}
