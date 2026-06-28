using System;

namespace MedManage.Core.DTOs.CaseTariff;

public class CaseTariffDto
{
    public long CaseIdTariffId { get; set; }
    public int CaseId { get; set; }
    public int TariffId { get; set; }
    public string? TariffCode { get; set; }
    public string? TariffDescription { get; set; }
    public decimal? Value { get; set; }
    public decimal? Qty { get; set; }
    public decimal? AgreedRateOverride { get; set; }
    public bool? ValueIsTotal { get; set; }
    public bool? Rejected { get; set; }
    public DateOnly DateOfProcedure { get; set; }
    // Calculated by the stored procedure
    public decimal? FullValue { get; set; }
    public decimal? AgreedRate { get; set; }
    public decimal? Discount { get; set; }
    public decimal? TotalOvercharged { get; set; }
    public decimal? TotalPayable { get; set; }
    public decimal? TotalPenalty { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    public string? Colour { get; set; }
    // Audit
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

public class CreateCaseTariffRequest
{
    public int TariffId { get; set; }
    public decimal? Value { get; set; }
    public decimal? Qty { get; set; }
    public decimal? AgreedRateOverride { get; set; }
    public bool? ValueIsTotal { get; set; }
    public bool? Rejected { get; set; }
    public DateOnly DateOfProcedure { get; set; }
}

public class UpdateCaseTariffRequest
{
    public decimal? Value { get; set; }
    public decimal? Qty { get; set; }
    public decimal? AgreedRateOverride { get; set; }
    public bool? ValueIsTotal { get; set; }
    public bool? Rejected { get; set; }
    public DateOnly DateOfProcedure { get; set; }
}
