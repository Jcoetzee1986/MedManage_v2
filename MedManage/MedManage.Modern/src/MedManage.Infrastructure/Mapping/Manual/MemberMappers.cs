using MedManage.Core.DTOs.Member;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping.Manual;

public static class MemberMappers
{
    public static MemberDto ToDto(this Member entity) => new()
    {
        MemberId = entity.MemberId,
        MemberNumber = entity.MemberNumber,
        Surname = entity.Surname,
        Initials = entity.Initials,
        Name = entity.Name,
        Idnumber = entity.Idnumber,
        PassportNumber = entity.PassportNumber,
        PassportExpiryDate = entity.PassportExpiryDate,
        DateOfBirth = entity.DateOfBirth,
        HasMedicalAid = entity.HasMedicalAid,
        DateOfBenefit = entity.DateOfBenefit,
        DateJoined = entity.DateJoined,
        Suspended = entity.Suspended,
        DateSuspended = entity.DateSuspended,
        MedicalAidExhausted = entity.MedicalAidExhausted,
        DateMedicalAidExhausted = entity.DateMedicalAidExhausted,
        WaitingPeriodApplicable = entity.WaitingPeriodApplicable,
        EmployerAddress = entity.EmployerAddress,
        EmployerAddressCode = entity.EmployerAddressCode,
        EmployerPhoneNumber = entity.EmployerPhoneNumber,
        Pensioner = entity.Pensioner,
        MemberAddress1 = entity.MemberAddress1,
        MemberAddress2 = entity.MemberAddress2,
        MemberAddress3 = entity.MemberAddress3,
        MemberAddressCode = entity.MemberAddressCode,
        MemberPhoneNumber = entity.MemberPhoneNumber,
        MemberCellNumber = entity.MemberCellNumber,
        NextOfKinName = entity.NextOfKinName,
        NextOfKinRelationship = entity.NextOfKinRelationship,
        NextOfKinContactNumber = entity.NextOfKinContactNumber,
        MemberDependents = entity.MemberDependents,
        FundReinstated = entity.FundReinstated,
        FundReinstatedDate = entity.FundReinstatedDate,
        Deceased = entity.Deceased,
        DeceasedDate = entity.DeceasedDate,
        MbodRma = entity.MbodRma,
        // Foreign Key IDs
        TitleId = entity.TitleId,
        GenderId = entity.GenderId,
        MedicalAidId = entity.MedicalAidId,
        MemberStatusId = entity.MemberStatusId,
        MemberCountryId = entity.MemberCountryId,
        MemberLanguageId = entity.MemberLanguageId,
        MemberRaceId = entity.MemberRaceId,
        MarritalStatusId = entity.MarritalStatusId,
        EmployerCountryId = entity.EmployerCountryId,
        PeriodInCountryId = entity.PeriodInCountryId,
        SuspendedReasonId = entity.SuspendedReasonId,
        MedAidProductId = entity.MedAidProductId,
        // Flattened navigation properties
        MedicalAidName = entity.MedicalAid?.MedicalAidName,
        MemberStatusName = entity.MemberStatus?.MemberStatus1,
        GenderName = entity.Gender?.GenderDescription,
        TitleName = entity.Title?.Title1,
        // Audit fields
        DateInserted = entity.DateInserted ?? default,
        UserID = entity.UserID ?? string.Empty,
        DateUpdated = entity.DateUpdated,
        UpdatedUserID = entity.UpdatedUserID,
        DateDeleted = entity.DateDeleted,
    };

