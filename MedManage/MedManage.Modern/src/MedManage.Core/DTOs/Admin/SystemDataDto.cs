namespace MedManage.Core.DTOs.Admin;

/// <summary>
/// DTO representing system configuration data
/// </summary>
public class SystemDataDto
{
    public int SystemDataId { get; set; }
    public int? SystemCountryId { get; set; }
    public Guid? SystemUniqueIdentifier { get; set; }
    public string? SystemEmailAddress { get; set; }
    public string? SmtpServer { get; set; }
    public bool? Ssl { get; set; }
    public string? Username { get; set; }
    public int? SpecialIcu { get; set; }
    public int? Icu { get; set; }
    public int? HighCare { get; set; }
    public int? NeuroWard { get; set; }
    public int? InIsolation { get; set; }
    public int? GeneralWard { get; set; }
    public int? Paediatric { get; set; }
    public int? Maternity { get; set; }
    public int? DayCase { get; set; }
    public int? StepDown { get; set; }
    public int? Psychiatric { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? Address4 { get; set; }
    public string? AddressCode { get; set; }
    public string? Email { get; set; }
    public string? Fax { get; set; }
    public string? Telephone { get; set; }
    public string? Website { get; set; }
    public bool HasLogo { get; set; }
    public int? DefaultProviderId { get; set; }
}

/// <summary>
/// Request to create system configuration
/// </summary>
public class CreateSystemDataRequest
{
    public int? SystemCountryId { get; set; }
    public string? SystemEmailAddress { get; set; }
    public string? SmtpServer { get; set; }
    public bool? Ssl { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int? SpecialIcu { get; set; }
    public int? Icu { get; set; }
    public int? HighCare { get; set; }
    public int? NeuroWard { get; set; }
    public int? InIsolation { get; set; }
    public int? GeneralWard { get; set; }
    public int? Paediatric { get; set; }
    public int? Maternity { get; set; }
    public int? DayCase { get; set; }
    public int? StepDown { get; set; }
    public int? Psychiatric { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? Address4 { get; set; }
    public string? AddressCode { get; set; }
    public string? Email { get; set; }
    public string? Fax { get; set; }
    public string? Telephone { get; set; }
    public string? Website { get; set; }
    public int? DefaultProviderId { get; set; }
}

/// <summary>
/// Request to update system configuration
/// </summary>
public class UpdateSystemDataRequest
{
    public int SystemDataId { get; set; }
    public int? SystemCountryId { get; set; }
    public string? SystemEmailAddress { get; set; }
    public string? SmtpServer { get; set; }
    public bool? Ssl { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int? SpecialIcu { get; set; }
    public int? Icu { get; set; }
    public int? HighCare { get; set; }
    public int? NeuroWard { get; set; }
    public int? InIsolation { get; set; }
    public int? GeneralWard { get; set; }
    public int? Paediatric { get; set; }
    public int? Maternity { get; set; }
    public int? DayCase { get; set; }
    public int? StepDown { get; set; }
    public int? Psychiatric { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? Address4 { get; set; }
    public string? AddressCode { get; set; }
    public string? Email { get; set; }
    public string? Fax { get; set; }
    public string? Telephone { get; set; }
    public string? Website { get; set; }
    public int? DefaultProviderId { get; set; }
}
