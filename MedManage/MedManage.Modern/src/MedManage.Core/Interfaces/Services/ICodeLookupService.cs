using MedManage.Core.DTOs.CodeLookup;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for searching CPT, ICD, and NAPPI codes with typeahead support
/// </summary>
public interface ICodeLookupService
{
    /// <summary>
    /// Search CPT codes by code or description
    /// </summary>
    Task<IEnumerable<CodeLookupDto>> SearchCptAsync(string? query, int limit = 20, CancellationToken cancellationToken = default, string? code = null, string? description = null);

    /// <summary>
    /// Search ICD codes by code or description
    /// </summary>
    Task<IEnumerable<CodeLookupDto>> SearchIcdAsync(string? query, int limit = 20, CancellationToken cancellationToken = default, string? code = null, string? description = null);

    /// <summary>
    /// Search NAPPI codes by code or description, optionally filtered by effective date
    /// </summary>
    Task<IEnumerable<NappiCodeLookupDto>> SearchNappiAsync(string? query, DateTime? effectiveDate = null, int limit = 20, CancellationToken cancellationToken = default, string? code = null, string? description = null);
}
