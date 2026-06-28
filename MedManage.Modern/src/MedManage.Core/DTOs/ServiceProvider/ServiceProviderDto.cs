namespace MedManage.Core.DTOs.ServiceProvider;

/// <summary>
/// ServiceProvider read model
/// </summary>
public class ServiceProviderDto
{
    public int ServiceProviderId { get; set; }
    public string? ServiceProviderName { get; set; }
    public string? ServiceProviderSurname { get; set; }
    public string? ServiceProviderInitials { get; set; }
    public string? PracticeName { get; set; }
    public string? GroupPracticeNr { get; set; }
    public string? PracticeNr { get; set; }
    public int? NoOfPartners { get; set; }
    public string? ServiceArea { get; set; }
    public int? SpecialityId { get; set; }
    public bool? IsHospital { get; set; }
    public string? PracticeAddress1 { get; set; }
    public string? PracticeAddress2 { get; set; }
    public string? PracticeAddress3 { get; set; }
    public string? PracticeAddress4 { get; set; }
    public string? PracticeAddressCode { get; set; }
    public string? PracticePaddress1 { get; set; }
    public string? PracticePaddress2 { get; set; }
    public string? PracticePaddress3 { get; set; }
    public string? PracticePaddress4 { get; set; }
    public string? PracticePaddressCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? EmailAddress { get; set; }
    public int? LanguageId { get; set; }
    public int? CountryId { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? BankBranchCode { get; set; }
    public string? BankAccountType { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? TariffStructureId { get; set; }
    public bool? TariffInclVat { get; set; }
    public bool? Visible { get; set; }
    public string? CellNumber { get; set; }

    // Audit fields
    public DateTime DateInserted { get; set; }
    public string UserID { get; set; } = string.Empty;
    public DateTime? DateUpdated { get; set; }
    public string? UpdatedUserID { get; set; }
    public DateTime? DateDeleted { get; set; }
}
