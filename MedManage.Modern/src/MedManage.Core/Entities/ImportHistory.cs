using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Table("ImportHistory", Schema = "shared")]
public class ImportHistory : BaseEntity
{
    [Key]
    [Column("ImportHistoryID")]
    public int ImportHistoryId { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ImportType { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    [Unicode(false)]
    public string FileName { get; set; } = string.Empty;

    [Column(TypeName = "datetime")]
    public DateTime ImportDate { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ImportedBy { get; set; }

    public int TotalRecords { get; set; }

    public int ImportedRecords { get; set; }

    public int SkippedRecords { get; set; }

    public int ErrorRecords { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = string.Empty;

    [Unicode(false)]
    public string? ErrorDetails { get; set; }
}
