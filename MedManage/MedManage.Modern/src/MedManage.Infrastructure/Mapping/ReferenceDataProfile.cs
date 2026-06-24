using AutoMapper;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class ReferenceDataProfile : Profile
{
    public ReferenceDataProfile()
    {
        // MarritalStatus mappings
        CreateMap<MarritalStatus, MarritalStatusDto>()
            .ForMember(dest => dest.MarritalStatus, opt => opt.MapFrom(src => src.MarritalStatus1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateMarritalStatusDto, MarritalStatus>()
            .ForMember(dest => dest.MarritalStatusId, opt => opt.Ignore())
            .ForMember(dest => dest.MarritalStatus1, opt => opt.MapFrom(src => src.MarritalStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateMarritalStatusDto, MarritalStatus>()
            .ForMember(dest => dest.MarritalStatus1, opt => opt.MapFrom(src => src.MarritalStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // MemberStatus mappings
        CreateMap<MemberStatus, MemberStatusDto>()
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => src.MemberStatus1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateMemberStatusDto, MemberStatus>()
            .ForMember(dest => dest.MemberStatusId, opt => opt.Ignore())
            .ForMember(dest => dest.MemberStatus1, opt => opt.MapFrom(src => src.MemberStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateMemberStatusDto, MemberStatus>()
            .ForMember(dest => dest.MemberStatus1, opt => opt.MapFrom(src => src.MemberStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // BillingStatus mappings
        CreateMap<BillingStatus, BillingStatusDto>()
            .ForMember(dest => dest.BillingStatus, opt => opt.MapFrom(src => src.BillingStatus1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateBillingStatusDto, BillingStatus>()
            .ForMember(dest => dest.BillingStatusId, opt => opt.Ignore())
            .ForMember(dest => dest.BillingStatus1, opt => opt.MapFrom(src => src.BillingStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateBillingStatusDto, BillingStatus>()
            .ForMember(dest => dest.BillingStatus1, opt => opt.MapFrom(src => src.BillingStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // CaseStatus mappings
        CreateMap<CaseStatus, CaseStatusDto>()
            .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => src.CaseStatus1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateCaseStatusDto, CaseStatus>()
            .ForMember(dest => dest.CaseStatusId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseStatus1, opt => opt.MapFrom(src => src.CaseStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateCaseStatusDto, CaseStatus>()
            .ForMember(dest => dest.CaseStatus1, opt => opt.MapFrom(src => src.CaseStatus))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // CaseType mappings
        CreateMap<CaseType, CaseTypeDto>()
            .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => src.CaseType1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateCaseTypeDto, CaseType>()
            .ForMember(dest => dest.CaseTypeId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseType1, opt => opt.MapFrom(src => src.CaseType))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateCaseTypeDto, CaseType>()
            .ForMember(dest => dest.CaseType1, opt => opt.MapFrom(src => src.CaseType))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // FacilityType mappings
        CreateMap<FacilityType, FacilityTypeDto>()
            .ForMember(dest => dest.FacilityType, opt => opt.MapFrom(src => src.FacilityType1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateFacilityTypeDto, FacilityType>()
            .ForMember(dest => dest.FacilityTypeId, opt => opt.Ignore())
            .ForMember(dest => dest.FacilityType1, opt => opt.MapFrom(src => src.FacilityType))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateFacilityTypeDto, FacilityType>()
            .ForMember(dest => dest.FacilityType1, opt => opt.MapFrom(src => src.FacilityType))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // Speciality mappings
        CreateMap<Speciality, SpecialityDto>()
            .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src => src.Speciality1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateSpecialityDto, Speciality>()
            .ForMember(dest => dest.SpecialityId, opt => opt.Ignore())
            .ForMember(dest => dest.Speciality1, opt => opt.MapFrom(src => src.Speciality))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateSpecialityDto, Speciality>()
            .ForMember(dest => dest.Speciality1, opt => opt.MapFrom(src => src.Speciality))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // ChronicIllness mappings
        CreateMap<ChronicIllness, ChronicIllnessDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateChronicIllnessDto, ChronicIllness>()
            .ForMember(dest => dest.ChronicIllnessId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateChronicIllnessDto, ChronicIllness>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // ChecklistTemplate mappings
        CreateMap<ChecklistTemplate, ChecklistTemplateDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateChecklistTemplateDto, ChecklistTemplate>()
            .ForMember(dest => dest.ChecklistTemplateId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateChecklistTemplateDto, ChecklistTemplate>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // Country mappings
        CreateMap<Country, CountryDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateCountryDto, Country>()
            .ForMember(dest => dest.CountryId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateCountryDto, Country>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // Gender mappings
        CreateMap<Gender, GenderDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateGenderDto, Gender>()
            .ForMember(dest => dest.GenderId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());
        CreateMap<UpdateGenderDto, Gender>()
            .ForMember(dest => dest.GenderId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // Language mappings
        CreateMap<Language, LanguageDto>()
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateLanguageDto, Language>()
            .ForMember(dest => dest.LanguageId, opt => opt.Ignore())
            .ForMember(dest => dest.Language1, opt => opt.MapFrom(src => src.Language))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());
        CreateMap<UpdateLanguageDto, Language>()
            .ForMember(dest => dest.LanguageId, opt => opt.Ignore())
            .ForMember(dest => dest.Language1, opt => opt.MapFrom(src => src.Language))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // Race mappings
        CreateMap<Race, RaceDto>()
            .ForMember(dest => dest.Race, opt => opt.MapFrom(src => src.Race1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateRaceDto, Race>()
            .ForMember(dest => dest.RaceId, opt => opt.Ignore())
            .ForMember(dest => dest.Race1, opt => opt.MapFrom(src => src.Race))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());
        CreateMap<UpdateRaceDto, Race>()
            .ForMember(dest => dest.RaceId, opt => opt.Ignore())
            .ForMember(dest => dest.Race1, opt => opt.MapFrom(src => src.Race))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // Title mappings
        CreateMap<Title, TitleDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateTitleDto, Title>()
            .ForMember(dest => dest.TitleId, opt => opt.Ignore())
            .ForMember(dest => dest.Title1, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());
        CreateMap<UpdateTitleDto, Title>()
            .ForMember(dest => dest.TitleId, opt => opt.Ignore())
            .ForMember(dest => dest.Title1, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // PeriodInCountry mappings
        CreateMap<PeriodInCountry, PeriodInCountryDto>()
            .ForMember(dest => dest.PeriodInCountry, opt => opt.MapFrom(src => src.PeriodInCountry1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreatePeriodInCountryDto, PeriodInCountry>()
            .ForMember(dest => dest.PeriodInCountryId, opt => opt.Ignore())
            .ForMember(dest => dest.PeriodInCountry1, opt => opt.MapFrom(src => src.PeriodInCountry))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());
        CreateMap<UpdatePeriodInCountryDto, PeriodInCountry>()
            .ForMember(dest => dest.PeriodInCountryId, opt => opt.Ignore())
            .ForMember(dest => dest.PeriodInCountry1, opt => opt.MapFrom(src => src.PeriodInCountry))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // CaseCategory mappings
        CreateMap<CaseCategory, CaseCategoryDto>()
            .ForMember(dest => dest.CaseCategory, opt => opt.MapFrom(src => src.CaseCategory1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateCaseCategoryDto, CaseCategory>()
            .ForMember(dest => dest.CaseCategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseCategory1, opt => opt.MapFrom(src => src.CaseCategory))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateCaseCategoryDto, CaseCategory>()
            .ForMember(dest => dest.CaseCategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseCategory1, opt => opt.MapFrom(src => src.CaseCategory))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // SuspendedReason mappings
        CreateMap<SuspendedReason, SuspendedReasonDto>()
            .ForMember(dest => dest.SuspendedReason, opt => opt.MapFrom(src => src.SuspendedReason1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));
        CreateMap<CreateSuspendedReasonDto, SuspendedReason>()
            .ForMember(dest => dest.SuspendedReasonId, opt => opt.Ignore())
            .ForMember(dest => dest.SuspendedReason1, opt => opt.MapFrom(src => src.SuspendedReason))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
        CreateMap<UpdateSuspendedReasonDto, SuspendedReason>()
            .ForMember(dest => dest.SuspendedReasonId, opt => opt.Ignore())
            .ForMember(dest => dest.SuspendedReason1, opt => opt.MapFrom(src => src.SuspendedReason))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
    }
}
