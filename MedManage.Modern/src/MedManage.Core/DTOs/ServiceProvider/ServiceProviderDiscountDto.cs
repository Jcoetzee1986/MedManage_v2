namespace MedManage.Core.DTOs.ServiceProvider;

/// <summary>
/// DTO for ServiceProvider_MainClient_Discount
/// </summary>
public class ServiceProviderDiscountDto
{
    public int ServiceProviderId { get; set; }
    public int MainClientId { get; set; }
    public decimal? Discount { get; set; }
}

/// <summary>
/// Request to create/update a discount for a service provider per MainClient
/// </summary>
public class CreateServiceProviderDiscountRequest
{
    public int MainClientId { get; set; }
    public decimal? Discount { get; set; }
}

/// <summary>
/// Request to update a discount for a service provider per MainClient
/// </summary>
public class UpdateServiceProviderDiscountRequest
{
    public int MainClientId { get; set; }
    public decimal? Discount { get; set; }
}
