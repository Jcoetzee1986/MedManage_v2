using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MedManage.Core.Interfaces;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Persistence;

/// <summary>
/// Main DbContext for MedManage application
/// Scaffold command: dotnet ef dbcontext scaffold "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -ContextDir Persistence -Context MedManageDbContext -Force --data-annotations
/// </summary>
public partial class MedManageDbContext : DbContext
{
    private readonly int? _currentMainClientId;
    private readonly ICurrentUserService? _currentUserService;

    public MedManageDbContext(DbContextOptions<MedManageDbContext> options, ICurrentUserService? currentUserService = null, int? currentMainClientId = null)
        : base(options)
    {
        _currentUserService = currentUserService;
        _currentMainClientId = currentMainClientId;
    }

    // DbSets generated from database scaffolding
    // public virtual DbSet<AllCodesDatum> AllCodesData { get; set; } // Table does not exist in database
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
    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
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
        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set audit fields before saving
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var currentUserId = _currentUserService?.UserId;

        foreach (var entry in entries)
        {
            if (entry.Entity is Core.Entities.BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.DateInserted = DateTime.UtcNow;
                    baseEntity.UserID = currentUserId ?? "SYSTEM";
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.DateUpdated = DateTime.UtcNow;
                    baseEntity.UpdatedUserID = currentUserId;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
