namespace MedManage.Core.DTOs.ServiceProvider;

/// <summary>
/// Request model for searching service providers
/// </summary>
public class ServiceProviderSearchRequest
{
    public string? ServiceProviderName { get; set; }
    public string? ServiceProviderSurname { get; set; }
    public string? PracticeName { get; set; }
    public string? PracticeNr { get; set; }
    public int? SpecialityId { get; set; }
    public bool? IsHospital { get; set; }
    public bool? Visible { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
