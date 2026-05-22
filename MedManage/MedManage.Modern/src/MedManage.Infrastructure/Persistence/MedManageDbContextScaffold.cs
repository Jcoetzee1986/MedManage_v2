using System;
using System.Collections.Generic;
using MedManage.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Persistence;

public partial class MedManageDbContextScaffold : DbContext
{
    public MedManageDbContextScaffold(DbContextOptions<MedManageDbContextScaffold> options)
        : base(options)
    {
    }

    public virtual DbSet<AllCodesDatum> AllCodesData { get; set; } // TODO: Table does not exist - regenerate or remove

    public virtual DbSet<AppUpdate> AppUpdates { get; set; }

    public virtual DbSet<AspnetApplication> AspnetApplications { get; set; }

    public virtual DbSet<AspnetMembership> AspnetMemberships { get; set; }

    public virtual DbSet<AspnetPath> AspnetPaths { get; set; }

    public virtual DbSet<AspnetPersonalizationAllUser> AspnetPersonalizationAllUsers { get; set; }

    public virtual DbSet<AspnetPersonalizationPerUser> AspnetPersonalizationPerUsers { get; set; }

    public virtual DbSet<AspnetProfile> AspnetProfiles { get; set; }

    public virtual DbSet<AspnetRole> AspnetRoles { get; set; }

    public virtual DbSet<AspnetSchemaVersion> AspnetSchemaVersions { get; set; }

    public virtual DbSet<AspnetUser> AspnetUsers { get; set; }

    public virtual DbSet<AspnetUsersInRole> AspnetUsersInRoles { get; set; }

    public virtual DbSet<AspnetWebEventEvent> AspnetWebEventEvents { get; set; }

    public virtual DbSet<BaseTariff> BaseTariffs { get; set; }

    public virtual DbSet<BaseTariff1> BaseTariffs1 { get; set; }

