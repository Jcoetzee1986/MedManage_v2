namespace MedManage.Core.DTOs.ServiceProvider;

/// <summary>
/// DTO for ServiceProvider_Tariff_Custom (custom tariff overrides)
/// </summary>
public class ServiceProviderTariffCustomDto
{
    public long ServiceProviderTariffCustomId { get; set; }
    public int ServiceProviderId { get; set; }
    public string? BaseTariffId { get; set; }
    public int? MainClientId { get; set; }
    public decimal? TariffAmount { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

/// <summary>
/// Request to create a custom tariff override for a service provider
/// </summary>
public class CreateServiceProviderTariffCustomRequest
{
    public string? BaseTariffId { get; set; }
    public int? MainClientId { get; set; }
    public decimal? TariffAmount { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

/// <summary>
/// Request to update a custom tariff override for a service provider
/// </summary>
public class UpdateServiceProviderTariffCustomRequest
{
    public long ServiceProviderTariffCustomId { get; set; }
    public string? BaseTariffId { get; set; }
    public int? MainClientId { get; set; }
    public decimal? TariffAmount { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
