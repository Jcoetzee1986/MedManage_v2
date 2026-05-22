using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedManage.Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation for transaction management and repository access
/// Repositories are lazily initialized only when accessed
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly MedManageDbContext _context;
    private readonly int? _currentMainClientId;
    private IDbContextTransaction? _transaction;

    // Lazy repository fields - Core Business Entities
    private IMemberRepository? _members;
    private ICaseRepository? _cases;
    private IServiceProviderRepository? _serviceProviders;
    private IMedicalAidRepository? _medicalAids;
    private IMedicalAidProductRepository? _medicalAidProducts;
    private IBookingRepository? _bookings;
    private IExclusionRepository? _exclusions;
    private IEpisodeRepository? _episodes;
    
    // Lazy repository fields - Medical Codes
    private ICptRepository? _cptCodes;
    private IIcdRepository? _icdCodes;
    private INappiCodeRepository? _nappiCodes;
    
    // Lazy repository fields - Reference Data
    private IChronicIllnessRepository? _chronicIllnesses;
    private ICountryRepository? _countries;
    private IGenderRepository? _genders;
    private ILanguageRepository? _languages;
    private IMarritalStatusRepository? _marritalStatuses;
    private ITitleRepository? _titles;
    private IRaceRepository? _races;
    private IBillingStatusRepository? _billingStatuses;
    private ICaseStatusRepository? _caseStatuses;
    private ICaseTypeRepository? _caseTypes;
    private IFacilityTypeRepository? _facilityTypes;
    private IMemberStatusRepository? _memberStatuses;
    private ISpecialityRepository? _specialities;
    private IChecklistTemplateRepository? _checklistTemplates;
    
    // Lazy repository fields - Case-Related
    private ICaseBillingRepository? _caseBillings;
    private ICaseChecklistRepository? _caseChecklists;
    private ICaseCommentRepository? _caseComments;
    private ICaseCptRepository? _caseCpts;
    private ICaseDiscountRepository? _caseDiscounts;
    private ICaseExclusionRepository? _caseExclusions;
    private ICaseFacilityTypeRepository? _caseFacilityTypes;
    private ICaseIcdRepository? _caseIcds;
    private ICaseLetterNoteRepository? _caseLetterNotes;
    private ICaseLinkRepository? _caseLinks;
    private ICaseLinkedFileRepository? _caseLinkedFiles;
    private ICaseNappiCodeRepository? _caseNappiCodes;
    private ICaseNoteRepository? _caseNotes;
    private ICaseTariffRepository? _caseTariffs;
    
    // Lazy repository fields - Member-Related
    private IMemberChronicIllnessRepository? _memberChronicIllnesses;
    private IMemberMedicalAidProductRepository? _memberMedicalAidProducts;
    private IMemberNoteRepository? _memberNotes;
    
    // Lazy repository fields - Tariffs
    private IBaseTariffRepository? _baseTariffs;
    private IServiceProviderTariffRepository? _serviceProviderTariffs;
    private IServiceProviderTariffCustomRepository? _serviceProviderTariffCustoms;
    
    // Lazy repository fields - Other
    private ILinkedFileRepository? _linkedFiles;
    private IMedicalAidExclusionRepository? _medicalAidExclusions;
    private IEpisodeCaseRepository? _episodeCases;
    private ISessionUserCaseRepository? _sessionUserCases;

    public UnitOfWork(MedManageDbContext context, int? currentMainClientId = null)
    {
        _context = context;
        _currentMainClientId = currentMainClientId;
    }

    // Core Business Entity Repository Properties
    public IMemberRepository Members => _members ??= new MemberRepository(_context);
    public ICaseRepository Cases => _cases ??= new CaseRepository(_context);
    public IServiceProviderRepository ServiceProviders => _serviceProviders ??= new ServiceProviderRepository(_context);
    public IMedicalAidRepository MedicalAids => _medicalAids ??= new MedicalAidRepository(_context);
    public IMedicalAidProductRepository MedicalAidProducts => _medicalAidProducts ??= new MedicalAidProductRepository(_context);
    public IBookingRepository Bookings => _bookings ??= new BookingRepository(_context);
    public IExclusionRepository Exclusions => _exclusions ??= new ExclusionRepository(_context);
    public IEpisodeRepository Episodes => _episodes ??= new EpisodeRepository(_context);
    
    // Medical Code Repository Properties
    public ICptRepository CptCodes => _cptCodes ??= new CptRepository(_context);
    public IIcdRepository IcdCodes => _icdCodes ??= new IcdRepository(_context);
    public INappiCodeRepository NappiCodes => _nappiCodes ??= new NappiCodeRepository(_context);
    
    // Reference Data Repository Properties
    public IChronicIllnessRepository ChronicIllnesses => _chronicIllnesses ??= new ChronicIllnessRepository(_context);
    public ICountryRepository Countries => _countries ??= new CountryRepository(_context);
    public IGenderRepository Genders => _genders ??= new GenderRepository(_context);
    public ILanguageRepository Languages => _languages ??= new LanguageRepository(_context);
    public IMarritalStatusRepository MarritalStatuses => _marritalStatuses ??= new MarritalStatusRepository(_context);
    public ITitleRepository Titles => _titles ??= new TitleRepository(_context);
    public IRaceRepository Races => _races ??= new RaceRepository(_context);
    public IBillingStatusRepository BillingStatuses => _billingStatuses ??= new BillingStatusRepository(_context);
    public ICaseStatusRepository CaseStatuses => _caseStatuses ??= new CaseStatusRepository(_context);
    public ICaseTypeRepository CaseTypes => _caseTypes ??= new CaseTypeRepository(_context);
    public IFacilityTypeRepository FacilityTypes => _facilityTypes ??= new FacilityTypeRepository(_context);
    public IMemberStatusRepository MemberStatuses => _memberStatuses ??= new MemberStatusRepository(_context);
    public ISpecialityRepository Specialities => _specialities ??= new SpecialityRepository(_context);
    public IChecklistTemplateRepository ChecklistTemplates => _checklistTemplates ??= new ChecklistTemplateRepository(_context);
    
    // Case-Related Repository Properties
    public ICaseBillingRepository CaseBillings => _caseBillings ??= new CaseBillingRepository(_context);
    public ICaseChecklistRepository CaseChecklists => _caseChecklists ??= new CaseChecklistRepository(_context);
    public ICaseCommentRepository CaseComments => _caseComments ??= new CaseCommentRepository(_context);
    public ICaseCptRepository CaseCpts => _caseCpts ??= new CaseCptRepository(_context);
    public ICaseDiscountRepository CaseDiscounts => _caseDiscounts ??= new CaseDiscountRepository(_context);
    public ICaseExclusionRepository CaseExclusions => _caseExclusions ??= new CaseExclusionRepository(_context);
    public ICaseFacilityTypeRepository CaseFacilityTypes => _caseFacilityTypes ??= new CaseFacilityTypeRepository(_context);
    public ICaseIcdRepository CaseIcds => _caseIcds ??= new CaseIcdRepository(_context);
    public ICaseLetterNoteRepository CaseLetterNotes => _caseLetterNotes ??= new CaseLetterNoteRepository(_context);
    public ICaseLinkRepository CaseLinks => _caseLinks ??= new CaseLinkRepository(_context);
    public ICaseLinkedFileRepository CaseLinkedFiles => _caseLinkedFiles ??= new CaseLinkedFileRepository(_context);
    public ICaseNappiCodeRepository CaseNappiCodes => _caseNappiCodes ??= new CaseNappiCodeRepository(_context);
    public ICaseNoteRepository CaseNotes => _caseNotes ??= new CaseNoteRepository(_context);
    public ICaseTariffRepository CaseTariffs => _caseTariffs ??= new CaseTariffRepository(_context);
    
    // Member-Related Repository Properties
    public IMemberChronicIllnessRepository MemberChronicIllnesses => _memberChronicIllnesses ??= new MemberChronicIllnessRepository(_context);
    public IMemberMedicalAidProductRepository MemberMedicalAidProducts => _memberMedicalAidProducts ??= new MemberMedicalAidProductRepository(_context);
    public IMemberNoteRepository MemberNotes => _memberNotes ??= new MemberNoteRepository(_context);
    
    // Tariff Repository Properties
    public IBaseTariffRepository BaseTariffs => _baseTariffs ??= new BaseTariffRepository(_context);
    public IServiceProviderTariffRepository ServiceProviderTariffs => _serviceProviderTariffs ??= new ServiceProviderTariffRepository(_context);
    public IServiceProviderTariffCustomRepository ServiceProviderTariffCustoms => _serviceProviderTariffCustoms ??= new ServiceProviderTariffCustomRepository(_context);
    
    // Other Repository Properties
    public ILinkedFileRepository LinkedFiles => _linkedFiles ??= new LinkedFileRepository(_context);
    public IMedicalAidExclusionRepository MedicalAidExclusions => _medicalAidExclusions ??= new MedicalAidExclusionRepository(_context);
    public IEpisodeCaseRepository EpisodeCases => _episodeCases ??= new EpisodeCaseRepository(_context);
    public ISessionUserCaseRepository SessionUserCases => _sessionUserCases ??= new SessionUserCaseRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            await (_transaction?.CommitAsync() ?? Task.CompletedTask);
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        await (_transaction?.RollbackAsync() ?? Task.CompletedTask);
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        // Note: Don't dispose _context here as it's managed by DI container
    }
}
