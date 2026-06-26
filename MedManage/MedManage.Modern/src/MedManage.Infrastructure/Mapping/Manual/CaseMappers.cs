using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping.Manual;

public static class CaseMappers
{
    public static CaseDto ToDto(this Case entity) => new()
    {
        CaseId = entity.CaseId,
        AuthNumber = entity.AuthNumber,
        AccountNr = entity.AccountNr,
        MemberId = entity.MemberId,
        ReferToId = entity.ReferToId,
        ReferFromId = entity.ReferFromId,
        AdmissionDate = entity.AdmissionDate,
        AdmissionTime = entity.AdmissionTime,
        DischargeDate = entity.DischargeDate,
        DischargeTime = entity.DischargeTime,
        AuthTypeId = entity.AuthTypeId,
        WcaIod = entity.WcaIod,
        TotalLengthOfStay = entity.TotalLengthOfStay,
        TotalAmount = entity.TotalAmount,
        FinalInvoiceAmount = entity.FinalInvoiceAmount,
        FinalInvoiceAmountUpdated = entity.FinalInvoiceAmountUpdated,
        StatusId = entity.StatusId,
        CaseDescription = entity.CaseDescription,
        Changes = entity.Changes,
        Limits = entity.Limits,
        Exclusions = entity.Exclusions,
        DateCreated = entity.DateCreated,
        HasBooking = entity.HasBooking,
        ChangeToCaseDate = entity.ChangeToCaseDate,
        PenaltyPercentage = entity.PenaltyPercentage,
        CaseCategoryId = entity.CaseCategoryId,
        // Audit fields
        DateInserted = entity.DateInserted ?? default,
        UserID = entity.UserID ?? string.Empty,
        DateUpdated = entity.DateUpdated,
        UpdatedUserID = entity.UpdatedUserID,
        DateDeleted = entity.DateDeleted,
        // Flattened Member fields
        MemberNumber = entity.Member?.MemberNumber,
        MemberSurname = entity.Member?.Surname,
        MemberName = entity.Member?.Name,
        MemberIdNumber = entity.Member?.Idnumber,
        MemberDateOfBirth = entity.Member?.DateOfBirth,
        MemberMedicalAidName = entity.Member?.MedicalAid?.MedicalAidName,
        MemberProductName = null, // Product name needs separate join
        MemberStatusName = entity.Member?.MemberStatus?.MemberStatus1,
        // Flattened Status/Type
        CaseStatusName = entity.Status?.CaseStatus1,
        CaseTypeName = entity.AuthType?.CaseType1,
        // Flattened ReferTo provider
        ReferToPracticeName = entity.ReferTo?.PracticeName,
        ReferToPersonSurname = entity.ReferTo?.ServiceProviderSurname,
        ReferToPersonName = entity.ReferTo?.ServiceProviderName,
        ReferToSpeciality = entity.ReferTo?.Speciality?.Speciality1,
        // Flattened ReferFrom provider
        ReferFromPracticeName = entity.ReferFrom?.PracticeName,
        ReferFromPersonSurname = entity.ReferFrom?.ServiceProviderSurname,
        ReferFromPersonName = entity.ReferFrom?.ServiceProviderName,
        ReferFromSpeciality = entity.ReferFrom?.Speciality?.Speciality1,
    };

    public static Case ToEntity(this CreateCaseRequest request) => new()
    {
        AuthNumber = request.AuthNumber,
        AccountNr = request.AccountNr,
        MemberId = request.MemberId,
        ReferToId = request.ReferToId,
        ReferFromId = request.ReferFromId,
        AdmissionDate = request.AdmissionDate,
        AdmissionTime = request.AdmissionTime,
        DischargeDate = request.DischargeDate,
        DischargeTime = request.DischargeTime,
        AuthTypeId = request.AuthTypeId,
        WcaIod = request.WcaIod,
        TotalLengthOfStay = request.TotalLengthOfStay,
        TotalAmount = request.TotalAmount,
        FinalInvoiceAmount = request.FinalInvoiceAmount,
        FinalInvoiceAmountUpdated = request.FinalInvoiceAmountUpdated,
        StatusId = request.StatusId,
        CaseDescription = request.CaseDescription,
        Changes = request.Changes,
        Limits = request.Limits,
        Exclusions = request.Exclusions,
        DateCreated = request.DateCreated,
        HasBooking = request.HasBooking,
        ChangeToCaseDate = request.ChangeToCaseDate,
        PenaltyPercentage = request.PenaltyPercentage,
        CaseCategoryId = request.CaseCategoryId,
    };

    public static void ApplyTo(this UpdateCaseRequest request, Case entity)
    {
        entity.AuthNumber = request.AuthNumber;
        entity.AccountNr = request.AccountNr;
        entity.MemberId = request.MemberId;
        entity.ReferToId = request.ReferToId;
        entity.ReferFromId = request.ReferFromId;
        entity.AdmissionDate = request.AdmissionDate;
        entity.AdmissionTime = request.AdmissionTime;
        entity.DischargeDate = request.DischargeDate;
        entity.DischargeTime = request.DischargeTime;
        entity.AuthTypeId = request.AuthTypeId;
        entity.WcaIod = request.WcaIod;
        entity.TotalLengthOfStay = request.TotalLengthOfStay;
        entity.TotalAmount = request.TotalAmount;
        entity.FinalInvoiceAmount = request.FinalInvoiceAmount;
        entity.FinalInvoiceAmountUpdated = request.FinalInvoiceAmountUpdated;
        entity.StatusId = request.StatusId;
        entity.CaseDescription = request.CaseDescription;
        entity.Changes = request.Changes;
        entity.Limits = request.Limits;
        entity.Exclusions = request.Exclusions;
        entity.DateCreated = request.DateCreated;
        entity.HasBooking = request.HasBooking;
        entity.ChangeToCaseDate = request.ChangeToCaseDate;
        entity.PenaltyPercentage = request.PenaltyPercentage;
        entity.CaseCategoryId = request.CaseCategoryId;
    }

    public static List<CaseDto> ToDtoList(this IEnumerable<Case> entities)
        => entities.Select(e => e.ToDto()).ToList();
}
