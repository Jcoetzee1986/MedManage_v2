using System.Globalization;
using MedManage.Core.DTOs.Import;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service for handling data file imports (DRD members, NAPPI codes, billing)
/// </summary>
public class ImportService : IImportService
{
    private readonly MedManageDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<ImportService> _logger;

    public ImportService(
        MedManageDbContext dbContext,
        ICurrentUserService currentUserService,
        ILogger<ImportService> logger)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<ImportResultDto> ImportMembersAsync(Stream fileStream, string fileName)
    {
        var result = new ImportResultDto
        {
            ImportType = "Members",
            FileName = fileName,
            ImportDate = DateTime.UtcNow,
            ImportedBy = _currentUserService.UserId
        };

        try
        {
            var lines = await ReadCsvLinesAsync(fileStream);
            if (lines.Count < 2)
            {
                result.Status = "Failed";
                result.ErrorDetails = "File is empty or contains only headers";
                await SaveImportHistory(result);
                return result;
            }

            var headers = ParseCsvLine(lines[0]);
            result.TotalRecords = lines.Count - 1;

            for (int i = 1; i < lines.Count; i++)
            {
                try
                {
                    var fields = ParseCsvLine(lines[i]);
                    if (fields.Length < 3)
                    {
                        result.SkippedRecords++;
                        result.ValidationErrors.Add(new ImportValidationError
                        {
                            Row = i + 1,
                            Field = "Row",
                            Message = "Insufficient columns"
                        });
                        continue;
                    }

                    var memberNumber = GetFieldValue(headers, fields, "MemberNumber", "MemberNo", "Member_Number");
                    var surname = GetFieldValue(headers, fields, "Surname", "LastName", "Last_Name");
                    var name = GetFieldValue(headers, fields, "Name", "FirstName", "First_Name");

                    if (string.IsNullOrWhiteSpace(memberNumber))
                    {
                        result.ErrorRecords++;
                        result.ValidationErrors.Add(new ImportValidationError
                        {
                            Row = i + 1,
                            Field = "MemberNumber",
                            Message = "Member number is required"
                        });
                        continue;
                    }

                    // Check if member already exists - merge by member number
                    var existing = await _dbContext.Members
                        .FirstOrDefaultAsync(m => m.MemberNumber == memberNumber);

                    if (existing != null)
                    {
                        // Update existing member
                        if (!string.IsNullOrWhiteSpace(surname)) existing.Surname = surname;
                        if (!string.IsNullOrWhiteSpace(name)) existing.Name = name;

                        var initials = GetFieldValue(headers, fields, "Initials");
                        if (!string.IsNullOrWhiteSpace(initials)) existing.Initials = initials;

                        var idNumber = GetFieldValue(headers, fields, "IDNumber", "ID_Number", "IdNo");
                        if (!string.IsNullOrWhiteSpace(idNumber)) existing.Idnumber = idNumber;

                        var dob = GetFieldValue(headers, fields, "DateOfBirth", "DOB", "Date_Of_Birth");
                        if (!string.IsNullOrWhiteSpace(dob) && DateOnly.TryParse(dob, out var dobDate))
                            existing.DateOfBirth = dobDate;

                        result.ImportedRecords++;
                    }
                    else
                    {
                        // Create new member
                        var member = new Member
                        {
                            MemberNumber = memberNumber,
                            Surname = surname,
                            Name = name,
                            Initials = GetFieldValue(headers, fields, "Initials"),
                            Idnumber = GetFieldValue(headers, fields, "IDNumber", "ID_Number", "IdNo")
                        };

                        var dob = GetFieldValue(headers, fields, "DateOfBirth", "DOB", "Date_Of_Birth");
                        if (!string.IsNullOrWhiteSpace(dob) && DateOnly.TryParse(dob, out var dobDate))
                            member.DateOfBirth = dobDate;

                        _dbContext.Members.Add(member);
                        result.ImportedRecords++;
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorRecords++;
                    result.ValidationErrors.Add(new ImportValidationError
                    {
                        Row = i + 1,
                        Field = "Row",
                        Message = $"Error processing row: {ex.Message}"
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            result.Status = result.ErrorRecords == 0 ? "Completed" : "CompletedWithErrors";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing members file {FileName}", fileName);
            result.Status = "Failed";
            result.ErrorDetails = ex.Message;
        }

        await SaveImportHistory(result);
        return result;
    }

    public async Task<ImportResultDto> ImportNappiCodesAsync(Stream fileStream, string fileName)
    {
        var result = new ImportResultDto
        {
            ImportType = "NappiCodes",
            FileName = fileName,
            ImportDate = DateTime.UtcNow,
            ImportedBy = _currentUserService.UserId
        };

        try
        {
            var lines = await ReadCsvLinesAsync(fileStream);
            if (lines.Count < 2)
            {
                result.Status = "Failed";
                result.ErrorDetails = "File is empty or contains only headers";
                await SaveImportHistory(result);
                return result;
            }

            var headers = ParseCsvLine(lines[0]);
            result.TotalRecords = lines.Count - 1;

            for (int i = 1; i < lines.Count; i++)
            {
                try
                {
                    var fields = ParseCsvLine(lines[i]);
                    if (fields.Length < 2)
                    {
                        result.SkippedRecords++;
                        continue;
                    }

                    var nappiCode = GetFieldValue(headers, fields, "NappiCode", "Code", "NAPPI_Code");
                    var description = GetFieldValue(headers, fields, "Description", "Desc", "Name");

                    if (string.IsNullOrWhiteSpace(nappiCode))
                    {
                        result.ErrorRecords++;
                        result.ValidationErrors.Add(new ImportValidationError
                        {
                            Row = i + 1,
                            Field = "NappiCode",
                            Message = "NAPPI code is required"
                        });
                        continue;
                    }

                    // Check if code already exists - update if so
                    var existing = await _dbContext.NappiCodes
                        .FirstOrDefaultAsync(n => n.Code == nappiCode);

                    if (existing != null)
                    {
                        if (!string.IsNullOrWhiteSpace(description)) existing.Description = description;

                        var priceStr = GetFieldValue(headers, fields, "Price", "Price1", "SEP");
                        if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                            existing.Price1 = price;

                        var unitsStr = GetFieldValue(headers, fields, "Units", "PackSize");
                        if (!string.IsNullOrWhiteSpace(unitsStr) && int.TryParse(unitsStr, out var units))
                            existing.Units = units;

                        var measure = GetFieldValue(headers, fields, "Measure", "UOM");
                        if (!string.IsNullOrWhiteSpace(measure)) existing.Measure = measure;

                        result.ImportedRecords++;
                    }
                    else
                    {
                        var nappi = new NappiCode
                        {
                            Code = nappiCode,
                            Description = description
                        };

                        var priceStr = GetFieldValue(headers, fields, "Price", "Price1", "SEP");
                        if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                            nappi.Price1 = price;

                        var unitsStr = GetFieldValue(headers, fields, "Units", "PackSize");
                        if (!string.IsNullOrWhiteSpace(unitsStr) && int.TryParse(unitsStr, out var units))
                            nappi.Units = units;

                        nappi.Measure = GetFieldValue(headers, fields, "Measure", "UOM");

                        _dbContext.NappiCodes.Add(nappi);
                        result.ImportedRecords++;
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorRecords++;
                    result.ValidationErrors.Add(new ImportValidationError
                    {
                        Row = i + 1,
                        Field = "Row",
                        Message = $"Error processing row: {ex.Message}"
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            result.Status = result.ErrorRecords == 0 ? "Completed" : "CompletedWithErrors";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing NAPPI codes file {FileName}", fileName);
            result.Status = "Failed";
            result.ErrorDetails = ex.Message;
        }

        await SaveImportHistory(result);
        return result;
    }

    public async Task<ImportResultDto> ImportBillingAsync(Stream fileStream, string fileName)
    {
        var result = new ImportResultDto
        {
            ImportType = "Billing",
            FileName = fileName,
            ImportDate = DateTime.UtcNow,
            ImportedBy = _currentUserService.UserId
        };

        try
        {
            var lines = await ReadCsvLinesAsync(fileStream);
            if (lines.Count < 2)
            {
                result.Status = "Failed";
                result.ErrorDetails = "File is empty or contains only headers";
                await SaveImportHistory(result);
                return result;
            }

            var headers = ParseCsvLine(lines[0]);
            result.TotalRecords = lines.Count - 1;

            for (int i = 1; i < lines.Count; i++)
            {
                try
                {
                    var fields = ParseCsvLine(lines[i]);
                    if (fields.Length < 3)
                    {
                        result.SkippedRecords++;
                        continue;
                    }

                    var accountNumber = GetFieldValue(headers, fields, "AccountNumber", "Account_Number", "AccNo");
                    var caseIdStr = GetFieldValue(headers, fields, "CaseID", "Case_ID", "CaseId");

                    if (string.IsNullOrWhiteSpace(accountNumber) && string.IsNullOrWhiteSpace(caseIdStr))
                    {
                        result.ErrorRecords++;
                        result.ValidationErrors.Add(new ImportValidationError
                        {
                            Row = i + 1,
                            Field = "AccountNumber/CaseID",
                            Message = "Account number or Case ID is required"
                        });
                        continue;
                    }

                    var billing = new CaseBilling
                    {
                        AccountNumber = accountNumber
                    };

                    if (!string.IsNullOrWhiteSpace(caseIdStr) && int.TryParse(caseIdStr, out var caseId))
                        billing.CaseId = caseId;

                    var invoiceNumber = GetFieldValue(headers, fields, "InvoiceNumber", "Invoice_Number", "InvNo");
                    if (!string.IsNullOrWhiteSpace(invoiceNumber)) billing.InvoiceNumber = invoiceNumber;

                    var amountDueStr = GetFieldValue(headers, fields, "AmountDue", "Amount_Due", "Amount");
                    if (!string.IsNullOrWhiteSpace(amountDueStr) && decimal.TryParse(amountDueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var amountDue))
                        billing.AmountDue = amountDue;

                    var accountDateStr = GetFieldValue(headers, fields, "AccountDate", "Account_Date", "Date");
                    if (!string.IsNullOrWhiteSpace(accountDateStr) && DateOnly.TryParse(accountDateStr, out var accountDate))
                        billing.AccountDate = accountDate;

                    var patientName = GetFieldValue(headers, fields, "PatientName", "Patient_Name");
                    if (!string.IsNullOrWhiteSpace(patientName)) billing.PatientName = patientName;

                    var patientSurname = GetFieldValue(headers, fields, "PatientSurname", "Patient_Surname");
                    if (!string.IsNullOrWhiteSpace(patientSurname)) billing.PatientSurname = patientSurname;

                    var providerIdStr = GetFieldValue(headers, fields, "ServiceProviderID", "Provider_ID", "ProviderId");
                    if (!string.IsNullOrWhiteSpace(providerIdStr) && int.TryParse(providerIdStr, out var providerId))
                        billing.ServiceProviderId = providerId;

                    _dbContext.CaseBillings.Add(billing);
                    result.ImportedRecords++;
                }
                catch (Exception ex)
                {
                    result.ErrorRecords++;
                    result.ValidationErrors.Add(new ImportValidationError
                    {
                        Row = i + 1,
                        Field = "Row",
                        Message = $"Error processing row: {ex.Message}"
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            result.Status = result.ErrorRecords == 0 ? "Completed" : "CompletedWithErrors";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing billing file {FileName}", fileName);
            result.Status = "Failed";
            result.ErrorDetails = ex.Message;
        }

        await SaveImportHistory(result);
        return result;
    }

    public async Task<IEnumerable<ImportHistoryDto>> GetImportHistoryAsync(string? importType = null, int? limit = 50)
    {
        var query = _dbContext.Set<ImportHistory>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(importType))
            query = query.Where(h => h.ImportType == importType);

        var history = await query
            .OrderByDescending(h => h.ImportDate)
            .Take(limit ?? 50)
            .Select(h => new ImportHistoryDto
            {
                ImportHistoryId = h.ImportHistoryId,
                ImportType = h.ImportType,
                FileName = h.FileName,
                ImportDate = h.ImportDate,
                ImportedBy = h.ImportedBy,
                TotalRecords = h.TotalRecords,
                ImportedRecords = h.ImportedRecords,
                SkippedRecords = h.SkippedRecords,
                ErrorRecords = h.ErrorRecords,
                Status = h.Status
            })
            .ToListAsync();

        return history;
    }

    // ─── Private Helpers ─────────────────────────────────────

    private async Task SaveImportHistory(ImportResultDto result)
    {
        var history = new ImportHistory
        {
            ImportType = result.ImportType,
            FileName = result.FileName,
            ImportDate = result.ImportDate,
            ImportedBy = result.ImportedBy,
            TotalRecords = result.TotalRecords,
            ImportedRecords = result.ImportedRecords,
            SkippedRecords = result.SkippedRecords,
            ErrorRecords = result.ErrorRecords,
            Status = result.Status,
            ErrorDetails = result.ErrorDetails
        };

        _dbContext.Set<ImportHistory>().Add(history);
        await _dbContext.SaveChangesAsync();
        result.ImportHistoryId = history.ImportHistoryId;
    }

    private static async Task<List<string>> ReadCsvLinesAsync(Stream stream)
    {
        var lines = new List<string>();
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync() is { } line)
        {
            if (!string.IsNullOrWhiteSpace(line))
                lines.Add(line);
        }
        return lines;
    }

    private static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var inQuotes = false;
        var field = new System.Text.StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if ((c == ',' || c == ';' || c == '|') && !inQuotes)
            {
                fields.Add(field.ToString().Trim());
                field.Clear();
            }
            else
            {
                field.Append(c);
            }
        }
        fields.Add(field.ToString().Trim());

        return fields.ToArray();
    }

    /// <summary>
    /// Gets a field value from the CSV row by trying multiple possible header names (case-insensitive)
    /// </summary>
    private static string? GetFieldValue(string[] headers, string[] fields, params string[] possibleNames)
    {
        foreach (var name in possibleNames)
        {
            var index = Array.FindIndex(headers, h =>
                string.Equals(h.Trim(), name, StringComparison.OrdinalIgnoreCase));
            if (index >= 0 && index < fields.Length)
            {
                var value = fields[index].Trim().Trim('"');
                return string.IsNullOrWhiteSpace(value) ? null : value;
            }
        }
        return null;
    }
}
