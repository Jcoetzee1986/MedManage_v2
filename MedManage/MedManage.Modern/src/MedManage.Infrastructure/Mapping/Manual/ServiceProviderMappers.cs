using MedManage.Core.DTOs.ServiceProvider;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping.Manual;

public static class ServiceProviderMappers
{
    public static ServiceProviderDto ToDto(this ServiceProvider entity) => new()
    {
        ServiceProviderId = entity.ServiceProviderId,
        ServiceProviderName = entity.ServiceProviderName,
        ServiceProviderSurname = entity.ServiceProviderSurname,
        ServiceProviderInitials = entity.ServiceProviderInitials,
        PracticeName = entity.PracticeName,
        GroupPracticeNr = entity.GroupPracticeNr,
        PracticeNr = entity.PracticeNr,
        NoOfPartners = entity.NoOfPartners,
        ServiceArea = entity.ServiceArea,
        SpecialityId = entity.SpecialityId,
        IsHospital = entity.IsHospital,
        PracticeAddress1 = entity.PracticeAddress1,
        PracticeAddress2 = entity.PracticeAddress2,
        PracticeAddress3 = entity.PracticeAddress3,
        PracticeAddress4 = entity.PracticeAddress4,
        PracticeAddressCode = entity.PracticeAddressCode,
        PracticePaddress1 = entity.PracticePaddress1,
        PracticePaddress2 = entity.PracticePaddress2,
        PracticePaddress3 = entity.PracticePaddress3,
        PracticePaddress4 = entity.PracticePaddress4,
        PracticePaddressCode = entity.PracticePaddressCode,
        PhoneNumber = entity.PhoneNumber,
        FaxNumber = entity.FaxNumber,
        EmailAddress = entity.EmailAddress,
        LanguageId = entity.LanguageId,
        CountryId = entity.CountryId,
        BankName = entity.BankName,
        BankBranch = entity.BankBranch,
        BankBranchCode = entity.BankBranchCode,
        BankAccountType = entity.BankAccountType,
        BankAccountNumber = entity.BankAccountNumber,
        TariffStructureId = entity.TariffStructureId,
        TariffInclVat = entity.TariffInclVat,
        Visible = entity.Visible,
        CellNumber = entity.CellNumber,
        DateInserted = entity.DateInserted ?? default,
        UserID = entity.UserID ?? string.Empty,
        DateUpdated = entity.DateUpdated,
        UpdatedUserID = entity.UpdatedUserID,
        DateDeleted = entity.DateDeleted,
    };

    public static ServiceProviderAutocompleteDto ToAutocompleteDto(this ServiceProvider entity) => new()
    {
        ServiceProviderId = entity.ServiceProviderId,
        ServiceProviderName = entity.ServiceProviderName,
        ServiceProviderSurname = entity.ServiceProviderSurname,
        PracticeName = entity.PracticeName,
        PracticeNr = entity.PracticeNr,
    };

    public static ServiceProvider ToEntity(this CreateServiceProviderRequest request) => new()
    {
        ServiceProviderName = request.ServiceProviderName,
        ServiceProviderSurname = request.ServiceProviderSurname,
        PracticeName = request.PracticeName,
        PracticeNr = request.PracticeNr,
        SpecialityId = request.SpecialityId,
        IsHospital = request.IsHospital,
        Visible = request.Visible,
        PhoneNumber = request.PhoneNumber,
        FaxNumber = request.FaxNumber,
        EmailAddress = request.EmailAddress,
    };

    public static void ApplyTo(this UpdateServiceProviderRequest request, ServiceProvider entity)
    {
        entity.ServiceProviderName = request.ServiceProviderName;
        entity.ServiceProviderSurname = request.ServiceProviderSurname;
        entity.PracticeName = request.PracticeName;
        entity.PracticeNr = request.PracticeNr;
        entity.SpecialityId = request.SpecialityId;
        entity.IsHospital = request.IsHospital;
        entity.Visible = request.Visible;
        entity.PhoneNumber = request.PhoneNumber;
        entity.FaxNumber = request.FaxNumber;
        entity.EmailAddress = request.EmailAddress;
    }

    public static List<ServiceProviderDto> ToDtoList(this IEnumerable<ServiceProvider> entities)
        => entities.Select(e => e.ToDto()).ToList();

    public static List<ServiceProviderAutocompleteDto> ToAutocompleteDtoList(this IEnumerable<ServiceProvider> entities)
        => entities.Select(e => e.ToAutocompleteDto()).ToList();

    // ─── ServiceProviderTariff ───────────────────────────────────
    public static ServiceProviderTariffDto ToDto(this ServiceProviderTariff entity) => new()
    {
        ServiceProviderTariffId = entity.ServiceProviderTariffId,
        ServiceProviderId = entity.ServiceProviderId,
        TariffNameId = entity.TariffNameId,
        MainClientId = entity.MainClientId,
        StartActiveDate = entity.StartActiveDate,
        EndActiveDate = entity.EndActiveDate,
        TariffPeriodName = entity.TariffPeriodName,
        PercentageAdded = entity.PercentageAdded,
    };

    public static ServiceProviderTariff ToEntity(this CreateServiceProviderTariffRequest request) => new()
    {
        TariffNameId = request.TariffNameId,
        MainClientId = request.MainClientId,
        StartActiveDate = request.StartActiveDate,
        EndActiveDate = request.EndActiveDate,
        TariffPeriodName = request.TariffPeriodName,
        PercentageAdded = request.PercentageAdded,
    };

    // ─── ServiceProviderTariffCustom ─────────────────────────────
    public static ServiceProviderTariffCustomDto ToDto(this ServiceProviderTariffCustom entity) => new()
    {
        ServiceProviderTariffCustomId = entity.ServiceProviderTariffCustomId,
        ServiceProviderId = entity.ServiceProviderId,
        BaseTariffId = entity.BaseTariffId,
        MainClientId = entity.MainClientId,
        TariffAmount = entity.TariffAmount,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
    };

    public static ServiceProviderTariffCustom ToEntity(this CreateServiceProviderTariffCustomRequest request) => new()
    {
        BaseTariffId = request.BaseTariffId,
        MainClientId = request.MainClientId,
        TariffAmount = request.TariffAmount,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
    };

    // ─── ServiceProviderMainClientDiscount ───────────────────────
    public static ServiceProviderDiscountDto ToDto(this ServiceProviderMainClientDiscount entity) => new()
    {
        ServiceProviderId = entity.ServiceProviderId,
        MainClientId = entity.MainClientId,
        Discount = entity.Discount,
    };
}
