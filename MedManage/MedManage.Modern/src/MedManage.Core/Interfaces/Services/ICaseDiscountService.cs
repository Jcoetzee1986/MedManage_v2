using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for managing case discounts and provider discount lookups.
/// </summary>
public interface ICaseDiscountService
{
    /// <summary>
    /// Get all discounts for a specific case.
    /// </summary>
    Task<IEnumerable<CaseDiscountDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a discount for a case.
    /// </summary>
    Task<CaseDiscountDto> CreateAsync(int caseId, CreateCaseDiscountDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a discount from a case (by caseId and discount value, since entity is keyless).
    /// </summary>
    Task<bool> DeleteAsync(int caseId, decimal discount, CancellationToken cancellationToken = default);
}
