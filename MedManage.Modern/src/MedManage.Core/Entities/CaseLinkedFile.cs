using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Case_LinkedFile", Schema = "CaseManagement")]
[Index("CaseId", "FileType", "FileName", "DateAdded", "UserID", "CaseLinkedFileId", "MemberId", Name = "_dta_index_Case_LinkedFile_7_7007106__K2_K4_K6_K7_K8_K1_K3_5")]
[Index("MemberId", "CaseId", "FileType", "FileName", "DateAdded", "UserID", "CaseLinkedFileId", Name = "_dta_index_Case_LinkedFile_7_7007106__K3_K2_K4_K6_K7_K8_K1_5")]
public partial class CaseLinkedFile : BaseEntity
{
    [Key]
    [Column("Case_LinkedFileID")]
    public int CaseLinkedFileId { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FileType { get; set; }

    [Unicode(false)]
    public string? FilePath { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? FileName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdded { get; set; }
}
