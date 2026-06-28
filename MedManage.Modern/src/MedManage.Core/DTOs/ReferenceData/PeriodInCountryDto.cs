namespace MedManage.Core.DTOs.ReferenceData;

public class PeriodInCountryDto
{
    public int PeriodInCountryId { get; set; }
    public string? PeriodInCountry { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreatePeriodInCountryDto
{
    public string? PeriodInCountry { get; set; }
}

public class UpdatePeriodInCountryDto
{
    public string? PeriodInCountry { get; set; }
}
