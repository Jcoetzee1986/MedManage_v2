using MedManage.Core.Interfaces.Repositories;

namespace MedManage.Core.Interfaces;

/// <summary>
/// Unit of Work pattern for managing database transactions and repository access
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Core Business Entity Repositories
    IMemberRepository Members { get; }
    ICaseRepository Cases { get; }
    IServiceProviderRepository ServiceProviders { get; }
    IMedicalAidRepository MedicalAids { get; }
    IMedicalAidProductRepository MedicalAidProducts { get; }
    IBookingRepository Bookings { get; }
    IExclusionRepository Exclusions { get; }
    IEpisodeRepository Episodes { get; }
    
    // Medical Code Repositories
    ICptRepository CptCodes { get; }
    IIcdRepository IcdCodes { get; }
    INappiCodeRepository NappiCodes { get; }
    
    // Reference Data Repositories
    IChronicIllnessRepository ChronicIllnesses { get; }
    ICountryRepository Countries { get; }
    IGenderRepository Genders { get; }
    ILanguageRepository Languages { get; }
    IMarritalStatusRepository MarritalStatuses { get; }
    ITitleRepository Titles { get; }
    IRaceRepository Races { get; }
    IBillingStatusRepository BillingStatuses { get; }
    ICaseStatusRepository CaseStatuses { get; }
    ICaseTypeRepository CaseTypes { get; }
    IFacilityTypeRepository FacilityTypes { get; }
    IMemberStatusRepository MemberStatuses { get; }
    ISpecialityRepository Specialities { get; }
    IChecklistTemplateRepository ChecklistTemplates { get; }
    
    // Case-Related Repositories
    ICaseBillingRepository CaseBillings { get; }
    ICaseChecklistRepository CaseChecklists { get; }
    ICaseCommentRepository CaseComments { get; }
    ICaseCptRepository CaseCpts { get; }
    ICaseDiscountRepository CaseDiscounts { get; }
    ICaseExclusionRepository CaseExclusions { get; }
    ICaseFacilityTypeRepository CaseFacilityTypes { get; }
    ICaseIcdRepository CaseIcds { get; }
    ICaseLetterNoteRepository CaseLetterNotes { get; }
    ICaseLinkRepository CaseLinks { get; }
    ICaseLinkedFileRepository CaseLinkedFiles { get; }
    ICaseNappiCodeRepository CaseNappiCodes { get; }
    ICaseNoteRepository CaseNotes { get; }
    ICaseTariffRepository CaseTariffs { get; }
    
    // Member-Related Repositories
    IMemberChronicIllnessRepository MemberChronicIllnesses { get; }
    IMemberMedicalAidProductRepository MemberMedicalAidProducts { get; }
    IMemberNoteRepository MemberNotes { get; }
    
    // Tariff Repositories
    IBaseTariffRepository BaseTariffs { get; }
    IServiceProviderTariffRepository ServiceProviderTariffs { get; }
    IServiceProviderTariffCustomRepository ServiceProviderTariffCustoms { get; }
    
    // Other Repositories
    ILinkedFileRepository LinkedFiles { get; }
    IMedicalAidExclusionRepository MedicalAidExclusions { get; }
    IEpisodeCaseRepository EpisodeCases { get; }
    ISessionUserCaseRepository SessionUserCases { get; }
    
    /// <summary>
    /// Saves all pending changes to the database
    /// </summary>
    /// <returns>Number of entities written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    Task BeginTransactionAsync();
    
    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync();
    
    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync();
}
