namespace MedManage.Core.DTOs.ServiceProvider;

/// <summary>
/// DTO for ServiceProvider_Tariff (tariff assignment)
/// </summary>
public class ServiceProviderTariffDto
{
    public long ServiceProviderTariffId { get; set; }
    public int ServiceProviderId { get; set; }
    public int? TariffNameId { get; set; }
    public string? TariffName { get; set; }
    public int? MainClientId { get; set; }
    public string? MainClientName { get; set; }
    public DateOnly? StartActiveDate { get; set; }
    public DateOnly? EndActiveDate { get; set; }
    public int? TariffPeriodName { get; set; }
    public decimal? PercentageAdded { get; set; }
}

/// <summary>
/// Request to create a tariff assignment for a service provider
/// </summary>
public class CreateServiceProviderTariffRequest
{
    public int? TariffNameId { get; set; }
    public int? MainClientId { get; set; }
    public DateOnly? StartActiveDate { get; set; }
    public DateOnly? EndActiveDate { get; set; }
    public int? TariffPeriodName { get; set; }
    public decimal? PercentageAdded { get; set; }
}

/// <summary>
/// Request to update a tariff assignment for a service provider
/// </summary>
public class UpdateServiceProviderTariffRequest
{
    public long ServiceProviderTariffId { get; set; }
    public int? TariffNameId { get; set; }
    public int? MainClientId { get; set; }
    public DateOnly? StartActiveDate { get; set; }
    public DateOnly? EndActiveDate { get; set; }
    public int? TariffPeriodName { get; set; }
    public decimal? PercentageAdded { get; set; }
}
