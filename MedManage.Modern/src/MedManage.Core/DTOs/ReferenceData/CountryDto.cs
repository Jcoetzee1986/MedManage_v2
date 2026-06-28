namespace MedManage.Core.DTOs.ReferenceData;

public class CountryDto
{
    public int CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? CountryIsocode { get; set; }
    public string? CountryCurrencyCode { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCountryDto
{
    public string? CountryName { get; set; }
    public string? CountryIsocode { get; set; }
    public string? CountryCurrencyCode { get; set; }
}

public class UpdateCountryDto
{
    public int CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? CountryIsocode { get; set; }
    public string? CountryCurrencyCode { get; set; }
}
