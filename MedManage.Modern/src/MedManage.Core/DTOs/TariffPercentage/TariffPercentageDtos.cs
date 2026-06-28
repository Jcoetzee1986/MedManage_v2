using System.ComponentModel.DataAnnotations;

namespace MedManage.Core.DTOs.TariffPercentage;

/// <summary>
/// DTO for displaying tariff percentage records.
/// </summary>
public class TariffPercentageDto
{
    public int TariffPercentageId { get; set; }
    public decimal PercentageAdded { get; set; }
    public int TariffPeriodName { get; set; }
    public DateOnly StartActiveDate { get; set; }
    public DateOnly? EndActiveDate { get; set; }
    public string? Status { get; set; }
    public int? RecordsAffected { get; set; }
    public string? Notes { get; set; }
    public DateTime? DateInserted { get; set; }
    public string? UserID { get; set; }
}

/// <summary>
/// DTO for creating a new tariff percentage record.
/// </summary>
public class CreateTariffPercentageDto
{
    [Required]
    [Range(0.0001, 9999.9999)]
    public decimal PercentageAdded { get; set; }

    [Required]
    [Range(2000, 2100)]
    public int TariffPeriodName { get; set; }

    [Required]
    public DateOnly StartActiveDate { get; set; }

    public DateOnly? EndActiveDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for updating an existing tariff percentage record.
/// All fields are optional — only provided fields are applied.
/// </summary>
public class UpdateTariffPercentageDto
{
    [Range(0.0001, 9999.9999)]
    public decimal? PercentageAdded { get; set; }

    public DateOnly? StartActiveDate { get; set; }

    public DateOnly? EndActiveDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
