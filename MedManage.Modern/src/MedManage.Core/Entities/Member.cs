using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Member", Schema = "shared")]
[Index("MedicalAidId", "MemberId", "MemberNumber", "Name", "Surname", "MedAidProductId", "Initials", "DateOfBirth", "MbodRma", Name = "_dta_index_Member_7_607341228__K14_K1_K2_K6_K4_K48_K5_K11_K49")]
[Index("MedicalAidId", "Name", "MemberNumber", "Surname", "MemberId", "MedAidProductId", "Initials", "DateOfBirth", "MbodRma", Name = "_dta_index_Member_7_607341228__K14_K6_K2_K4_K1_K48_K5_K11_K49")]
[Index("MemberId", "MedAidProductId", "MedicalAidId", "MemberNumber", "Name", "Surname", "Initials", "DateOfBirth", "MbodRma", Name = "_dta_index_Member_7_607341228__K1_K48_K14_K2_K6_K4_K5_K11_K49")]
[Index("MemberNumber", "Name", "Surname", "Initials", "DateOfBirth", "MedicalAidId", "MbodRma", "MedAidProductId", Name = "_dta_index_Member_7_607341228__K2_K6_K4_K5_K11_K14_K49_K48_1")]
[Index("Surname", "MemberNumber", "Name", "MemberId", "MedAidProductId", "MedicalAidId", "Initials", "DateOfBirth", "MbodRma", Name = "_dta_index_Member_7_607341228__K4_K2_K6_K1_K48_K14_K5_K11_K49")]
[Index("Name", "MedAidProductId", "MemberId", "MemberNumber", "Surname", "MedicalAidId", "Initials", "DateOfBirth", "MbodRma", Name = "_dta_index_Member_7_607341228__K6_K48_K1_K2_K4_K14_K5_K11_K49")]
[Index("TitleId", "PeriodInCountryId", "GenderId", "MedicalAidId", "MarritalStatusId", "EmployerCountryId", "MemberStatusId", "MemberCountryId", "MemberLanguageId", "MemberRaceId", "MedAidProductId", Name = "idx_Shared_Member_FKs")]
[Index("MemberNumber", "Surname", "Initials", "Name", Name = "idx_Shared_Member_SearchCriteria")]
public partial class Member : BaseEntity
{
    [Key]
    [Column("MemberID")]
    public int MemberId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? MemberNumber { get; set; }

    [Column("TitleID")]
    public int? TitleId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Surname { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Initials { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("IDNumber")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Idnumber { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PassportNumber { get; set; }

    public DateOnly? PassportExpiryDate { get; set; }

    [Column("PeriodInCountryID")]
    public int? PeriodInCountryId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [Column("GenderID")]
    public int? GenderId { get; set; }

    public bool? HasMedicalAid { get; set; }

    [Column("MedicalAidID")]
    public int? MedicalAidId { get; set; }

    public DateOnly? DateOfBenefit { get; set; }

    public DateOnly? DateJoined { get; set; }

    public bool? Suspended { get; set; }

    public DateOnly? DateSuspended { get; set; }

    [Column("SuspendedReasonID")]
    public int? SuspendedReasonId { get; set; }

    public bool? MedicalAidExhausted { get; set; }

    public DateOnly? DateMedicalAidExhausted { get; set; }

    public bool? WaitingPeriodApplicable { get; set; }

    [Column("MarritalStatusID")]
    public int? MarritalStatusId { get; set; }

    [Column("EmployerCountryID")]
    public int? EmployerCountryId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? EmployerAddress { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? EmployerAddressCode { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? EmployerPhoneNumber { get; set; }

    public bool? Pensioner { get; set; }

    [Column("MemberStatusID")]
    public int? MemberStatusId { get; set; }

    [Column("MemberCountryID")]
    public int? MemberCountryId { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? MemberAddress1 { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? MemberAddress2 { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? MemberAddress3 { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? MemberAddressCode { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? MemberPhoneNumber { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? MemberCellNumber { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? NextOfKinName { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? NextOfKinRelationship { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? NextOfKinContactNumber { get; set; }

    [Column("MemberLanguageID")]
    public int? MemberLanguageId { get; set; }

    [Column("MemberRaceID")]
    public int? MemberRaceId { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? MemberDependents { get; set; }

    public bool? FundReinstated { get; set; }

    public DateOnly? FundReinstatedDate { get; set; }

    public bool? Deceased { get; set; }

    public DateOnly? DeceasedDate { get; set; }

    [Column("MedAidProductID")]
    public int? MedAidProductId { get; set; }

    [Column("MBOD_RMA")]
    public bool? MbodRma { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();

    [ForeignKey("GenderId")]
    [InverseProperty("Members")]
    public virtual Gender? Gender { get; set; }

    [ForeignKey("MarritalStatusId")]
    [InverseProperty("Members")]
    public virtual MarritalStatus? MarritalStatus { get; set; }

    [ForeignKey("MedicalAidId")]
    [InverseProperty("Members")]
    public virtual MedicalAid? MedicalAid { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<MemberChronicIllness> MemberChronicIllnesses { get; set; } = new List<MemberChronicIllness>();

    [ForeignKey("MemberCountryId")]
    [InverseProperty("Members")]
    public virtual Country? MemberCountry { get; set; }

    [ForeignKey("MemberLanguageId")]
    [InverseProperty("Members")]
    public virtual Language? MemberLanguage { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<MemberNote> MemberNotes { get; set; } = new List<MemberNote>();

    [ForeignKey("MemberRaceId")]
    [InverseProperty("Members")]
    public virtual Race? MemberRace { get; set; }

    [ForeignKey("MemberStatusId")]
    [InverseProperty("Members")]
    public virtual MemberStatus? MemberStatus { get; set; }

    [ForeignKey("PeriodInCountryId")]
    [InverseProperty("Members")]
    public virtual PeriodInCountry? PeriodInCountry { get; set; }

    [ForeignKey("TitleId")]
    [InverseProperty("Members")]
    public virtual Title? Title { get; set; }
}
