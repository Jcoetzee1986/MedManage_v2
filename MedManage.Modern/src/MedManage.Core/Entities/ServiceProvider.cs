using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("ServiceProvider", Schema = "shared")]
[Index("ServiceProviderId", "PracticeNr", "PracticeName", Name = "_dta_index_ServiceProvider_ServiceProciderID_PracticeNumber_PracticeName")]
[Index("SpecialityId", Name = "idx_Shared_ServiceProvider_FKs")]
[Index("ServiceProviderName", "ServiceProviderSurname", Name = "idx_Shared_ServiceProvider_SearchColumns_Name_Suname")]
[Index("PracticeName", "PracticeNr", Name = "idx_Shared_ServiceProvider_SearchColumns_PracticeName_PracticeNr")]
public partial class ServiceProvider : BaseEntity
{
    [Key]
    [Column("ServiceProviderID")]
    public int ServiceProviderId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? ServiceProviderName { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? ServiceProviderSurname { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? ServiceProviderInitials { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? PracticeName { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? GroupPracticeNr { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PracticeNr { get; set; }

    public int? NoOfPartners { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ServiceArea { get; set; }

    [Column("SpecialityID")]
    public int? SpecialityId { get; set; }

    public bool? IsHospital { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PracticeAddress1 { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PracticeAddress2 { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PracticeAddress3 { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PracticeAddress4 { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? PracticeAddressCode { get; set; }

    [Column("PracticePAddress1")]
    [StringLength(300)]
    [Unicode(false)]
    public string? PracticePaddress1 { get; set; }

    [Column("PracticePAddress2")]
    [StringLength(300)]
    [Unicode(false)]
    public string? PracticePaddress2 { get; set; }

    [Column("PracticePAddress3")]
    [StringLength(300)]
    [Unicode(false)]
    public string? PracticePaddress3 { get; set; }

    [Column("PracticePAddress4")]
    [StringLength(300)]
    [Unicode(false)]
    public string? PracticePaddress4 { get; set; }

    [Column("PracticePAddressCode")]
    [StringLength(300)]
    [Unicode(false)]
    public string? PracticePaddressCode { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? FaxNumber { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? EmailAddress { get; set; }

    [Column("LanguageID")]
    public int? LanguageId { get; set; }

    [Column("CountryID")]
    public int? CountryId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? BankName { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? BankBranch { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? BankBranchCode { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? BankAccountType { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? BankAccountNumber { get; set; }

    [Column("TariffStructureID")]
    [StringLength(3)]
    [Unicode(false)]
    public string? TariffStructureId { get; set; }

    [Column("TariffInclVAT")]
    public bool? TariffInclVat { get; set; }

    public bool? Visible { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CellNumber { get; set; }

    [InverseProperty("ReferFrom")]
    public virtual ICollection<Case> CaseReferFroms { get; set; } = new List<Case>();

    [InverseProperty("ReferTo")]
    public virtual ICollection<Case> CaseReferTos { get; set; } = new List<Case>();

    [ForeignKey("SpecialityId")]
    [InverseProperty("ServiceProviders")]
    public virtual Speciality? Speciality { get; set; }
}
