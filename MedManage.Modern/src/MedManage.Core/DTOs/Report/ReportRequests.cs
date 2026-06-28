using System;

namespace MedManage.Core.DTOs.Report;

public class CasesBetweenDatesRequest
{
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public int? MainClientId { get; set; }
    public int? StatusId { get; set; }
    public int? CaseTypeId { get; set; }
    public string Format { get; set; } = "excel"; // excel or pdf
}

public class BillingSummaryRequest
{
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
    public int? MainClientId { get; set; }
    public int? ServiceProviderId { get; set; }
    public string Format { get; set; } = "excel";
}

public class CaseTariffDetailRequest
{
    public int CaseId { get; set; }
    public string Format { get; set; } = "pdf";
}

public class MyCasesRequest
{
    public int? StatusId { get; set; }
    public int? MainClientId { get; set; }
    public string Format { get; set; } = "excel";
}

public class CaseCommentsExportRequest
{
    public int CaseId { get; set; }
    public string Format { get; set; } = "excel";
}

public class WipExtractRequest
{
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
    public int? MainClientId { get; set; }
    public string Format { get; set; } = "excel";
}

public class LinkedCasesRequest
{
    public int CaseId { get; set; }
    public string Format { get; set; } = "pdf";
}

public class ReportOutput
{
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = "report";
}
