using System.Reflection;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Repositories;
using MedManage.Infrastructure.Services.Business;
using Microsoft.Extensions.DependencyInjection;

namespace MedManage.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Add AutoMapper (DI extensions included in v16+)
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        
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
        
        // Other Business Repositories
        services.AddScoped<ILinkedFileRepository, LinkedFileRepository>();
        services.AddScoped<IMedicalAidExclusionRepository, MedicalAidExclusionRepository>();
        services.AddScoped<IEpisodeCaseRepository, EpisodeCaseRepository>();
        services.AddScoped<ISessionUserCaseRepository, SessionUserCaseRepository>();

        return services;
    }
    
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Core Business Services
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<IServiceProviderService, ServiceProviderService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IEpisodeService, EpisodeService>();
        services.AddScoped<IMedicalAidService, MedicalAidService>();
        services.AddScoped<IExclusionService, ExclusionService>();
        services.AddScoped<IMemberNoteService, MemberNoteService>();
        services.AddScoped<ICaseNoteService, CaseNoteService>();
        services.AddScoped<ICaseCommentService, CaseCommentService>();
        services.AddScoped<ICaseLetterNoteService, CaseLetterNoteService>();
        services.AddScoped<ICaseLinkedFileService, CaseLinkedFileService>();
        services.AddScoped<ICaseBillingService, CaseBillingService>();
        services.AddScoped<ICaseChecklistService, CaseChecklistService>();
        services.AddScoped<ICaseExclusionService, CaseExclusionService>();
        services.AddScoped<IEpisodeCaseService, EpisodeCaseService>();
        
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
        
        return services;
    }
}