    public virtual DbSet<BillingStatus> BillingStatuses { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<CaseBilling> CaseBillings { get; set; }

    public virtual DbSet<CaseBillingComment> CaseBillingComments { get; set; }

    public virtual DbSet<CaseCategory> CaseCategories { get; set; }

    public virtual DbSet<CaseChecklist> CaseChecklists { get; set; }

    public virtual DbSet<CaseComment> CaseComments { get; set; }

    public virtual DbSet<CaseCpt> CaseCpts { get; set; }

    public virtual DbSet<CaseDiscount> CaseDiscounts { get; set; }

    public virtual DbSet<CaseExclusion> CaseExclusions { get; set; }

    public virtual DbSet<CaseFacilityType> CaseFacilityTypes { get; set; }

    public virtual DbSet<CaseIcd> CaseIcds { get; set; }

    public virtual DbSet<CaseLetterNote> CaseLetterNotes { get; set; }

    public virtual DbSet<CaseLink> CaseLinks { get; set; }

    public virtual DbSet<CaseLink1> CaseLinks1 { get; set; }

    public virtual DbSet<CaseLink2> CaseLinks2 { get; set; }

    public virtual DbSet<CaseLinkedFile> CaseLinkedFiles { get; set; }

    public virtual DbSet<CaseManagementAudit> CaseManagementAudits { get; set; }

    public virtual DbSet<CaseManagementAuditDetail> CaseManagementAuditDetails { get; set; }

    public virtual DbSet<CaseNappiCode> CaseNappiCodes { get; set; }

    public virtual DbSet<CaseNote> CaseNotes { get; set; }

    public virtual DbSet<CaseStatus> CaseStatuses { get; set; }

    public virtual DbSet<CaseTariff> CaseTariffs { get; set; }

    public virtual DbSet<CaseType> CaseTypes { get; set; }

    public virtual DbSet<ChecklistTemplate> ChecklistTemplates { get; set; }

    public virtual DbSet<ChronicIllness> ChronicIllnesses { get; set; }

    public virtual DbSet<ChronicIllness1> ChronicIllnesses1 { get; set; }

    public virtual DbSet<ClientUpdate> ClientUpdates { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CountryVat> CountryVats { get; set; }

    public virtual DbSet<Cpt> Cpts { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<EpisodeCase> EpisodeCases { get; set; }

    public virtual DbSet<Exclusion> Exclusions { get; set; }

    public virtual DbSet<FacilityType> FacilityTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Icd> Icds { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LinkedFile> LinkedFiles { get; set; }

    public virtual DbSet<MainClient> MainClients { get; set; }

    public virtual DbSet<MainClientMedicalAidProduct> MainClientMedicalAidProducts { get; set; }

    public virtual DbSet<MainClientTariff> MainClientTariffs { get; set; }

    public virtual DbSet<MarritalStatus> MarritalStatuses { get; set; }

    public virtual DbSet<MedicalAid> MedicalAids { get; set; }

    public virtual DbSet<MedicalAidExclusion> MedicalAidExclusions { get; set; }

    public virtual DbSet<MedicalAidProduct> MedicalAidProducts { get; set; }

    public virtual DbSet<MedicalAidTariff> MedicalAidTariffs { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberChronicIllness> MemberChronicIllnesses { get; set; }

    public virtual DbSet<MemberMedicalAidProduct> MemberMedicalAidProducts { get; set; }

    public virtual DbSet<MemberNote> MemberNotes { get; set; }

    public virtual DbSet<MemberStatus> MemberStatuses { get; set; }

    public virtual DbSet<NappiCode> NappiCodes { get; set; }

    public virtual DbSet<PeriodInCountry> PeriodInCountries { get; set; }

    public virtual DbSet<Race> Races { get; set; }

    public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }

    public virtual DbSet<ServiceProviderMainClientDiscount> ServiceProviderMainClientDiscounts { get; set; }

    public virtual DbSet<ServiceProviderTariff> ServiceProviderTariffs { get; set; }

    public virtual DbSet<ServiceProviderTariffCustom> ServiceProviderTariffCustoms { get; set; }

    public virtual DbSet<SessionUserCase> SessionUserCases { get; set; }

    public virtual DbSet<Speciality> Specialities { get; set; }

    public virtual DbSet<SuspendedReason> SuspendedReasons { get; set; }

    public virtual DbSet<SystemDatum> SystemData { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    public virtual DbSet<TariffCalc> TariffCalcs { get; set; }

    public virtual DbSet<TariffName> TariffNames { get; set; }

    public virtual DbSet<TariffName1> TariffNames1 { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<VwAspnetApplication> VwAspnetApplications { get; set; }

    public virtual DbSet<VwAspnetMembershipUser> VwAspnetMembershipUsers { get; set; }

    public virtual DbSet<VwAspnetProfile> VwAspnetProfiles { get; set; }

    public virtual DbSet<VwAspnetRole> VwAspnetRoles { get; set; }

    public virtual DbSet<VwAspnetUser> VwAspnetUsers { get; set; }

    public virtual DbSet<VwAspnetUsersInRole> VwAspnetUsersInRoles { get; set; }

    public virtual DbSet<VwAspnetWebPartStatePath> VwAspnetWebPartStatePaths { get; set; }

    public virtual DbSet<VwAspnetWebPartStateShared> VwAspnetWebPartStateShareds { get; set; }

    public virtual DbSet<VwAspnetWebPartStateUser> VwAspnetWebPartStateUsers { get; set; }

    public virtual DbSet<VwCaseDetail> VwCaseDetails { get; set; }

    public virtual DbSet<VwCaseDetailWithAmount> VwCaseDetailWithAmounts { get; set; }

    public virtual DbSet<VwServiceProvider> VwServiceProviders { get; set; }

    public virtual DbSet<XHospitalType> XHospitalTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllCodesDatum>(entity =>
        {
            entity.Property(e => e.CasePrefix).IsFixedLength();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_AllCodesData_DateInserted");
            entity.Property(e => e.RecordKey).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AppUpdate>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_AppUpdates_DateInserted");
        });

        modelBuilder.Entity<AspnetApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId)
                .HasName("PK__aspnet_A__C93A4C982022C2A6")
                .IsClustered(false);

            entity.HasIndex(e => e.LoweredApplicationName, "aspnet_Applications_Index").IsClustered();

            entity.Property(e => e.ApplicationId).HasDefaultValueSql("(newid())");
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_Applications_DateInserted");
        });

        modelBuilder.Entity<AspnetMembership>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("PK__aspnet_M__1788CC4D3EA749C6")
                .IsClustered(false);

            entity.HasIndex(e => new { e.ApplicationId, e.LoweredEmail }, "aspnet_Membership_index").IsClustered();

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_Membership_DateInserted");

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetMemberships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Me__Appli__408F9238");

            entity.HasOne(d => d.User).WithOne(p => p.AspnetMembership)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Me__UserI__4183B671");
        });

        modelBuilder.Entity<AspnetPath>(entity =>
        {
            entity.HasKey(e => e.PathId)
                .HasName("PK__aspnet_P__CD67DC5877DFC722")
                .IsClustered(false);

            entity.HasIndex(e => new { e.ApplicationId, e.LoweredPath }, "aspnet_Paths_index")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.PathId).HasDefaultValueSql("(newid())");
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_Paths_DateInserted");

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetPaths)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Pa__Appli__79C80F94");
        });

        modelBuilder.Entity<AspnetPersonalizationAllUser>(entity =>
        {
            entity.HasKey(e => e.PathId).HasName("PK__aspnet_P__CD67DC597F80E8EA");

            entity.Property(e => e.PathId).ValueGeneratedNever();
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_PersonalizationAllUsers_DateInserted");

            entity.HasOne(d => d.Path).WithOne(p => p.AspnetPersonalizationAllUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Pe__PathI__0169315C");
        });

        modelBuilder.Entity<AspnetPersonalizationPerUser>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__aspnet_P__3214EC0604459E07")
                .IsClustered(false);

            entity.HasIndex(e => new { e.PathId, e.UserId }, "aspnet_PersonalizationPerUser_index1")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_PersonalizationPerUser_DateInserted");

            entity.HasOne(d => d.Path).WithMany(p => p.AspnetPersonalizationPerUsers).HasConstraintName("FK__aspnet_Pe__PathI__07220AB2");

            entity.HasOne(d => d.User).WithMany(p => p.AspnetPersonalizationPerUsers).HasConstraintName("FK__aspnet_Pe__UserI__08162EEB");
        });

        modelBuilder.Entity<AspnetProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__aspnet_P__1788CC4C558AAF1E");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_Profile_DateInserted");

            entity.HasOne(d => d.User).WithOne(p => p.AspnetProfile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Pr__UserI__5772F790");
        });

        modelBuilder.Entity<AspnetRole>(entity =>
        {
            entity.HasKey(e => e.RoleId)
                .HasName("PK__aspnet_R__8AFACE1B60FC61CA")
                .IsClustered(false);

            entity.HasIndex(e => new { e.ApplicationId, e.LoweredRoleName }, "aspnet_Roles_index1")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_Roles_DateInserted");

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Ro__Appli__62E4AA3C");
        });

        modelBuilder.Entity<AspnetSchemaVersion>(entity =>
        {
            entity.HasKey(e => new { e.Feature, e.CompatibleSchemaVersion }).HasName("PK__aspnet_S__5A1E6BC1324172E1");

            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_SchemaVersions_DateInserted");
        });

        modelBuilder.Entity<AspnetUser>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("PK__aspnet_U__1788CC4D2AA05119")
                .IsClustered(false);

            entity.HasIndex(e => new { e.ApplicationId, e.LoweredUserName }, "aspnet_Users_Index")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_Users_DateInserted");
            entity.Property(e => e.MobileAlias).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Us__Appli__2C88998B");
        });

        modelBuilder.Entity<AspnetUsersInRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK__aspnet_U__AF2760AD66B53B20");

            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_UsersInRoles_DateInserted");

            entity.HasOne(d => d.Role).WithMany(p => p.AspnetUsersInRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Us__RoleI__6991A7CB");

            entity.HasOne(d => d.User).WithMany(p => p.AspnetUsersInRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Us__UserI__689D8392");
        });

        modelBuilder.Entity<AspnetWebEventEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__aspnet_W__7944C810184C96B4");

            entity.Property(e => e.EventId).IsFixedLength();
            //entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_aspnet_WebEvent_Events_DateInserted");
        });

        modelBuilder.Entity<BaseTariff>(entity =>
        {
            entity.ToTable("BaseTariff", "shared", tb => tb.HasTrigger("trg_for_shared_BaseTariff"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_BaseTariff_DateInserted");
        });

        modelBuilder.Entity<BaseTariff1>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_BaseTariff_DateInserted");
        });

        modelBuilder.Entity<BillingStatus>(entity =>
        {
            entity.Property(e => e.BillingStatusId).ValueGeneratedNever();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_BillingStatus_DateInserted");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Bookings_DateInserted");
        });

        modelBuilder.Entity<Case>(entity =>
        {
            entity.ToTable("Cases", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Cases"));

            entity.Property(e => e.AuthTypeId).HasComment("Speciality");
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Cases_DateInserted");

            entity.HasOne(d => d.Member).WithMany(p => p.Cases).HasConstraintName("FK_Cases_Member");

            entity.HasOne(d => d.ReferFrom).WithMany(p => p.CaseReferFroms).HasConstraintName("FK_Cases_ServiceProvider1");

            entity.HasOne(d => d.ReferTo).WithMany(p => p.CaseReferTos).HasConstraintName("FK_Cases_ServiceProvider");

            entity.HasOne(d => d.Status).WithMany(p => p.Cases).HasConstraintName("FK_Cases_CaseStatus");
        });

        modelBuilder.Entity<CaseCategory>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseCategory_DateInserted");
        });

        modelBuilder.Entity<CaseChecklist>(entity =>
        {
            entity.ToTable("Case_Checklist", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_Checklist"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Case_Checklist_DateInserted");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseChecklists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Checklist_Cases");

            entity.HasOne(d => d.ChecklistTemplate).WithMany(p => p.CaseChecklists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Checklist_ChecklistTemplate");
        });

        modelBuilder.Entity<CaseComment>(entity =>
        {
            entity.ToTable("CaseComment", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_CaseComment"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseComment_DateInserted");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseComments).HasConstraintName("FK_CaseComment_Cases");
        });

        modelBuilder.Entity<CaseCpt>(entity =>
        {
            entity.ToTable("Case_CPT", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_CPT"));

            entity.HasOne(d => d.Case).WithMany(p => p.CaseCpts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_CPT_Cases");

            entity.HasOne(d => d.Cpt).WithMany(p => p.CaseCpts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_CPT_CPT");
        });

        modelBuilder.Entity<CaseDiscount>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Case_Discount_DateInserted");
        });

        modelBuilder.Entity<CaseExclusion>(entity =>
        {
            entity.ToTable("Case_Exclusion", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_Exclusion"));

            entity.HasOne(d => d.Case).WithMany(p => p.CaseExclusions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Exclusion_Cases");

            entity.HasOne(d => d.Exclusion).WithMany(p => p.CaseExclusions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Exclusion_Exclusion");
        });

        modelBuilder.Entity<CaseFacilityType>(entity =>
        {
            entity.ToTable("Case_FacilityType", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_FacilityType"));

            entity.HasOne(d => d.Case).WithMany(p => p.CaseFacilityTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_FacilityType_Cases");

            entity.HasOne(d => d.FacilityType).WithMany(p => p.CaseFacilityTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_FacilityType_FacilityType");
        });

        modelBuilder.Entity<CaseIcd>(entity =>
        {
            entity.ToTable("Case_ICD", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_ICD"));

            entity.HasOne(d => d.Case).WithMany(p => p.CaseIcds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_ICD_Cases");

            entity.HasOne(d => d.Icd).WithMany(p => p.CaseIcds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_ICD_ICD");
        });

        modelBuilder.Entity<CaseLetterNote>(entity =>
        {
            entity.Property(e => e.CaseId).ValueGeneratedNever();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseLetterNote_DateInserted");
        });

        modelBuilder.Entity<CaseLink>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Case_Link_DateInserted");
        });

        modelBuilder.Entity<CaseLink1>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseLink_DateInserted");
        });

        modelBuilder.Entity<CaseLink2>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseLink_DateInserted");
        });

        modelBuilder.Entity<CaseLinkedFile>(entity =>
        {
            entity.ToTable("Case_LinkedFile", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_LinkedFile"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Case_LinkedFile_DateInserted");
        });

        modelBuilder.Entity<CaseManagementAudit>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseManagement_Audit_DateInserted");
            entity.Property(e => e.EventType).IsFixedLength();
        });

        modelBuilder.Entity<CaseManagementAuditDetail>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseManagement_AuditDetail_DateInserted");
        });

        modelBuilder.Entity<CaseNappiCode>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Case_NappiCodes_DateInserted");
        });

        modelBuilder.Entity<CaseNote>(entity =>
        {
            entity.ToTable("CaseNote", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_CaseNote"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseNote_DateInserted");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseNotes).HasConstraintName("FK_CaseNote_Cases");
        });

        modelBuilder.Entity<CaseStatus>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseStatus_DateInserted");
        });

        modelBuilder.Entity<CaseTariff>(entity =>
        {
            entity.Property(e => e.CaseIdTariffId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<CaseType>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CaseType_DateInserted");
        });

        modelBuilder.Entity<ChecklistTemplate>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ChecklistTemplate_DateInserted");
        });

        modelBuilder.Entity<ChronicIllness>(entity =>
        {
            entity.ToTable("ChronicIllness", tb => tb.HasTrigger("trg_for_dbo_ChronicIllness"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ChronicIllness_DateInserted");
        });

        modelBuilder.Entity<ChronicIllness1>(entity =>
        {
            entity.ToTable("ChronicIllness", "shared", tb => tb.HasTrigger("trg_for_shared_ChronicIllness"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ChronicIllness_DateInserted");
        });

        modelBuilder.Entity<ClientUpdate>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ClientUpdates_DateInserted");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", "shared", tb => tb.HasTrigger("trg_for_shared_Country"));

            entity.Property(e => e.CountryCurrencyCode).IsFixedLength();
            entity.Property(e => e.CountryIsocode).IsFixedLength();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Country_DateInserted");
        });

        modelBuilder.Entity<CountryVat>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CountryVAT_DateInserted");
        });

        modelBuilder.Entity<Cpt>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_CPT_DateInserted");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.ToTable("Episode", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Episode"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Episode_DateInserted");
        });

        modelBuilder.Entity<EpisodeCase>(entity =>
        {
            entity.ToTable("Episode_Case", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Episode_Case"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Episode_Case_DateInserted");
        });

        modelBuilder.Entity<Exclusion>(entity =>
        {
            entity.ToTable("Exclusion", "shared", tb => tb.HasTrigger("trg_for_shared_Exclusion"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Exclusion_DateInserted");
        });

        modelBuilder.Entity<FacilityType>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_FacilityType_DateInserted");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Gender_DateInserted");
            entity.Property(e => e.GenderCode).IsFixedLength();
        });

        modelBuilder.Entity<Icd>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ICD_DateInserted");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Language_DateInserted");
        });

        modelBuilder.Entity<LinkedFile>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_LinkedFile_DateInserted");
        });

        modelBuilder.Entity<MainClient>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MainClient_DateInserted");
        });

        modelBuilder.Entity<MainClientMedicalAidProduct>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MainClient_MedicalAidProduct_DateInserted");
        });

        modelBuilder.Entity<MainClientTariff>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MainClient_Tariff_DateInserted");
        });

        modelBuilder.Entity<MarritalStatus>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MarritalStatus_DateInserted");
        });

        modelBuilder.Entity<MedicalAid>(entity =>
        {
            entity.HasKey(e => e.MedicalAidId).HasName("PK_Fund");

            entity.ToTable("MedicalAid", "shared", tb => tb.HasTrigger("trg_for_shared_MedicalAid"));

            entity.Property(e => e.CasePrefix).IsFixedLength();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MedicalAid_DateInserted");
        });

        modelBuilder.Entity<MedicalAidExclusion>(entity =>
        {
            entity.HasKey(e => new { e.MedicalAidId, e.BaseTariffId }).HasName("PK_Fund_Exclusion");

            entity.ToTable("MedicalAid_Exclusion", "shared", tb => tb.HasTrigger("trg_for_shared_MedicalAid_Exclusion"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MedicalAid_Exclusion_DateInserted");

            entity.HasOne(d => d.BaseTariff).WithMany(p => p.MedicalAidExclusions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalAid_Exclusion_BaseTariff");

            entity.HasOne(d => d.MedicalAid).WithMany(p => p.MedicalAidExclusions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalAid_Exclusion_MedicalAid");
        });

        modelBuilder.Entity<MedicalAidProduct>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MedicalAidProduct_DateInserted");
        });

        modelBuilder.Entity<MedicalAidTariff>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MedicalAid_Tariff_DateInserted");

            entity.HasOne(d => d.MedicalAid).WithMany(p => p.MedicalAidTariffs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalAid_Tariff_MedicalAid");

            entity.HasOne(d => d.TariffName).WithMany(p => p.MedicalAidTariffs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalAid_Tariff_TariffName");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_Import_Member");

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Member_DateInserted");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member", "shared", tb => tb.HasTrigger("trg_for_shared_Member"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Member_DateInserted");

            entity.HasOne(d => d.Gender).WithMany(p => p.Members).HasConstraintName("FK_Member_Gender");

            entity.HasOne(d => d.MarritalStatus).WithMany(p => p.Members).HasConstraintName("FK_Member_MarritalStatus");

            entity.HasOne(d => d.MedicalAid).WithMany(p => p.Members).HasConstraintName("FK_Member_MedicalAid");

            entity.HasOne(d => d.MemberCountry).WithMany(p => p.Members).HasConstraintName("FK_Member_Country");

            entity.HasOne(d => d.MemberLanguage).WithMany(p => p.Members).HasConstraintName("FK_Member_Language");

            entity.HasOne(d => d.MemberRace).WithMany(p => p.Members).HasConstraintName("FK_Member_Race");

            entity.HasOne(d => d.MemberStatus).WithMany(p => p.Members).HasConstraintName("FK_Member_MemberStatus");

            entity.HasOne(d => d.PeriodInCountry).WithMany(p => p.Members).HasConstraintName("FK_Member_PeriodInCountry");

            entity.HasOne(d => d.Title).WithMany(p => p.Members).HasConstraintName("FK_Member_Title");
        });

        modelBuilder.Entity<MemberChronicIllness>(entity =>
        {
            entity.ToTable("Member_ChronicIllness", "shared", tb => tb.HasTrigger("trg_for_shared_Member_ChronicIllness"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Member_ChronicIllness_DateInserted");

            entity.HasOne(d => d.ChronicIllness).WithMany(p => p.MemberChronicIllnesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_ChronicIllness_ChronicIllness");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberChronicIllnesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_ChronicIllness_Member");
        });

        modelBuilder.Entity<MemberMedicalAidProduct>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Member_MedicalAidProduct_DateInserted");
            entity.Property(e => e.MedAidProductIdMemberId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<MemberNote>(entity =>
        {
            entity.ToTable("MemberNote", "shared", tb => tb.HasTrigger("trg_for_shared_MemberNote"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MemberNote_DateInserted");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberNotes).HasConstraintName("FK_MemberNote_Member");
        });

        modelBuilder.Entity<MemberStatus>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_MemberStatus_DateInserted");
        });

        modelBuilder.Entity<NappiCode>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_NappiCodes_DateInserted");
        });

        modelBuilder.Entity<PeriodInCountry>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_PeriodInCountry_DateInserted");
        });

        modelBuilder.Entity<Race>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Race_DateInserted");
        });

        modelBuilder.Entity<ServiceProvider>(entity =>
        {
            entity.ToTable("ServiceProvider", "shared", tb => tb.HasTrigger("trg_for_shared_ServiceProvider"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ServiceProvider_DateInserted");

            entity.HasOne(d => d.Speciality).WithMany(p => p.ServiceProviders).HasConstraintName("FK_ServiceProvider_Speciality");
        });

        modelBuilder.Entity<ServiceProviderMainClientDiscount>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ServiceProvider_MainClient_Discount_DateInserted");
        });

        modelBuilder.Entity<ServiceProviderTariff>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ServiceProvider_Tariff_DateInserted");
            entity.Property(e => e.ServiceProviderTariffId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ServiceProviderTariffCustom>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_ServiceProvider_Tariff_Custom_DateInserted");
            entity.Property(e => e.ServiceProviderTariffCustomId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<SessionUserCase>(entity =>
        {
            entity.HasKey(e => e.CaseId).HasName("PK_Session_User_Case_1");

            entity.ToTable("Session_User_Case", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Session_User_Case"));

            entity.Property(e => e.CaseId).ValueGeneratedNever();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Session_User_Case_DateInserted");
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.ToTable("Speciality", "shared", tb => tb.HasTrigger("trg_for_shared_Speciality"));

            entity.Property(e => e.SpecialityId).ValueGeneratedNever();
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Speciality_DateInserted");
        });

        modelBuilder.Entity<SuspendedReason>(entity =>
        {
            entity.HasKey(e => e.SuspendedReasonId).HasName("PK_SuspensionReason");

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_SuspendedReason_DateInserted");
        });

        modelBuilder.Entity<SystemDatum>(entity =>
        {
            entity.ToTable("SystemData", "shared", tb => tb.HasTrigger("trg_for_shared_SystemData"));

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_SystemData_DateInserted");
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.TariffId).HasName("PK_Tariff_1");

            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Tariff_DateInserted");
        });

        modelBuilder.Entity<TariffName>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_TariffName_DateInserted");
        });

        modelBuilder.Entity<TariffName1>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_TariffName_DateInserted");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_Title_DateInserted");
        });

        modelBuilder.Entity<VwAspnetApplication>(entity =>
        {
            entity.ToView("vw_aspnet_Applications");
        });

        modelBuilder.Entity<VwAspnetMembershipUser>(entity =>
        {
            entity.ToView("vw_aspnet_MembershipUsers");
        });

        modelBuilder.Entity<VwAspnetProfile>(entity =>
        {
            entity.ToView("vw_aspnet_Profiles");
        });

        modelBuilder.Entity<VwAspnetRole>(entity =>
        {
            entity.ToView("vw_aspnet_Roles");
        });

        modelBuilder.Entity<VwAspnetUser>(entity =>
        {
            entity.ToView("vw_aspnet_Users");
        });

        modelBuilder.Entity<VwAspnetUsersInRole>(entity =>
        {
            entity.ToView("vw_aspnet_UsersInRoles");
        });

        modelBuilder.Entity<VwAspnetWebPartStatePath>(entity =>
        {
            entity.ToView("vw_aspnet_WebPartState_Paths");
        });

        modelBuilder.Entity<VwAspnetWebPartStateShared>(entity =>
        {
            entity.ToView("vw_aspnet_WebPartState_Shared");
        });

        modelBuilder.Entity<VwAspnetWebPartStateUser>(entity =>
        {
            entity.ToView("vw_aspnet_WebPartState_User");
        });

        modelBuilder.Entity<VwCaseDetail>(entity =>
        {
            entity.ToView("vw_CaseDetail");
        });

        modelBuilder.Entity<VwCaseDetailWithAmount>(entity =>
        {
            entity.ToView("vw_CaseDetail_WithAmounts");

            entity.Property(e => e.CasePrefix).IsFixedLength();
        });

        modelBuilder.Entity<VwServiceProvider>(entity =>
        {
            entity.ToView("vw_ServiceProvider");
        });

        modelBuilder.Entity<XHospitalType>(entity =>
        {
            entity.Property(e => e.DateInserted).HasDefaultValueSql("(getdate())", "DF_xHospitalType_DateInserted");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
