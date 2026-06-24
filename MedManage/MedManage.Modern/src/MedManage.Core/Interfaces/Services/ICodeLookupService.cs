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
    /// <param name="query">Search term (matches code or description)</param>
    /// <param name="limit">Maximum results to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<CodeLookupDto>> SearchCptAsync(string query, int limit = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search ICD codes by code or description
    /// </summary>
    /// <param name="query">Search term (matches code or description)</param>
    /// <param name="limit">Maximum results to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<CodeLookupDto>> SearchIcdAsync(string query, int limit = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search NAPPI codes by code or description, optionally filtered by effective date
    /// </summary>
    /// <param name="query">Search term (matches code or description)</param>
    /// <param name="effectiveDate">Optional date to filter codes valid on that date</param>
    /// <param name="limit">Maximum results to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<NappiCodeLookupDto>> SearchNappiAsync(string query, DateTime? effectiveDate = null, int limit = 20, CancellationToken cancellationToken = default);
}
