using System;

namespace MedManage.Core.DTOs.Tariff;

// --- Tariff Lookup DTOs ---

public class TariffLookupRequest
{
    public string TariffCode { get; set; } = null!;
    public int ProviderId { get; set; }
    public DateOnly TreatmentDate { get; set; }
}

public class TariffLookupResult
{
    public string? BaseTariffId { get; set; }
    public string? TariffCode { get; set; }
    public string? TariffDescription { get; set; }
    public int? SpecialityId { get; set; }
    public string? Speciality { get; set; }
    public decimal? TariffAmount { get; set; }
    public int? TariffId { get; set; }
    public decimal? Qty { get; set; }
    public string? Metric { get; set; }
    public decimal? Quantity { get; set; }
    public string? TariffPeriodName { get; set; }
    public int? TariffNameId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

// --- Case Tariff Calculation DTOs ---

public class CaseTariffCalculationRequest
{
    public int CaseId { get; set; }
}

public class CaseTariffCalculationResult
{
    public int? TariffCalcId { get; set; }
    public string? TariffCode { get; set; }
    public decimal? Value { get; set; }
    public decimal? AgreedRate { get; set; }
    public decimal Discount { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? TotalPayable { get; set; }
    public decimal? TotalOvercharged { get; set; }
    public decimal Qty { get; set; }
    public DateOnly DateOfProcedure { get; set; }
    public string? TariffDescription { get; set; }
    public int? CaseId { get; set; }
    public int? TariffId { get; set; }
    public int Seq { get; set; }
    public string? BaseTariffId { get; set; }
    public bool? Rejected { get; set; }
    public string? Colour { get; set; }
}

// --- Base Tariff CRUD DTOs ---

public class BaseTariffDto
{
    public string BaseTariffId { get; set; } = null!;
    public string? TariffCode { get; set; }
    public int? SpecialityId { get; set; }
    public string? TariffDescription { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? ChargeCodes { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
    // From Tariff rate table (for search results)
    public int? TariffId { get; set; }
    public decimal? TariffAmount { get; set; }
}

public class CreateBaseTariffDto
{
    public string BaseTariffId { get; set; } = null!;
    public string? TariffCode { get; set; }
    public int? SpecialityId { get; set; }
    public string? TariffDescription { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? ChargeCodes { get; set; }
}

public class UpdateBaseTariffDto
{
    public string? TariffCode { get; set; }
    public int? SpecialityId { get; set; }
    public string? TariffDescription { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? ChargeCodes { get; set; }
}

// --- Tariff Rate/Period CRUD DTOs ---

public class TariffRateDto
{
    public int TariffId { get; set; }
    public string? BaseTariffId { get; set; }
    public int? TariffNameId { get; set; }
    public decimal? TariffAmount { get; set; }
    public string? Metric { get; set; }
    public decimal? Quantity { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? TariffPeriodName { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

public class CreateTariffRateDto
{
    public string? BaseTariffId { get; set; }
    public int? TariffNameId { get; set; }
    public decimal? TariffAmount { get; set; }
    public string? Metric { get; set; }
    public decimal? Quantity { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? TariffPeriodName { get; set; }
}

public class UpdateTariffRateDto
{
    public string? BaseTariffId { get; set; }
    public int? TariffNameId { get; set; }
    public decimal? TariffAmount { get; set; }
    public string? Metric { get; set; }
    public decimal? Quantity { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? TariffPeriodName { get; set; }
}

// --- Tariff Name CRUD DTOs ---

public class TariffNameDto
{
    public int TariffNameId { get; set; }
    public string? TariffName { get; set; }
    public bool? Visible { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

public class CreateTariffNameDto
{
    public string? TariffName { get; set; }
    public bool? Visible { get; set; }
}

public class UpdateTariffNameDto
{
    public string? TariffName { get; set; }
    public bool? Visible { get; set; }
}
