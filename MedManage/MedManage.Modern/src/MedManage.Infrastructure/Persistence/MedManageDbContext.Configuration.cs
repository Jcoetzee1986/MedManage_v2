using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Persistence;

/// <summary>
/// Partial class containing Entity Framework model configurations
/// Generated from database scaffolding
/// </summary>
public partial class MedManageDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllCodesDatum>(entity =>
        {
            entity.Property(e => e.CasePrefix).IsFixedLength();
            entity.Property(e => e.RecordKey).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AspnetApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId)
                .HasName("PK__aspnet_A__C93A4C982022C2A6")
                .IsClustered(false);

            entity.HasIndex(e => e.LoweredApplicationName, "aspnet_Applications_Index").IsClustered();

            entity.Property(e => e.ApplicationId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<AspnetMembership>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("PK__aspnet_M__1788CC4D3EA749C6")
                .IsClustered(false);

            entity.HasIndex(e => new { e.ApplicationId, e.LoweredEmail }, "aspnet_Membership_index").IsClustered();

            entity.Property(e => e.UserId).ValueGeneratedNever();

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

            entity.HasIndex(e => new  { e.ApplicationId, e.LoweredPath }, "aspnet_Paths_index")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.PathId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetPaths)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Pa__Appli__79C80F94");
        });

        modelBuilder.Entity<AspnetPersonalizationAllUser>(entity =>
        {
            entity.HasKey(e => e.PathId).HasName("PK__aspnet_P__CD67DC597F80E8EA");

            entity.Property(e => e.PathId).ValueGeneratedNever();

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

            entity.HasOne(d => d.Path).WithMany(p => p.AspnetPersonalizationPerUsers).HasConstraintName("FK__aspnet_Pe__PathI__07220AB2");

            entity.HasOne(d => d.User).WithMany(p => p.AspnetPersonalizationPerUsers).HasConstraintName("FK__aspnet_Pe__UserI__08162EEB");
        });

        modelBuilder.Entity<AspnetProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__aspnet_P__1788CC4C558AAF1E");

            entity.Property(e => e.UserId).ValueGeneratedNever();

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

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Ro__Appli__62E4AA3C");
        });

        modelBuilder.Entity<AspnetSchemaVersion>(entity =>
        {
            entity.HasKey(e => new { e.Feature, e.CompatibleSchemaVersion }).HasName("PK__aspnet_S__5A1E6BC1324172E1");
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
            entity.Property(e => e.MobileAlias).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Application).WithMany(p => p.AspnetUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__aspnet_Us__Appli__2C88998B");
        });

        modelBuilder.Entity<AspnetWebEventEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__aspnet_W__7944C810184C96B4");

            entity.Property(e => e.EventId).IsFixedLength();
        });

        modelBuilder.Entity<BaseTariff>(entity =>
        {
            entity.ToTable("BaseTariff", "shared", tb => tb.HasTrigger("trg_for_shared_BaseTariff"));
        });

        modelBuilder.Entity<BillingStatus>(entity =>
        {
            entity.Property(e => e.BillingStatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Case>(entity =>
        {
            entity.ToTable("Cases", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Cases"));

            entity.Property(e => e.AuthTypeId).HasComment("Speciality");

            entity.HasOne(d => d.Member).WithMany(p => p.Cases).HasConstraintName("FK_Cases_Member");

            entity.HasOne(d => d.ReferFrom).WithMany(p => p.CaseReferFroms).HasConstraintName("FK_Cases_ServiceProvider1");

            entity.HasOne(d => d.ReferTo).WithMany(p => p.CaseReferTos).HasConstraintName("FK_Cases_ServiceProvider");

            entity.HasOne(d => d.Status).WithMany(p => p.Cases).HasConstraintName("FK_Cases_CaseStatus");
        });

        modelBuilder.Entity<CaseChecklist>(entity =>
        {
            entity.ToTable("Case_Checklist", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_Checklist"));

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
        });

        modelBuilder.Entity<CaseLinkedFile>(entity =>
        {
            entity.ToTable("Case_LinkedFile", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Case_LinkedFile"));
        });

        modelBuilder.Entity<CaseManagementAudit>(entity =>
        {
            entity.Property(e => e.EventType).IsFixedLength();
        });

        modelBuilder.Entity<CaseNote>(entity =>
        {
            entity.ToTable("CaseNote", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_CaseNote"));

            entity.HasOne(d => d.Case).WithMany(p => p.CaseNotes).HasConstraintName("FK_CaseNote_Cases");
        });

        modelBuilder.Entity<CaseTariff>(entity =>
        {
            entity.HasKey(e => e.CaseIdTariffId);
            entity.Property(e => e.CaseIdTariffId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChronicIllness>(entity =>
        {
            entity.ToTable("ChronicIllness", tb => tb.HasTrigger("trg_for_dbo_ChronicIllness"));
        });

        modelBuilder.Entity<ChronicIllness1>(entity =>
        {
            entity.ToTable("ChronicIllness", "shared", tb => tb.HasTrigger("trg_for_shared_ChronicIllness"));
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", "shared", tb => tb.HasTrigger("trg_for_shared_Country"));

            entity.Property(e => e.CountryCurrencyCode).IsFixedLength();
            entity.Property(e => e.CountryIsocode).IsFixedLength();
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.ToTable("Episode", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Episode"));
        });

        modelBuilder.Entity<EpisodeCase>(entity =>
        {
            entity.ToTable("Episode_Case", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Episode_Case"));
        });

        modelBuilder.Entity<Exclusion>(entity =>
        {
            entity.ToTable("Exclusion", "shared", tb => tb.HasTrigger("trg_for_shared_Exclusion"));
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.Property(e => e.GenderCode).IsFixedLength();
        });

        modelBuilder.Entity<MedicalAid>(entity =>
        {
            entity.HasKey(e => e.MedicalAidId).HasName("PK_Fund");

            entity.ToTable("MedicalAid", "shared", tb => tb.HasTrigger("trg_for_shared_MedicalAid"));

            entity.Property(e => e.CasePrefix).IsFixedLength();
        });

        modelBuilder.Entity<MedicalAidExclusion>(entity =>
        {
            entity.HasKey(e => new { e.MedicalAidId, e.BaseTariffId }).HasName("PK_Fund_Exclusion");

            entity.ToTable("MedicalAid_Exclusion", "shared", tb => tb.HasTrigger("trg_for_shared_MedicalAid_Exclusion"));

            entity.HasOne(d => d.BaseTariff).WithMany(p => p.MedicalAidExclusions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalAid_Exclusion_BaseTariff");

            entity.HasOne(d => d.MedicalAid).WithMany(p => p.MedicalAidExclusions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalAid_Exclusion_MedicalAid");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_Import_Member");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member", "shared", tb => tb.HasTrigger("trg_for_shared_Member"));

            // Configure identity and database-generated columns
            entity.Property(e => e.MemberId).ValueGeneratedOnAdd();
            entity.Property(e => e.DateInserted).ValueGeneratedOnAddOrUpdate().Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

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

            entity.HasOne(d => d.ChronicIllness).WithMany(p => p.MemberChronicIllnesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_ChronicIllness_ChronicIllness");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberChronicIllnesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_ChronicIllness_Member");
        });

        modelBuilder.Entity<MemberMedicalAidProduct>(entity =>
        {
            entity.Property(e => e.MedAidProductIdMemberId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<MemberNote>(entity =>
        {
            entity.ToTable("MemberNote", "shared", tb => tb.HasTrigger("trg_for_shared_MemberNote"));

            entity.HasOne(d => d.Member).WithMany(p => p.MemberNotes).HasConstraintName("FK_MemberNote_Member");
        });

        modelBuilder.Entity<ServiceProvider>(entity =>
        {
            entity.ToTable("ServiceProvider", "shared", tb => tb.HasTrigger("trg_for_shared_ServiceProvider"));

            entity.HasOne(d => d.Speciality).WithMany(p => p.ServiceProviders).HasConstraintName("FK_ServiceProvider_Speciality");
        });

        modelBuilder.Entity<ServiceProviderTariff>(entity =>
        {
            entity.Property(e => e.ServiceProviderTariffId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ServiceProviderTariffCustom>(entity =>
        {
            entity.Property(e => e.ServiceProviderTariffCustomId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<SessionUserCase>(entity =>
        {
            entity.HasKey(e => e.CaseId).HasName("PK_Session_User_Case_1");

            entity.ToTable("Session_User_Case", "CaseManagement", tb => tb.HasTrigger("trg_for_CaseManagement_Session_User_Case"));

            entity.Property(e => e.CaseId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.ToTable("Speciality", "shared", tb => tb.HasTrigger("trg_for_shared_Speciality"));

            entity.Property(e => e.SpecialityId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SuspendedReason>(entity =>
        {
            entity.HasKey(e => e.SuspendedReasonId).HasName("PK_SuspensionReason");
        });

        modelBuilder.Entity<SystemDatum>(entity =>
        {
            entity.ToTable("SystemData", "shared", tb => tb.HasTrigger("trg_for_shared_SystemData"));
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.TariffId).HasName("PK_Tariff_1");
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

        // Apply global query filter for soft delete on all entities that inherit from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.DateDeleted));
                var condition = System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(null, typeof(DateTime?)));
                var lambda = System.Linq.Expressions.Expression.Lambda(condition, parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}
