using MedManage.Core.DTOs.CodeLookup;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service for searching CPT, ICD, and NAPPI codes with typeahead support.
/// Returns top N matching results ordered by code for efficient autocomplete.
/// When 'code' param is provided, uses StartsWith (left-match).
/// When 'description' param is provided, uses Contains (wildcard).
/// </summary>
public class CodeLookupService : ICodeLookupService
{
    private readonly MedManageDbContext _context;

    public CodeLookupService(MedManageDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CodeLookupDto>> SearchCptAsync(string? query, int limit = 20, CancellationToken cancellationToken = default, string? code = null, string? description = null)
    {
        var q = _context.Cpts.Where(c => c.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(code))
        {
            var trimCode = code.Trim();
            q = q.Where(c => c.Code != null && c.Code.StartsWith(trimCode));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            var trimDesc = description.Trim();
            q = q.Where(c => c.ShortDescr != null && c.ShortDescr.Contains(trimDesc));
        }

        // Fallback to the generic 'q' param (wildcard on both)
        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<CodeLookupDto>();

            var trimmed = query.Trim();
            q = q.Where(c =>
                (c.Code != null && c.Code.StartsWith(trimmed)) ||
                (c.ShortDescr != null && c.ShortDescr.Contains(trimmed)));
        }

        return await q
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

    public async Task<IEnumerable<CodeLookupDto>> SearchIcdAsync(string? query, int limit = 20, CancellationToken cancellationToken = default, string? code = null, string? description = null)
    {
        var q = _context.Icds.Where(i => i.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(code))
        {
            var trimCode = code.Trim();
            q = q.Where(i => i.DiagnosisCode != null && i.DiagnosisCode.StartsWith(trimCode));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            var trimDesc = description.Trim();
            q = q.Where(i => i.DiagnosisDesc != null && i.DiagnosisDesc.Contains(trimDesc));
        }

        // Fallback to the generic 'q' param
        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<CodeLookupDto>();

            var trimmed = query.Trim();
            q = q.Where(i =>
                (i.DiagnosisCode != null && i.DiagnosisCode.StartsWith(trimmed)) ||
                (i.DiagnosisDesc != null && i.DiagnosisDesc.Contains(trimmed)));
        }

        return await q
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

    public async Task<IEnumerable<NappiCodeLookupDto>> SearchNappiAsync(string? query, DateTime? effectiveDate = null, int limit = 20, CancellationToken cancellationToken = default, string? code = null, string? description = null)
    {
        var q = _context.NappiCodes.Where(n => n.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(code))
        {
            var trimCode = code.Trim();
            q = q.Where(n => n.Code != null && n.Code.StartsWith(trimCode));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            var trimDesc = description.Trim();
            q = q.Where(n => n.Description != null && n.Description.Contains(trimDesc));
        }

        // Fallback to the generic 'q' param
        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(description))
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<NappiCodeLookupDto>();

            var trimmed = query.Trim();
            q = q.Where(n =>
                (n.Code != null && n.Code.StartsWith(trimmed)) ||
                (n.Description != null && n.Description.Contains(trimmed)));
        }

        if (effectiveDate.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(effectiveDate.Value);
            q = q.Where(n =>
                (!n.StartDate.HasValue || n.StartDate <= dateOnly) &&
                (!n.EndDate.HasValue || n.EndDate >= dateOnly));
        }

        return await q
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
