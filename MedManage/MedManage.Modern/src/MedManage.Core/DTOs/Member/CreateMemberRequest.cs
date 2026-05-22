namespace MedManage.Core.DTOs.Member;

/// <summary>
/// Request model for creating a new member
/// </summary>
public class CreateMemberRequest
{
    public string? MemberNumber { get; set; }
    public int? TitleId { get; set; }
    public string? Surname { get; set; }
    public string? Initials { get; set; }
    public string? Name { get; set; }
    public string? Idnumber { get; set; }
    public string? PassportNumber { get; set; }
    public DateOnly? PassportExpiryDate { get; set; }
    public int? PeriodInCountryId { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public int? GenderId { get; set; }
    public bool? HasMedicalAid { get; set; }
    public int? MedicalAidId { get; set; }
    public DateOnly? DateOfBenefit { get; set; }
    public DateOnly? DateJoined { get; set; }
    public bool? Suspended { get; set; }
    public DateOnly? DateSuspended { get; set; }
    public int? SuspendedReasonId { get; set; }
    public bool? MedicalAidExhausted { get; set; }
    public DateOnly? DateMedicalAidExhausted { get; set; }
    public bool? WaitingPeriodApplicable { get; set; }
    public int? MarritalStatusId { get; set; }
    public int? EmployerCountryId { get; set; }
    public string? EmployerAddress { get; set; }
    public string? EmployerAddressCode { get; set; }
    public string? EmployerPhoneNumber { get; set; }
    public bool? Pensioner { get; set; }
    public int? MemberStatusId { get; set; }
    public int? MemberCountryId { get; set; }
    public string? MemberAddress1 { get; set; }
    public string? MemberAddress2 { get; set; }
    public string? MemberAddress3 { get; set; }
    public string? MemberAddressCode { get; set; }
    public string? MemberPhoneNumber { get; set; }
    public string? MemberCellNumber { get; set; }
    public string? NextOfKinName { get; set; }
    public string? NextOfKinRelationship { get; set; }
    public string? NextOfKinContactNumber { get; set; }
    public int? MemberLanguageId { get; set; }
    public int? MemberRaceId { get; set; }
    public string? MemberDependents { get; set; }
    public bool? FundReinstated { get; set; }
    public DateOnly? FundReinstatedDate { get; set; }
    public bool? Deceased { get; set; }
    public DateOnly? DeceasedDate { get; set; }
    public int? MedAidProductId { get; set; }
    public bool? MbodRma { get; set; }
}
