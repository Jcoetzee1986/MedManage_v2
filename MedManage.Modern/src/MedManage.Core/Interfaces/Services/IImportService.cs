using MedManage.Core.DTOs.Import;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for handling data file imports (DRD members, NAPPI codes, billing)
/// </summary>
public interface IImportService
{
    /// <summary>
    /// Import DRD member file - parses CSV, validates, and merges into Members table
    /// </summary>
    Task<ImportResultDto> ImportMembersAsync(Stream fileStream, string fileName);

    /// <summary>
    /// Import NAPPI code file - parses CSV, validates, and updates NappiCodes table
    /// </summary>
    Task<ImportResultDto> ImportNappiCodesAsync(Stream fileStream, string fileName);

    /// <summary>
    /// Import billing file - parses CSV, validates, and creates billing records
    /// </summary>
    Task<ImportResultDto> ImportBillingAsync(Stream fileStream, string fileName);

    /// <summary>
    /// Get import history with optional filtering
    /// </summary>
    Task<IEnumerable<ImportHistoryDto>> GetImportHistoryAsync(string? importType = null, int? limit = 50);
}
