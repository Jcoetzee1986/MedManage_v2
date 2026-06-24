namespace MedManage.Core.DTOs.Report;

/// <summary>
/// Result from a report generation request
/// </summary>
public class ReportResult
{
    /// <summary>
    /// The generated report content as bytes
    /// </summary>
    public byte[] Content { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// MIME content type of the generated report (e.g., application/pdf, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet)
    /// </summary>
    public string ContentType { get; set; } = "application/pdf";

    /// <summary>
    /// Suggested file name for the report download
    /// </summary>
    public string FileName { get; set; } = "report.pdf";
}

/// <summary>
/// Supported report output formats
/// </summary>
public enum ReportFormat
{
    Pdf,
    Excel
}

/// <summary>
/// Request to generate a case letter report
/// </summary>
public class CaseLetterReportRequest
{
    /// <summary>
    /// The case ID to generate the letter for
    /// </summary>
    public int CaseId { get; set; }

    /// <summary>
    /// Output format (PDF or Excel)
    /// </summary>
    public ReportFormat Format { get; set; } = ReportFormat.Pdf;
}

/// <summary>
/// Request to generate a cases-between-dates report
/// </summary>
public class CasesBetweenDatesReportRequest
{
    /// <summary>
    /// Start date of the reporting period
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// End date of the reporting period
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Optional filter by service provider ID
    /// </summary>
    public int? ServiceProviderId { get; set; }

    /// <summary>
    /// Optional filter by case status ID
    /// </summary>
    public int? CaseStatusId { get; set; }

    /// <summary>
    /// Output format (PDF or Excel)
    /// </summary>
    public ReportFormat Format { get; set; } = ReportFormat.Pdf;
}

/// <summary>
/// Request to generate a WIP (Work In Progress) extract report
/// </summary>
public class WipExtractReportRequest
{
    /// <summary>
    /// Optional filter by service provider ID
    /// </summary>
    public int? ServiceProviderId { get; set; }

    /// <summary>
    /// Optional filter by main client ID
    /// </summary>
    public int? MainClientId { get; set; }

    /// <summary>
    /// As-at date for the WIP snapshot
    /// </summary>
    public DateTime? AsAtDate { get; set; }

    /// <summary>
    /// Output format (PDF or Excel)
    /// </summary>
    public ReportFormat Format { get; set; } = ReportFormat.Excel;
}

/// <summary>
/// Request to generate a billing summary report
/// </summary>
public class BillingSummaryReportRequest
{
    /// <summary>
    /// Start date of the billing period
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// End date of the billing period
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Optional filter by service provider ID
    /// </summary>
    public int? ServiceProviderId { get; set; }

    /// <summary>
    /// Optional filter by billing status ID
    /// </summary>
    public int? BillingStatusId { get; set; }

    /// <summary>
    /// Output format (PDF or Excel)
    /// </summary>
    public ReportFormat Format { get; set; } = ReportFormat.Excel;
}
