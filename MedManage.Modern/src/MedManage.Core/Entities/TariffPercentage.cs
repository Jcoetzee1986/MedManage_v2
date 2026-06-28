using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedManage.Core.Entities;

[Table("TariffPercentage", Schema = "Tariff")]
public class TariffPercentage : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TariffPercentageId { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal PercentageAdded { get; set; }

    public int TariffPeriodName { get; set; } // Year (e.g., 2026)

    public DateOnly StartActiveDate { get; set; }

    public DateOnly? EndActiveDate { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; } // Pending, Processing, Completed, Failed

    public int? RecordsAffected { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
