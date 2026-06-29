using ClosedXML.Excel;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using MedManage.Core.DTOs.Report;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class ReportGenerationService : IReportGenerationService
{
    private readonly MedManageDbContext _db;
    private readonly ILogger<ReportGenerationService> _logger;

    public ReportGenerationService(MedManageDbContext db, ILogger<ReportGenerationService> logger)
    {
        _db = db;
        _logger = logger;
    }

    // ═══════════════════════════════════════════════════════════════════
    // 1. Cases Between Dates
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateCasesBetweenDatesAsync(CasesBetweenDatesRequest request, CancellationToken cancellationToken = default)
    {
        var query = _db.Cases
            .Include(c => c.Member).ThenInclude(m => m!.MedicalAid)
            .Include(c => c.Status)
            .Include(c => c.AuthType)
            .Include(c => c.ReferTo)
            .Where(c => c.DateDeleted == null
                && c.DateCreated >= request.DateFrom
                && c.DateCreated <= request.DateTo);

        if (request.MainClientId.HasValue)
            query = query.Where(c => c.Member != null && c.Member.MedicalAid != null && c.Member.MedicalAid.MainClientId == request.MainClientId);
        if (request.StatusId.HasValue)
            query = query.Where(c => c.StatusId == request.StatusId);
        if (request.CaseTypeId.HasValue)
            query = query.Where(c => c.AuthTypeId == request.CaseTypeId);

        var data = await query.OrderBy(c => c.AdmissionDate).Select(c => new
        {
            c.AuthNumber,
            Status = c.Status != null ? c.Status.CaseStatus1 : null,
            CaseType = c.AuthType != null ? c.AuthType.CaseType1 : null,
            MemberName = c.Member != null ? (c.Member.Surname + ", " + c.Member.Name) : null,
            MemberNumber = c.Member != null ? c.Member.MemberNumber : null,
            MedicalAid = c.Member != null && c.Member.MedicalAid != null ? c.Member.MedicalAid.MedicalAidName : null,
            Provider = c.ReferTo != null ? c.ReferTo.PracticeName : null,
            c.AdmissionDate,
            c.DischargeDate,
            c.TotalLengthOfStay,
            c.TotalAmount,
            c.FinalInvoiceAmount
        }).ToListAsync(cancellationToken);

        var headers = new[] { "Auth Number", "Status", "Case Type", "Member", "Member #", "Medical Aid", "Provider", "Admission", "Discharge", "LOS", "Total Amount", "Final Invoice" };

        if (request.Format == "pdf")
            return await GeneratePdfTable("Cases Between Dates", $"{request.DateFrom:yyyy/MM/dd} — {request.DateTo:yyyy/MM/dd}", headers, data.Select(d => new object?[] { d.AuthNumber, d.Status, d.CaseType, d.MemberName, d.MemberNumber, d.MedicalAid, d.Provider, d.AdmissionDate?.ToString("yyyy/MM/dd"), d.DischargeDate?.ToString("yyyy/MM/dd"), d.TotalLengthOfStay, d.TotalAmount, d.FinalInvoiceAmount }).ToList());

        return GenerateExcel("Cases Between Dates", headers, data.Select(d => new object?[] { d.AuthNumber, d.Status, d.CaseType, d.MemberName, d.MemberNumber, d.MedicalAid, d.Provider, d.AdmissionDate?.ToString("yyyy/MM/dd"), d.DischargeDate?.ToString("yyyy/MM/dd"), d.TotalLengthOfStay, d.TotalAmount, d.FinalInvoiceAmount }).ToList());
    }

    // ═══════════════════════════════════════════════════════════════════
    // 2. Billing Summary
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateBillingSummaryAsync(BillingSummaryRequest request, CancellationToken cancellationToken = default)
    {
        var query = from b in _db.CaseBillings
                    where b.DateDeleted == null
                    join c in _db.Cases on b.CaseId equals c.CaseId into cJoin from c in cJoin.DefaultIfEmpty()
                    join sp in _db.ServiceProviders on b.ServiceProviderId equals sp.ServiceProviderId into spJoin from sp in spJoin.DefaultIfEmpty()
                    select new { b, c, sp };

        if (request.DateFrom.HasValue)
            query = query.Where(x => x.b.DateReceived >= request.DateFrom);
        if (request.DateTo.HasValue)
            query = query.Where(x => x.b.DateReceived <= request.DateTo);
        if (request.ServiceProviderId.HasValue)
            query = query.Where(x => x.b.ServiceProviderId == request.ServiceProviderId);
        if (request.MainClientId.HasValue)
            query = query.Where(x => x.c != null && x.c.Member != null && x.c.Member.MedicalAid != null && x.c.Member.MedicalAid.MainClientId == request.MainClientId);

        var data = await query.OrderByDescending(x => x.b.DateReceived).Select(x => new
        {
            AuthNumber = x.c != null ? x.c.AuthNumber : null,
            x.b.AccountNumber,
            AccountDate = x.b.AccountDate,
            MemberName = x.b.PatientName,
            MemberSurname = x.b.PatientSurname,
            MemberNumber = x.c != null && x.c.Member != null ? x.c.Member.MemberNumber : null,
            PracticeName = x.sp != null ? x.sp.PracticeName : null,
            x.b.AmountDue,
            x.b.Discount,
            x.b.Penalty,
            x.b.FinalInvoiceAmountDue,
            x.b.DateReceived,
            x.b.DatePaid,
            x.b.Remittance,
            x.b.Paid,
            x.b.Comment
        }).ToListAsync(cancellationToken);

        var headers = new[] { "Auth #", "Account #", "Account Date", "Surname", "Name", "Member #", "Practice", "Amount Due", "Discount", "Penalty", "Final Invoice", "Received", "Paid Date", "Remittance", "Paid", "Comment" };

        var rows = data.Select(d => new object?[] { d.AuthNumber, d.AccountNumber, d.AccountDate?.ToString("yyyy/MM/dd"), d.MemberSurname, d.MemberName, d.MemberNumber, d.PracticeName, d.AmountDue, d.Discount, d.Penalty, d.FinalInvoiceAmountDue, d.DateReceived?.ToString("yyyy/MM/dd"), d.DatePaid?.ToString("yyyy/MM/dd"), d.Remittance, d.Paid == true ? "Yes" : "No", d.Comment }).ToList();

        if (request.Format == "pdf")
            return await GeneratePdfTable("Billing Summary", null, headers, rows);

        return GenerateExcel("Billing Summary", headers, rows);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 3. Case Tariff Detail (uses SP for calculated rates/discounts)
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateCaseTariffDetailAsync(CaseTariffDetailRequest request, CancellationToken cancellationToken = default)
    {
        // Use the SP which calculates agreed rates, discounts, penalties, overcharged amounts
        var tariffs = new List<Dictionary<string, object?>>();
        var connection = _db.Database.GetDbConnection();
        try
        {
            await connection.OpenAsync(cancellationToken);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Tariff.usp_Case_Tariff_Select";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CaseID", request.CaseId));
            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                tariffs.Add(row);
            }
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();
        }

        var headers = new[] { "Code", "Description", "Value", "Qty", "Agreed Rate", "Discount", "Overcharged", "Total Payable", "Penalty", "Date", "Rejected", "Colour" };
        var rows = tariffs.Select(t => new object?[]
        {
            t.GetValueOrDefault("TariffCode"),
            t.GetValueOrDefault("TariffDescription"),
            t.GetValueOrDefault("Value"),
            t.GetValueOrDefault("Qty"),
            t.GetValueOrDefault("AgreedRate"),
            t.GetValueOrDefault("Discount"),
            t.GetValueOrDefault("TotalOvercharged"),
            t.GetValueOrDefault("TotalPayable"),
            t.GetValueOrDefault("TotalPenalty"),
            t.GetValueOrDefault("DateOfProcedure") is DateTime dt ? dt.ToString("yyyy/MM/dd") : t.GetValueOrDefault("DateOfProcedure")?.ToString(),
            t.GetValueOrDefault("Rejected")?.ToString() == "True" ? "Yes" : "No",
            t.GetValueOrDefault("Colour")
        }).ToList();

        if (request.Format == "pdf")
            return await GeneratePdfTable($"Case Tariff Detail — Case {request.CaseId}", $"Penalty %: {tariffs.FirstOrDefault()?.GetValueOrDefault("PenaltyPercentage") ?? 0}", headers, rows);

        return GenerateExcel($"Tariff Detail Case {request.CaseId}", headers, rows);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 4. My Cases
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateMyCasesAsync(MyCasesRequest request, string userId, CancellationToken cancellationToken = default)
    {
        var query = _db.Cases
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Include(c => c.ReferTo)
            .Where(c => c.DateDeleted == null && c.UserID == userId);

        if (request.StatusId.HasValue)
            query = query.Where(c => c.StatusId == request.StatusId);

        var data = await query.OrderByDescending(c => c.DateInserted).Select(c => new
        {
            c.AuthNumber,
            Status = c.Status != null ? c.Status.CaseStatus1 : null,
            MemberName = c.Member != null ? (c.Member.Surname + ", " + c.Member.Name) : null,
            MemberNumber = c.Member != null ? c.Member.MemberNumber : null,
            Provider = c.ReferTo != null ? c.ReferTo.PracticeName : null,
            c.AdmissionDate,
            c.DischargeDate,
            c.TotalAmount
        }).ToListAsync(cancellationToken);

        var headers = new[] { "Auth Number", "Status", "Member", "Member #", "Provider", "Admission", "Discharge", "Amount" };

        if (request.Format == "pdf")
            return await GeneratePdfTable("My Cases", null, headers, data.Select(d => new object?[] { d.AuthNumber, d.Status, d.MemberName, d.MemberNumber, d.Provider, d.AdmissionDate?.ToString("yyyy/MM/dd"), d.DischargeDate?.ToString("yyyy/MM/dd"), d.TotalAmount }).ToList());

        return GenerateExcel("My Cases", headers, data.Select(d => new object?[] { d.AuthNumber, d.Status, d.MemberName, d.MemberNumber, d.Provider, d.AdmissionDate?.ToString("yyyy/MM/dd"), d.DischargeDate?.ToString("yyyy/MM/dd"), d.TotalAmount }).ToList());
    }

    // ═══════════════════════════════════════════════════════════════════
    // 5. Case Comments Export
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateCaseCommentsExportAsync(CaseCommentsExportRequest request, CancellationToken cancellationToken = default)
    {
        var comments = await _db.CaseComments
            .Where(c => c.CaseId == request.CaseId && c.DateDeleted == null)
            .OrderByDescending(c => c.DateCreated)
            .Select(c => new { c.CaseComment1, c.DateCreated, c.UserID })
            .ToListAsync(cancellationToken);

        var headers = new[] { "Comment", "Date", "User" };
        var rows = comments.Select(c => new object?[] { c.CaseComment1, c.DateCreated?.ToString("yyyy/MM/dd HH:mm"), c.UserID }).ToList();

        if (request.Format == "pdf")
            return await GeneratePdfTable($"Case Comments — Case {request.CaseId}", null, headers, rows);

        return GenerateExcel($"Comments Case {request.CaseId}", headers, rows);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 6. WIP Extract
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateWipExtractAsync(WipExtractRequest request, CancellationToken cancellationToken = default)
    {
        var query = _db.Cases
            .Include(c => c.Member).ThenInclude(m => m!.MedicalAid)
            .Include(c => c.Status)
            .Include(c => c.ReferTo)
            .Where(c => c.DateDeleted == null && c.StatusId != 3); // Exclude closed

        if (request.DateFrom.HasValue)
            query = query.Where(c => c.DateCreated >= request.DateFrom);
        if (request.DateTo.HasValue)
            query = query.Where(c => c.DateCreated <= request.DateTo);
        if (request.MainClientId.HasValue)
            query = query.Where(c => c.Member != null && c.Member.MedicalAid != null && c.Member.MedicalAid.MainClientId == request.MainClientId);

        var data = await query.OrderBy(c => c.AdmissionDate).Select(c => new
        {
            c.AuthNumber,
            Status = c.Status != null ? c.Status.CaseStatus1 : null,
            MemberName = c.Member != null ? (c.Member.Surname + ", " + c.Member.Name) : null,
            MemberNumber = c.Member != null ? c.Member.MemberNumber : null,
            MedicalAid = c.Member != null && c.Member.MedicalAid != null ? c.Member.MedicalAid.MedicalAidName : null,
            Provider = c.ReferTo != null ? c.ReferTo.PracticeName : null,
            c.AdmissionDate,
            c.DischargeDate,
            c.TotalLengthOfStay,
            c.TotalAmount,
            c.FinalInvoiceAmount,
            c.PenaltyPercentage
        }).ToListAsync(cancellationToken);

        var headers = new[] { "Auth Number", "Status", "Member", "Member #", "Medical Aid", "Provider", "Admission", "Discharge", "LOS", "Total Amount", "Final Invoice", "Penalty %" };

        if (request.Format == "pdf")
            return await GeneratePdfTable("WIP Extract (Work In Progress)", null, headers, data.Select(d => new object?[] { d.AuthNumber, d.Status, d.MemberName, d.MemberNumber, d.MedicalAid, d.Provider, d.AdmissionDate?.ToString("yyyy/MM/dd"), d.DischargeDate?.ToString("yyyy/MM/dd"), d.TotalLengthOfStay, d.TotalAmount, d.FinalInvoiceAmount, d.PenaltyPercentage }).ToList());

        return GenerateExcel("WIP Extract", headers, data.Select(d => new object?[] { d.AuthNumber, d.Status, d.MemberName, d.MemberNumber, d.MedicalAid, d.Provider, d.AdmissionDate?.ToString("yyyy/MM/dd"), d.DischargeDate?.ToString("yyyy/MM/dd"), d.TotalLengthOfStay, d.TotalAmount, d.FinalInvoiceAmount, d.PenaltyPercentage }).ToList());
    }

    // ═══════════════════════════════════════════════════════════════════
    // 7. Linked Cases
    // ═══════════════════════════════════════════════════════════════════
    public async Task<ReportOutput> GenerateLinkedCasesAsync(LinkedCasesRequest request, CancellationToken cancellationToken = default)
    {
        var links = await _db.CaseLinks
            .Where(l => l.ParentCase == request.CaseId || l.ChildCase == request.CaseId)
            .ToListAsync(cancellationToken);

        var linkedIds = links.SelectMany(l => new[] { l.ParentCase, l.ChildCase }).Distinct().ToList();

        var cases = await _db.Cases
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Where(c => linkedIds.Contains(c.CaseId) && c.DateDeleted == null)
            .Select(c => new
            {
                c.CaseId,
                c.AuthNumber,
                Status = c.Status != null ? c.Status.CaseStatus1 : null,
                MemberName = c.Member != null ? (c.Member.Surname + ", " + c.Member.Name) : null,
                c.AdmissionDate,
                c.DischargeDate
            }).ToListAsync(cancellationToken);

        var headers = new[] { "Case ID", "Auth Number", "Status", "Member", "Admission", "Discharge" };
        var rows = cases.Select(c => new object?[] { c.CaseId, c.AuthNumber, c.Status, c.MemberName, c.AdmissionDate?.ToString("yyyy/MM/dd"), c.DischargeDate?.ToString("yyyy/MM/dd") }).ToList();

        if (request.Format == "pdf")
            return await GeneratePdfTable($"Linked Cases — Case {request.CaseId}", null, headers, rows);

        return GenerateExcel($"Linked Cases {request.CaseId}", headers, rows);
    }

    // ═══════════════════════════════════════════════════════════════════
    // EXCEL GENERATION (ClosedXML)
    // ═══════════════════════════════════════════════════════════════════
    private ReportOutput GenerateExcel(string title, string[] headers, List<object?[]> rows)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add(title.Length > 31 ? title[..31] : title);

        // Headers
        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i + 1).Value = headers[i];
            ws.Cell(1, i + 1).Style.Font.Bold = true;
            ws.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#f5e6a3");
        }

        // Data
        for (int r = 0; r < rows.Count; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                var val = rows[r][c];
                if (val is decimal d)
                    ws.Cell(r + 2, c + 1).Value = (double)d;
                else if (val is int intVal)
                    ws.Cell(r + 2, c + 1).Value = intVal;
                else
                    ws.Cell(r + 2, c + 1).Value = val?.ToString() ?? "";
            }
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);

        return new ReportOutput
        {
            Content = ms.ToArray(),
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileName = $"{title.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.xlsx"
        };
    }

    // ═══════════════════════════════════════════════════════════════════
    // PDF GENERATION (HTML → PuppeteerSharp)
    // ═══════════════════════════════════════════════════════════════════
    private async Task<ReportOutput> GeneratePdfTable(string title, string? subtitle, string[] headers, List<object?[]> rows)
    {
        var html = $@"<!DOCTYPE html>
<html><head><meta charset='utf-8'><style>
body {{ font-family: Arial, sans-serif; font-size: 8pt; padding: 15mm; }}
h1 {{ font-size: 14pt; margin-bottom: 4px; }}
h2 {{ font-size: 10pt; color: #666; margin-bottom: 12px; }}
table {{ width: 100%; border-collapse: collapse; }}
th {{ background: #f5e6a3; border: 1px solid #c9a800; padding: 4px 6px; text-align: left; font-size: 7.5pt; }}
td {{ border: 1px solid #ddd; padding: 3px 6px; font-size: 7.5pt; }}
tr:nth-child(even) {{ background: #fafafa; }}
.footer {{ margin-top: 12px; font-size: 7pt; color: #666; text-align: center; }}
</style></head><body>
<h1>{title}</h1>
{(subtitle != null ? $"<h2>{subtitle}</h2>" : "")}
<table><thead><tr>{string.Join("", headers.Select(h => $"<th>{h}</th>"))}</tr></thead><tbody>
{string.Join("", rows.Select(r => "<tr>" + string.Join("", r.Select(v => $"<td>{v ?? ""}</td>")) + "</tr>"))}
</tbody></table>
<div class='footer'>Generated: {DateTime.Now:yyyy/MM/dd HH:mm} | {rows.Count} record(s)</div>
</body></html>";

        var pdf = await HtmlToPdfAsync(html);
        return new ReportOutput
        {
            Content = pdf,
            ContentType = "application/pdf",
            FileName = $"{title.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.pdf"
        };
    }

    private static IBrowser? _browser;
    private static readonly SemaphoreSlim _browserLock = new(1, 1);

    private static async Task<IBrowser> GetBrowserAsync()
    {
        if (_browser != null && _browser.IsConnected)
            return _browser;

        await _browserLock.WaitAsync();
        try
        {
            if (_browser != null && _browser.IsConnected)
                return _browser;

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--single-process"
                }
            });
            return _browser;
        }
        finally
        {
            _browserLock.Release();
        }
    }

    private static async Task<byte[]> HtmlToPdfAsync(string html)
    {
        var browser = await GetBrowserAsync();
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html, new NavigationOptions
        {
            WaitUntil = [WaitUntilNavigation.Load],
            Timeout = 60000
        });
        var pdf = await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            Landscape = true,
            PrintBackground = true,
            MarginOptions = new MarginOptions { Top = "10mm", Bottom = "10mm", Left = "10mm", Right = "10mm" }
        });
        return pdf;
    }
}