    public static Member ToEntity(this CreateMemberRequest request) => new()
    {
        MemberNumber = request.MemberNumber,
        TitleId = request.TitleId,
        Surname = request.Surname,
        Initials = request.Initials,
        Name = request.Name,
        Idnumber = request.Idnumber,
        PassportNumber = request.PassportNumber,
        PassportExpiryDate = request.PassportExpiryDate,
        PeriodInCountryId = request.PeriodInCountryId,
        DateOfBirth = request.DateOfBirth,
        GenderId = request.GenderId,
        HasMedicalAid = request.HasMedicalAid,
        MedicalAidId = request.MedicalAidId,
        DateOfBenefit = request.DateOfBenefit,
        DateJoined = request.DateJoined,
        Suspended = request.Suspended,
        DateSuspended = request.DateSuspended,
        SuspendedReasonId = request.SuspendedReasonId,
        MedicalAidExhausted = request.MedicalAidExhausted,
        DateMedicalAidExhausted = request.DateMedicalAidExhausted,
        WaitingPeriodApplicable = request.WaitingPeriodApplicable,
        MarritalStatusId = request.MarritalStatusId,
        EmployerCountryId = request.EmployerCountryId,
        EmployerAddress = request.EmployerAddress,
        EmployerAddressCode = request.EmployerAddressCode,
        EmployerPhoneNumber = request.EmployerPhoneNumber,
        Pensioner = request.Pensioner,
        MemberStatusId = request.MemberStatusId,
        MemberCountryId = request.MemberCountryId,
        MemberAddress1 = request.MemberAddress1,
        MemberAddress2 = request.MemberAddress2,
        MemberAddress3 = request.MemberAddress3,
        MemberAddressCode = request.MemberAddressCode,
        MemberPhoneNumber = request.MemberPhoneNumber,
        MemberCellNumber = request.MemberCellNumber,
        NextOfKinName = request.NextOfKinName,
        NextOfKinRelationship = request.NextOfKinRelationship,
        NextOfKinContactNumber = request.NextOfKinContactNumber,
        MemberLanguageId = request.MemberLanguageId,
        MemberRaceId = request.MemberRaceId,
        MemberDependents = request.MemberDependents,
        FundReinstated = request.FundReinstated,
        FundReinstatedDate = request.FundReinstatedDate,
        Deceased = request.Deceased,
        DeceasedDate = request.DeceasedDate,
        MedAidProductId = request.MedAidProductId,
        MbodRma = request.MbodRma,
    };

    public static void ApplyTo(this UpdateMemberRequest request, Member entity)
    {
        entity.MemberNumber = request.MemberNumber;
        entity.TitleId = request.TitleId;
        entity.Surname = request.Surname;
        entity.Initials = request.Initials;
        entity.Name = request.Name;
        entity.Idnumber = request.Idnumber;
        entity.PassportNumber = request.PassportNumber;
        entity.PassportExpiryDate = request.PassportExpiryDate;
        entity.PeriodInCountryId = request.PeriodInCountryId;
        entity.DateOfBirth = request.DateOfBirth;
        entity.GenderId = request.GenderId;
        entity.HasMedicalAid = request.HasMedicalAid;
        entity.MedicalAidId = request.MedicalAidId;
        entity.DateOfBenefit = request.DateOfBenefit;
        entity.DateJoined = request.DateJoined;
        entity.Suspended = request.Suspended;
        entity.DateSuspended = request.DateSuspended;
        entity.SuspendedReasonId = request.SuspendedReasonId;
        entity.MedicalAidExhausted = request.MedicalAidExhausted;
        entity.DateMedicalAidExhausted = request.DateMedicalAidExhausted;
        entity.WaitingPeriodApplicable = request.WaitingPeriodApplicable;
        entity.MarritalStatusId = request.MarritalStatusId;
        entity.EmployerCountryId = request.EmployerCountryId;
        entity.EmployerAddress = request.EmployerAddress;
        entity.EmployerAddressCode = request.EmployerAddressCode;
        entity.EmployerPhoneNumber = request.EmployerPhoneNumber;
        entity.Pensioner = request.Pensioner;
        entity.MemberStatusId = request.MemberStatusId;
        entity.MemberCountryId = request.MemberCountryId;
        entity.MemberAddress1 = request.MemberAddress1;
        entity.MemberAddress2 = request.MemberAddress2;
        entity.MemberAddress3 = request.MemberAddress3;
        entity.MemberAddressCode = request.MemberAddressCode;
        entity.MemberPhoneNumber = request.MemberPhoneNumber;
        entity.MemberCellNumber = request.MemberCellNumber;
        entity.NextOfKinName = request.NextOfKinName;
        entity.NextOfKinRelationship = request.NextOfKinRelationship;
        entity.NextOfKinContactNumber = request.NextOfKinContactNumber;
        entity.MemberLanguageId = request.MemberLanguageId;
        entity.MemberRaceId = request.MemberRaceId;
        entity.MemberDependents = request.MemberDependents;
        entity.FundReinstated = request.FundReinstated;
        entity.FundReinstatedDate = request.FundReinstatedDate;
        entity.Deceased = request.Deceased;
        entity.DeceasedDate = request.DeceasedDate;
        entity.MedAidProductId = request.MedAidProductId;
        entity.MbodRma = request.MbodRma;
    }

    public static List<MemberDto> ToDtoList(this IEnumerable<Member> entities)
        => entities.Select(e => e.ToDto()).ToList();
}
