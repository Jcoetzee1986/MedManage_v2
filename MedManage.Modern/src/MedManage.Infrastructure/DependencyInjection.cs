using System.Threading.Channels;
using MedManage.Core.Configuration;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Entities;
using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Repositories;
using MedManage.Infrastructure.Services;
using MedManage.Infrastructure.Services.Business;
using Microsoft.Extensions.DependencyInjection;

namespace MedManage.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register tariff percentage background processing channel
        services.AddTariffPercentageChannel();
        
        // Register IEntityMapper implementations for reference data types
        services.AddSingleton<IEntityMapper<MarritalStatus, MarritalStatusDto, CreateMarritalStatusDto, UpdateMarritalStatusDto>, MarritalStatusMapper>();
        services.AddSingleton<IEntityMapper<MemberStatus, MemberStatusDto, CreateMemberStatusDto, UpdateMemberStatusDto>, MemberStatusMapper>();
        services.AddSingleton<IEntityMapper<BillingStatus, BillingStatusDto, CreateBillingStatusDto, UpdateBillingStatusDto>, BillingStatusMapper>();
        services.AddSingleton<IEntityMapper<CaseStatus, CaseStatusDto, CreateCaseStatusDto, UpdateCaseStatusDto>, CaseStatusMapper>();
        services.AddSingleton<IEntityMapper<CaseType, CaseTypeDto, CreateCaseTypeDto, UpdateCaseTypeDto>, CaseTypeMapper>();
        services.AddSingleton<IEntityMapper<FacilityType, FacilityTypeDto, CreateFacilityTypeDto, UpdateFacilityTypeDto>, FacilityTypeMapper>();
        services.AddSingleton<IEntityMapper<Speciality, SpecialityDto, CreateSpecialityDto, UpdateSpecialityDto>, SpecialityMapper>();
        services.AddSingleton<IEntityMapper<ChronicIllness, ChronicIllnessDto, CreateChronicIllnessDto, UpdateChronicIllnessDto>, ChronicIllnessMapper>();
        services.AddSingleton<IEntityMapper<ChecklistTemplate, ChecklistTemplateDto, CreateChecklistTemplateDto, UpdateChecklistTemplateDto>, ChecklistTemplateMapper>();
        services.AddSingleton<IEntityMapper<Country, CountryDto, CreateCountryDto, UpdateCountryDto>, CountryMapper>();
        services.AddSingleton<IEntityMapper<Gender, GenderDto, CreateGenderDto, UpdateGenderDto>, GenderMapper>();
        services.AddSingleton<IEntityMapper<Language, LanguageDto, CreateLanguageDto, UpdateLanguageDto>, LanguageMapper>();
        services.AddSingleton<IEntityMapper<Race, RaceDto, CreateRaceDto, UpdateRaceDto>, RaceMapper>();
        services.AddSingleton<IEntityMapper<Title, TitleDto, CreateTitleDto, UpdateTitleDto>, TitleMapper>();
        services.AddSingleton<IEntityMapper<PeriodInCountry, PeriodInCountryDto, CreatePeriodInCountryDto, UpdatePeriodInCountryDto>, PeriodInCountryMapper>();
        services.AddSingleton<IEntityMapper<CaseCategory, CaseCategoryDto, CreateCaseCategoryDto, UpdateCaseCategoryDto>, CaseCategoryMapper>();
        services.AddSingleton<IEntityMapper<SuspendedReason, SuspendedReasonDto, CreateSuspendedReasonDto, UpdateSuspendedReasonDto>, SuspendedReasonMapper>();
        services.AddSingleton<IEntityMapper<Exclusion, ExclusionDto, CreateExclusionDto, UpdateExclusionDto>, ExclusionMapper>();
        
        // Add Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Add Repositories
        services.AddRepositories();
        
        // Add Business Services
        services.AddBusinessServices();
        
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Base repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Reference Data Repositories
        services.AddScoped<IChronicIllnessRepository, ChronicIllnessRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IMarritalStatusRepository, MarritalStatusRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();
        services.AddScoped<IRaceRepository, RaceRepository>();
        services.AddScoped<IBillingStatusRepository, BillingStatusRepository>();
        services.AddScoped<ICaseStatusRepository, CaseStatusRepository>();
        services.AddScoped<ICaseTypeRepository, CaseTypeRepository>();
        services.AddScoped<IFacilityTypeRepository, FacilityTypeRepository>();
        services.AddScoped<IMemberStatusRepository, MemberStatusRepository>();
        services.AddScoped<ISpecialityRepository, SpecialityRepository>();
        services.AddScoped<IChecklistTemplateRepository, ChecklistTemplateRepository>();
        
        // Medical Code Repositories
        services.AddScoped<ICptRepository, CptRepository>();
        services.AddScoped<IIcdRepository, IcdRepository>();
        services.AddScoped<INappiCodeRepository, NappiCodeRepository>();
        
        // Core Business Entity Repositories
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<ICaseRepository, CaseRepository>();
        services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
        services.AddScoped<IMedicalAidRepository, MedicalAidRepository>();
        services.AddScoped<IMedicalAidProductRepository, MedicalAidProductRepository>();
        services.AddScoped<IExclusionRepository, ExclusionRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        
        // Case-Related Repositories
        services.AddScoped<ICaseBillingRepository, CaseBillingRepository>();
        services.AddScoped<ICaseChecklistRepository, CaseChecklistRepository>();
        services.AddScoped<ICaseCommentRepository, CaseCommentRepository>();
        services.AddScoped<ICaseCptRepository, CaseCptRepository>();
        services.AddScoped<ICaseDiscountRepository, CaseDiscountRepository>();
        services.AddScoped<ICaseExclusionRepository, CaseExclusionRepository>();
        services.AddScoped<ICaseFacilityTypeRepository, CaseFacilityTypeRepository>();
        services.AddScoped<ICaseIcdRepository, CaseIcdRepository>();
        services.AddScoped<ICaseLetterNoteRepository, CaseLetterNoteRepository>();
        services.AddScoped<ICaseLinkRepository, CaseLinkRepository>();
        services.AddScoped<ICaseLinkedFileRepository, CaseLinkedFileRepository>();
        services.AddScoped<ICaseNappiCodeRepository, CaseNappiCodeRepository>();
        services.AddScoped<ICaseNoteRepository, CaseNoteRepository>();
        services.AddScoped<ICaseTariffRepository, CaseTariffRepository>();
        
        // Member-Related Repositories
        services.AddScoped<IMemberChronicIllnessRepository, MemberChronicIllnessRepository>();
        services.AddScoped<IMemberMedicalAidProductRepository, MemberMedicalAidProductRepository>();
        services.AddScoped<IMemberNoteRepository, MemberNoteRepository>();
        
        // Tariff Repositories
        services.AddScoped<IBaseTariffRepository, BaseTariffRepository>();
        services.AddScoped<IServiceProviderTariffRepository, ServiceProviderTariffRepository>();
        services.AddScoped<IServiceProviderTariffCustomRepository, ServiceProviderTariffCustomRepository>();
        
        // Service Provider Sub-Entity Repositories
        services.AddScoped<IServiceProviderDiscountRepository, ServiceProviderDiscountRepository>();
        
        // Other Business Repositories
        services.AddScoped<ILinkedFileRepository, LinkedFileRepository>();
        services.AddScoped<IMedicalAidExclusionRepository, MedicalAidExclusionRepository>();
        services.AddScoped<IMedicalAidTariffRepository, MedicalAidTariffRepository>();
        services.AddScoped<IEpisodeCaseRepository, EpisodeCaseRepository>();
        services.AddScoped<ISessionUserCaseRepository, SessionUserCaseRepository>();

        return services;
    }
    
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Core Business Services
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<ICaseBusinessRuleService, CaseBusinessRuleService>();
        services.AddScoped<ICaseCopyService, CaseCopyService>();
        services.AddScoped<ICaseLockService, CaseLockService>();
        services.AddScoped<ICaseWorkflowService, CaseWorkflowService>();
        services.AddScoped<IServiceProviderService, ServiceProviderService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IEpisodeService, EpisodeService>();
        services.AddScoped<IMedicalAidService, MedicalAidService>();
        services.AddScoped<IExclusionService, ExclusionService>();
        services.AddScoped<IMemberNoteService, MemberNoteService>();
        services.AddScoped<IMemberChronicIllnessService, MemberChronicIllnessService>();
        services.AddScoped<IMemberMedicalAidProductService, MemberMedicalAidProductService>();
        services.AddScoped<ICaseNoteService, CaseNoteService>();
        services.AddScoped<ICaseCommentService, CaseCommentService>();
        services.AddScoped<ICaseLetterNoteService, CaseLetterNoteService>();
        services.AddScoped<ICaseLinkedFileService, CaseLinkedFileService>();
        services.AddScoped<ICaseBillingService, CaseBillingService>();
        services.AddScoped<ICaseDiscountService, CaseDiscountService>();
        services.AddScoped<ICaseChecklistService, CaseChecklistService>();
        services.AddScoped<ICaseCptService, CaseCptService>();
        services.AddScoped<ICaseExclusionService, CaseExclusionService>();
        services.AddScoped<ICaseFacilityTypeService, CaseFacilityTypeService>();
        services.AddScoped<ICaseIcdService, CaseIcdService>();
        services.AddScoped<ICaseTariffService, CaseTariffService>();
        services.AddScoped<ICaseLinkService, CaseLinkService>();
        services.AddScoped<ICaseNappiService, CaseNappiService>();
        services.AddScoped<IEpisodeCaseService, EpisodeCaseService>();
        
        // Tariff Services
        services.AddScoped<ITariffService, TariffService>();
        services.AddScoped<ITariffPercentageService, TariffPercentageService>();
        
        // Code Lookup Service
        services.AddScoped<ICodeLookupService, CodeLookupService>();
        
        // Admin Services
        services.AddScoped<ISystemDataService, SystemDataService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        
        // Import Service
        services.AddScoped<IImportService, ImportService>();
        
        // Document Management Service
        services.AddScoped<IDocumentService, DocumentService>();

        // Billing Comments Service
        services.AddScoped<ICaseBillingCommentService, CaseBillingCommentService>();
        
        // MainClient Tariff Service
        services.AddScoped<IMainClientTariffService, MainClientTariffService>();
        
        // Session Admin Service
        services.AddScoped<ISessionAdminService, SessionAdminService>();
        
        // Dashboard Service
        services.AddScoped<IDashboardService, DashboardService>();

        // Letter Template Service (HTML templates + PDF generation)
        services.AddScoped<ILetterTemplateService, LetterTemplateService>();

        // Report Generation Service (ClosedXML + PuppeteerSharp)
        services.AddScoped<IReportGenerationService, ReportGenerationService>();
        
        // Reference Data Services
        services.AddScoped<IMarritalStatusService, MarritalStatusService>();
        services.AddScoped<IMemberStatusService, MemberStatusService>();
        services.AddScoped<IBillingStatusService, BillingStatusService>();
        services.AddScoped<ICaseStatusService, CaseStatusService>();
        services.AddScoped<ICaseTypeService, CaseTypeService>();
        services.AddScoped<IFacilityTypeService, FacilityTypeService>();
        services.AddScoped<ISpecialityService, SpecialityService>();
        services.AddScoped<IChronicIllnessService, ChronicIllnessService>();
        services.AddScoped<IChecklistTemplateService, ChecklistTemplateService>();
        services.AddScoped<ICountryService, CountryService>();
        
        // Generic Reference Data Services (using ReferenceDataService<TEntity, TDto, TCreateDto, TUpdateDto>)
        services.AddScoped<IReferenceDataService<GenderDto, CreateGenderDto, UpdateGenderDto>,
            ReferenceDataService<Gender, GenderDto, CreateGenderDto, UpdateGenderDto>>();
        services.AddScoped<IReferenceDataService<LanguageDto, CreateLanguageDto, UpdateLanguageDto>,
            ReferenceDataService<Language, LanguageDto, CreateLanguageDto, UpdateLanguageDto>>();
        services.AddScoped<IReferenceDataService<RaceDto, CreateRaceDto, UpdateRaceDto>,
            ReferenceDataService<Race, RaceDto, CreateRaceDto, UpdateRaceDto>>();
        services.AddScoped<IReferenceDataService<TitleDto, CreateTitleDto, UpdateTitleDto>,
            ReferenceDataService<Title, TitleDto, CreateTitleDto, UpdateTitleDto>>();
        services.AddScoped<IReferenceDataService<PeriodInCountryDto, CreatePeriodInCountryDto, UpdatePeriodInCountryDto>,
            ReferenceDataService<PeriodInCountry, PeriodInCountryDto, CreatePeriodInCountryDto, UpdatePeriodInCountryDto>>();
        services.AddScoped<IReferenceDataService<CaseCategoryDto, CreateCaseCategoryDto, UpdateCaseCategoryDto>,
            ReferenceDataService<CaseCategory, CaseCategoryDto, CreateCaseCategoryDto, UpdateCaseCategoryDto>>();
        services.AddScoped<IReferenceDataService<SuspendedReasonDto, CreateSuspendedReasonDto, UpdateSuspendedReasonDto>,
            ReferenceDataService<SuspendedReason, SuspendedReasonDto, CreateSuspendedReasonDto, UpdateSuspendedReasonDto>>();
        services.AddScoped<IReferenceDataService<MarritalStatusDto, CreateMarritalStatusDto, UpdateMarritalStatusDto>,
            ReferenceDataService<MarritalStatus, MarritalStatusDto, CreateMarritalStatusDto, UpdateMarritalStatusDto>>();
        services.AddScoped<IReferenceDataService<MemberStatusDto, CreateMemberStatusDto, UpdateMemberStatusDto>,
            ReferenceDataService<MemberStatus, MemberStatusDto, CreateMemberStatusDto, UpdateMemberStatusDto>>();
        services.AddScoped<IReferenceDataService<BillingStatusDto, CreateBillingStatusDto, UpdateBillingStatusDto>,
            ReferenceDataService<BillingStatus, BillingStatusDto, CreateBillingStatusDto, UpdateBillingStatusDto>>();
        services.AddScoped<IReferenceDataService<CaseStatusDto, CreateCaseStatusDto, UpdateCaseStatusDto>,
            ReferenceDataService<CaseStatus, CaseStatusDto, CreateCaseStatusDto, UpdateCaseStatusDto>>();
        services.AddScoped<IReferenceDataService<CaseTypeDto, CreateCaseTypeDto, UpdateCaseTypeDto>,
            ReferenceDataService<CaseType, CaseTypeDto, CreateCaseTypeDto, UpdateCaseTypeDto>>();
        services.AddScoped<IReferenceDataService<FacilityTypeDto, CreateFacilityTypeDto, UpdateFacilityTypeDto>,
            ReferenceDataService<FacilityType, FacilityTypeDto, CreateFacilityTypeDto, UpdateFacilityTypeDto>>();
        services.AddScoped<IReferenceDataService<SpecialityDto, CreateSpecialityDto, UpdateSpecialityDto>,
            ReferenceDataService<Speciality, SpecialityDto, CreateSpecialityDto, UpdateSpecialityDto>>();
        services.AddScoped<IReferenceDataService<ChecklistTemplateDto, CreateChecklistTemplateDto, UpdateChecklistTemplateDto>,
            ReferenceDataService<ChecklistTemplate, ChecklistTemplateDto, CreateChecklistTemplateDto, UpdateChecklistTemplateDto>>();
        services.AddScoped<IReferenceDataService<CountryDto, CreateCountryDto, UpdateCountryDto>,
            ReferenceDataService<Country, CountryDto, CreateCountryDto, UpdateCountryDto>>();
        services.AddScoped<IReferenceDataService<ExclusionDto, CreateExclusionDto, UpdateExclusionDto>,
            ReferenceDataService<Exclusion, ExclusionDto, CreateExclusionDto, UpdateExclusionDto>>();
        
        return services;
    }
    
    /// <summary>
    /// Registers a bounded Channel&lt;TariffUpdateJob&gt; as a singleton for background processing
    /// of tariff percentage propagation jobs.
    /// </summary>
    public static IServiceCollection AddTariffPercentageChannel(this IServiceCollection services)
    {
        var channel = Channel.CreateBounded<TariffUpdateJob>(new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        });

        services.AddSingleton(channel);
        services.AddSingleton(channel.Reader);
        services.AddSingleton(channel.Writer);

        return services;
    }
}
