using System;
using System.ComponentModel.DataAnnotations;

namespace MedManage.Core.DTOs.CaseBilling;

/// <summary>
/// DTO representing a case discount.
/// </summary>
public class CaseDiscountDto
{
    public int? CaseId { get; set; }
    public decimal? Discount { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

/// <summary>
/// Request to create a case discount.
/// </summary>
public class CreateCaseDiscountDto
{
    /// <summary>
    /// The discount percentage to apply.
    /// </summary>
    [Required]
    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
    public decimal Discount { get; set; }
}

/// <summary>
/// Request to update a case discount.
/// </summary>
public class UpdateCaseDiscountDto
{
    /// <summary>
    /// The updated discount percentage.
    /// </summary>
    [Required]
    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
    public decimal Discount { get; set; }
}
