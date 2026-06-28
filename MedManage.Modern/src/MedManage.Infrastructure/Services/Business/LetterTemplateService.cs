using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MedManage.Infrastructure.Services.Business;

public class LetterTemplateService : ILetterTemplateService
{
    private readonly MedManageDbContext _dbContext;
    private readonly ILogger<LetterTemplateService> _logger;
    private readonly ITariffPercentageService _tariffPercentageService;
    private static IBrowser? _browser;
    private static readonly SemaphoreSlim _browserLock = new(1, 1);

    public LetterTemplateService(MedManageDbContext dbContext, ILogger<LetterTemplateService> logger, ITariffPercentageService tariffPercentageService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _tariffPercentageService = tariffPercentageService;
    }

    public async Task<IEnumerable<LetterTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LetterTemplate>()
            .Where(t => t.DateDeleted == null)
            .OrderBy(t => t.TemplateName)
            .ToListAsync(cancellationToken);
    }

    public async Task<LetterTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LetterTemplate>()
            .FirstOrDefaultAsync(t => t.LetterTemplateId == id && t.DateDeleted == null, cancellationToken);
    }

    public async Task<LetterTemplate?> GetForCaseAsync(int caseId, string templateType = "CaseLetter", CancellationToken cancellationToken = default)
    {
        // Get the case's medical aid → main client to determine the template
        var caseInfo = await _dbContext.Cases
            .Where(c => c.CaseId == caseId)
            .Select(c => new {
                MainClientId = c.Member != null && c.Member.MedicalAid != null ? c.Member.MedicalAid.MainClientId : (int?)null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (caseInfo?.MainClientId != null)
        {
            // Try client-specific template first
            var clientTemplate = await _dbContext.Set<LetterTemplate>()
                .FirstOrDefaultAsync(t => t.MainClientId == caseInfo.MainClientId
                    && t.TemplateType == templateType
                    && t.IsActive && t.DateDeleted == null, cancellationToken);

            if (clientTemplate != null) return clientTemplate;
        }

        // Fall back to default template
        return await _dbContext.Set<LetterTemplate>()
            .FirstOrDefaultAsync(t => t.IsDefault && t.TemplateType == templateType
                && t.IsActive && t.DateDeleted == null, cancellationToken);
    }

    public async Task<LetterTemplate> CreateAsync(LetterTemplate template, CancellationToken cancellationToken = default)
    {
        template.DateInserted = DateTime.Now;
        _dbContext.Set<LetterTemplate>().Add(template);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task<LetterTemplate> UpdateAsync(LetterTemplate template, CancellationToken cancellationToken = default)
    {
        template.DateUpdated = DateTime.Now;
        _dbContext.Set<LetterTemplate>().Update(template);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await GetByIdAsync(id, cancellationToken);
        if (template == null) return false;
        template.DateDeleted = DateTime.Now;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<byte[]> RenderCaseLetterAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var template = await GetForCaseAsync(caseId, "CaseLetter", cancellationToken);
        if (template == null)
            throw new InvalidOperationException("No letter template found for this case. Please configure a template in Admin.");

        // Gather all data from existing SPs
        var data = await GatherCaseLetterDataAsync(caseId, cancellationToken);

        // Render HTML with Handlebars
        var handlebars = Handlebars.Create();
        var compiledTemplate = handlebars.Compile(template.HtmlContent ?? "");
        var html = compiledTemplate(data);

        // Convert HTML to PDF using Puppeteer
        var pdf = await HtmlToPdfAsync(html);
        return pdf;
    }

    private async Task<Dictionary<string, object?>> GatherCaseLetterDataAsync(int caseId, CancellationToken cancellationToken)
    {
        var data = new Dictionary<string, object?>();

        // 1. Case details with joins
        var caseInfo = await _dbContext.Cases
            .Where(c => c.CaseId == caseId && c.DateDeleted == null)
            .Select(c => new
            {
                c.CaseId,
                c.AuthNumber,
                c.AccountNr,
                c.AdmissionDate,
                c.DischargeDate,
                c.TotalLengthOfStay,
                CaseStatus = c.Status != null ? c.Status.CaseStatus1 : null,
                MemberNumber = c.Member != null ? c.Member.MemberNumber : null,
                Name = c.Member != null ? c.Member.Name : null,
                Surname = c.Member != null ? c.Member.Surname : null,
                DateOfBirth = c.Member != null ? c.Member.DateOfBirth : null,
                MedicalAidName = c.Member != null && c.Member.MedicalAid != null ? c.Member.MedicalAid.MedicalAidName : null,
                MedAidProductName = (string?)null, // would need join to product table
                CurrentPractice = c.ReferTo != null ? c.ReferTo.PracticeName : null,
                ReferringProvider = c.ReferFrom != null
                    ? (c.ReferFrom.PracticeNr ?? "") + " " + (c.ReferFrom.PracticeName ?? "")
                    : null,
                Speciality = c.ReferTo != null && c.ReferTo.SpecialityId != null
                    ? _dbContext.Specialities.Where(s => s.SpecialityId == c.ReferTo.SpecialityId).Select(s => s.Speciality1).FirstOrDefault()
                    : null,
                MainClientId = c.Member != null && c.Member.MedicalAid != null ? c.Member.MedicalAid.MainClientId : (int?)null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (caseInfo == null) throw new InvalidOperationException("Case not found");

        data["CaseId"] = caseInfo.CaseId;
        data["AuthNumber"] = caseInfo.AuthNumber;
        data["AccountNr"] = caseInfo.AccountNr;
        data["AdmissionDate"] = caseInfo.AdmissionDate?.ToString("yyyy/MM/dd");
        data["DischargeDate"] = caseInfo.DischargeDate?.ToString("yyyy/MM/dd");
        data["TotalLOS"] = caseInfo.TotalLengthOfStay ?? 0;
        data["CaseStatus"] = caseInfo.CaseStatus;
        data["IsClosed"] = caseInfo.CaseStatus?.ToLower().Contains("closed") == true;
        data["MemberNumber"] = caseInfo.MemberNumber;
        data["Name"] = caseInfo.Name;
        data["Surname"] = caseInfo.Surname;
        data["DateOfBirth"] = caseInfo.DateOfBirth?.ToString("yyyy/MM/dd");
        data["MedicalAidName"] = caseInfo.MedicalAidName;
        data["MedAidProductName"] = caseInfo.MedAidProductName;
        data["HasMedicalScheme"] = !string.IsNullOrWhiteSpace(caseInfo.MedAidProductName)
            && caseInfo.MedAidProductName?.Trim() != "None"
            && caseInfo.MedAidProductName?.Trim() != "";

        // Look up member's active medical aid product
        if (caseInfo.MainClientId != null)
        {
            var memberId = caseInfo.MemberNumber != null ? await _dbContext.Cases
                .Where(c => c.CaseId == caseId).Select(c => c.MemberId).FirstOrDefaultAsync(cancellationToken) : null;

            if (memberId.HasValue)
            {
                var productName = await _dbContext.Database
                    .SqlQueryRaw<ProductNameRow>(
                        "SELECT TOP 1 mp.MedAidProductName as Name FROM shared.Member_MedicalAidProduct mmp INNER JOIN shared.MedicalAidProduct mp ON mp.MedAidProductID = mmp.MedAidProductID WHERE mmp.MemberID = {0} AND mmp.DateDeleted IS NULL AND (mmp.EndDate IS NULL OR mmp.EndDate >= GETDATE()) ORDER BY mmp.StartDate DESC",
                        memberId.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                if (productName?.Name != null && productName.Name.Trim() != "None")
                {
                    data["MedAidProductName"] = productName.Name;
                    data["HasMedicalScheme"] = true;
                }
            }
        }
        data["CurrentPractice"] = caseInfo.CurrentPractice;
        data["ReferringProvider"] = caseInfo.ReferringProvider;
        data["Speciality"] = caseInfo.Speciality;

        // Get system data (address, logo, contact info)
        var systemData = await _dbContext.Database
            .SqlQueryRaw<SystemDataRow>("SELECT TOP 1 Address1, Address2, Address3, Address4, AddressCode, Email, Fax, Telephone, Website, Logo FROM shared.SystemData")
            .FirstOrDefaultAsync(cancellationToken);

        if (systemData != null)
        {
            data["Address1"] = systemData.Address1;
            data["Address2"] = systemData.Address2;
            data["Address3"] = systemData.Address3;
            data["Address4"] = systemData.Address4;
            data["AddressCode"] = systemData.AddressCode;
            data["Email"] = systemData.Email;
            data["Fax"] = systemData.Fax;
            data["Telephone"] = systemData.Telephone;
            data["Website"] = systemData.Website;
            if (systemData.Logo != null && systemData.Logo.Length > 0)
                data["SystemLogoBase64"] = Convert.ToBase64String(systemData.Logo);
        }

        // Get main client logo
        if (caseInfo.MainClientId.HasValue)
        {
            var mcLogo = await _dbContext.Database
                .SqlQueryRaw<LogoRow>("SELECT MainClientLogo as Logo FROM shared.MainClient WHERE MainClientID = {0}", caseInfo.MainClientId.Value)
                .FirstOrDefaultAsync(cancellationToken);
            if (mcLogo?.Logo != null && mcLogo.Logo.Length > 0)
                data["MainClientLogoBase64"] = Convert.ToBase64String(mcLogo.Logo);
        }

        // 2. ICD codes
        var icdCodes = await _dbContext.CaseIcds
            .Where(ci => ci.CaseId == caseId && ci.DateDeleted == null)
            .Join(_dbContext.Icds, ci => ci.Icdid, i => i.Icdid, (ci, i) => new { i.DiagnosisCode, i.DiagnosisDesc })
            .ToListAsync(cancellationToken);
        data["IcdCodes"] = icdCodes;

        // 3. CPT codes
        var cptCodes = await _dbContext.CaseCpts
            .Where(cp => cp.CaseId == caseId && cp.DateDeleted == null)
            .Join(_dbContext.Cpts, cp => cp.Cptid, c => c.Cptid, (cp, c) => new { CODE = c.Code, LONG_DESCR = c.ShortDescr, CodeType = cp.PrimaryCode == true ? "Primary" : "Secondary" })
            .ToListAsync(cancellationToken);
        data["CptCodes"] = cptCodes;

        // 4. Tariffs (uses SP for calculated rates)
        var tariffs = new List<Dictionary<string, object?>>();
        var connection = _dbContext.Database.GetDbConnection();
        try
        {
            await connection.OpenAsync(cancellationToken);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Tariff.usp_Case_Tariff_Select";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CaseID", caseId));
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
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
        data["Tariffs"] = tariffs;

        // 5. Facility Types
        var facilityTypes = await _dbContext.CaseFacilityTypes
            .Where(ft => ft.CaseId == caseId && ft.DateDeleted == null)
            .Select(ft => new { FacilityType = ft.FacilityType != null ? ft.FacilityType.FacilityType1 : null, ft.DateAdmitted, ft.DateDischarged, ft.Los })
            .ToListAsync(cancellationToken);
        data["FacilityTypes"] = facilityTypes.Select(ft => new Dictionary<string, object?>
        {
            ["FacilityType"] = ft.FacilityType,
            ["DateAdmitted"] = ft.DateAdmitted.ToString("yyyy/MM/dd"),
            ["DateDischarged"] = ft.DateDischarged?.ToString("yyyy/MM/dd") ?? "",
            ["Los"] = ft.Los
        }).ToList();

        // Override TotalLOS with sum from facility types if available
        var computedLos = facilityTypes.Sum(ft => ft.Los ?? 0);
        if (computedLos > 0)
        {
            data["TotalLOS"] = computedLos;
        }

        // 6. Last case note (for letter note)
        var letterNote = await _dbContext.CaseLetterNotes
            .Where(ln => ln.CaseId == caseId && ln.DateDeleted == null)
            .FirstOrDefaultAsync(cancellationToken);
        if (letterNote != null)
        {
            data["CaseNote"] = letterNote.Note;
            data["IncludeDischargeForm"] = letterNote.IncludeDischargeForm ?? false;
            data["IncludeReferralLetter"] = letterNote.IncludeReferralLetter ?? false;
        }

        data["GeneratedDate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

        // Load the referral letter page image (stored as a file)
        var referralImagePath = Path.Combine(AppContext.BaseDirectory, "Templates", "referral_letter_page2.b64");
        if (File.Exists(referralImagePath))
        {
            data["ReferralLetterImage"] = File.ReadAllText(referralImagePath).Trim();
        }

        // 7. Tariff Percentage fields for letter templates
        var tariffPercentages = (await _tariffPercentageService.GetActivePercentagesForLetterAsync(cancellationToken)).ToList();

        if (tariffPercentages.Count > 0)
        {
            // First record is the current year (ordered by TariffPeriodName descending)
            data["TariffPercentageCurrentYear"] = tariffPercentages[0].TariffPeriodName.ToString();
            data["TariffPercentageCurrentYearValue"] = tariffPercentages[0].PercentageAdded.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

            if (tariffPercentages.Count > 1)
            {
                // Second record is the prior year
                data["TariffPercentagePriorYear"] = tariffPercentages[1].TariffPeriodName.ToString();
                data["TariffPercentagePriorYearValue"] = tariffPercentages[1].PercentageAdded.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                data["TariffPercentagePriorYear"] = "";
                data["TariffPercentagePriorYearValue"] = "";
            }
        }
        else
        {
            // No completed records — set all fields to empty string to avoid Handlebars rendering errors
            data["TariffPercentageCurrentYear"] = "";
            data["TariffPercentageCurrentYearValue"] = "";
            data["TariffPercentagePriorYear"] = "";
            data["TariffPercentagePriorYearValue"] = "";
        }

        return data;
    }

    // Helper classes for raw SQL projections
    private class SystemDataRow
    {
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public string? AddressCode { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Telephone { get; set; }
        public string? Website { get; set; }
        public byte[]? Logo { get; set; }
    }

    private class LogoRow
    {
        public byte[]? Logo { get; set; }
    }

    private class ProductNameRow
    {
        public string? Name { get; set; }
    }

    private async Task<byte[]> HtmlToPdfAsync(string html)
    {
        var browser = await GetBrowserAsync();
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Load] });

        // Set document title for PDF metadata
        await page.EvaluateExpressionAsync("document.title = 'Case Letter'");

        var pdf = await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true,
            DisplayHeaderFooter = false,
            MarginOptions = new MarginOptions
            {
                Top = "10mm",
                Bottom = "10mm",
                Left = "10mm",
                Right = "10mm"
            }
        });

        return pdf;
    }

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
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
            });

            return _browser;
        }
        finally
        {
            _browserLock.Release();
        }
    }
}
