using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Core.Entities;

[Keyless]
public partial class VwServiceProvider
{
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

    [StringLength(300)]
    [Unicode(false)]
    public string? Speciality { get; set; }

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

    [StringLength(100)]
    [Unicode(false)]
    public string? Language { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CountryName { get; set; }

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

    [Column("TariffInclVAT")]
    public bool? TariffInclVat { get; set; }

    public bool? Visible { get; set; }
}
