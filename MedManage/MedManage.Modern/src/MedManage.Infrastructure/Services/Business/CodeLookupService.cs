using MedManage.Core.DTOs.CodeLookup;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service for searching CPT, ICD, and NAPPI codes with typeahead support.
/// Returns top N matching results ordered by code for efficient autocomplete.
/// </summary>
public class CodeLookupService : ICodeLookupService
{
    private readonly MedManageDbContext _context;

    public CodeLookupService(MedManageDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CodeLookupDto>> SearchCptAsync(string query, int limit = 20, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<CodeLookupDto>();

        var trimmed = query.Trim();

        return await _context.Cpts
            .Where(c => c.DateDeleted == null &&
                (c.Code != null && c.Code.Contains(trimmed) ||
                 c.ShortDescr != null && c.ShortDescr.Contains(trimmed)))
            .OrderBy(c => c.Code)
            .Take(limit)
            .Select(c => new CodeLookupDto
            {
                Id = c.Cptid,
                Code = c.Code ?? string.Empty,
                Description = c.ShortDescr ?? string.Empty
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CodeLookupDto>> SearchIcdAsync(string query, int limit = 20, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<CodeLookupDto>();

        var trimmed = query.Trim();

        return await _context.Icds
            .Where(i => i.DateDeleted == null &&
                (i.DiagnosisCode != null && i.DiagnosisCode.Contains(trimmed) ||
                 i.DiagnosisDesc != null && i.DiagnosisDesc.Contains(trimmed)))
            .OrderBy(i => i.DiagnosisCode)
            .Take(limit)
            .Select(i => new CodeLookupDto
            {
                Id = i.Icdid,
                Code = i.DiagnosisCode ?? string.Empty,
                Description = i.DiagnosisDesc ?? string.Empty
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NappiCodeLookupDto>> SearchNappiAsync(string query, DateTime? effectiveDate = null, int limit = 20, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<NappiCodeLookupDto>();

        var trimmed = query.Trim();

        var baseQuery = _context.NappiCodes
            .Where(n => n.DateDeleted == null &&
                (n.Code != null && n.Code.Contains(trimmed) ||
                 n.Description != null && n.Description.Contains(trimmed)));

        if (effectiveDate.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(effectiveDate.Value);
            baseQuery = baseQuery.Where(n =>
                (!n.StartDate.HasValue || n.StartDate <= dateOnly) &&
                (!n.EndDate.HasValue || n.EndDate >= dateOnly));
        }

        return await baseQuery
            .OrderBy(n => n.Code)
            .Take(limit)
            .Select(n => new NappiCodeLookupDto
            {
                Id = n.NappiId,
                Code = n.Code ?? string.Empty,
                Description = n.Description ?? string.Empty,
                StartDate = n.StartDate,
                EndDate = n.EndDate
            })
            .ToListAsync(cancellationToken);
    }
}
