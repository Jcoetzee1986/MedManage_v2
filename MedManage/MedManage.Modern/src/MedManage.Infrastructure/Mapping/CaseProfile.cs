using AutoMapper;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        // Entity to DTO — flatten navigation properties for list and detail views
        CreateMap<Case, CaseDto>()
            // Member fields
            .ForMember(dest => dest.MemberNumber, opt => opt.MapFrom(src => src.Member != null ? src.Member.MemberNumber : null))
            .ForMember(dest => dest.MemberSurname, opt => opt.MapFrom(src => src.Member != null ? src.Member.Surname : null))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.Name : null))
            .ForMember(dest => dest.MemberIdNumber, opt => opt.MapFrom(src => src.Member != null ? src.Member.Idnumber : null))
            .ForMember(dest => dest.MemberDateOfBirth, opt => opt.MapFrom(src => src.Member != null ? src.Member.DateOfBirth : null))
            .ForMember(dest => dest.MemberMedicalAidName, opt => opt.MapFrom(src => src.Member != null && src.Member.MedicalAid != null ? src.Member.MedicalAid.MedicalAidName : null))
            .ForMember(dest => dest.MemberProductName, opt => opt.MapFrom(src => (string?)null)) // Product name needs separate join; set below
            .ForMember(dest => dest.MemberStatusName, opt => opt.MapFrom(src => src.Member != null && src.Member.MemberStatus != null ? src.Member.MemberStatus.MemberStatus1 : null))
            // Status / Type
            .ForMember(dest => dest.CaseStatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.CaseStatus1 : null))
            .ForMember(dest => dest.CaseTypeName, opt => opt.MapFrom(src => src.AuthType != null ? src.AuthType.CaseType1 : null))
            // Refer To provider
            .ForMember(dest => dest.ReferToPracticeName, opt => opt.MapFrom(src => src.ReferTo != null ? src.ReferTo.PracticeName : null))
            .ForMember(dest => dest.ReferToPersonSurname, opt => opt.MapFrom(src => src.ReferTo != null ? src.ReferTo.ServiceProviderSurname : null))
            .ForMember(dest => dest.ReferToPersonName, opt => opt.MapFrom(src => src.ReferTo != null ? src.ReferTo.ServiceProviderName : null))
            .ForMember(dest => dest.ReferToSpeciality, opt => opt.MapFrom(src => src.ReferTo != null && src.ReferTo.Speciality != null ? src.ReferTo.Speciality.Speciality1 : null))
            // Refer From provider
            .ForMember(dest => dest.ReferFromPracticeName, opt => opt.MapFrom(src => src.ReferFrom != null ? src.ReferFrom.PracticeName : null))
            .ForMember(dest => dest.ReferFromPersonSurname, opt => opt.MapFrom(src => src.ReferFrom != null ? src.ReferFrom.ServiceProviderSurname : null))
            .ForMember(dest => dest.ReferFromPersonName, opt => opt.MapFrom(src => src.ReferFrom != null ? src.ReferFrom.ServiceProviderName : null))
            .ForMember(dest => dest.ReferFromSpeciality, opt => opt.MapFrom(src => src.ReferFrom != null && src.ReferFrom.Speciality != null ? src.ReferFrom.Speciality.Speciality1 : null));

        // Create Request to Entity
        CreateMap<CreateCaseRequest, Case>()
            .ForMember(dest => dest.CaseId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CaseChecklists, opt => opt.Ignore())
            .ForMember(dest => dest.CaseComments, opt => opt.Ignore())
            .ForMember(dest => dest.CaseCpts, opt => opt.Ignore())
            .ForMember(dest => dest.CaseExclusions, opt => opt.Ignore())
            .ForMember(dest => dest.CaseFacilityTypes, opt => opt.Ignore())
            .ForMember(dest => dest.CaseIcds, opt => opt.Ignore())
            .ForMember(dest => dest.CaseNotes, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.ReferFrom, opt => opt.Ignore())
            .ForMember(dest => dest.ReferTo, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.AuthType, opt => opt.Ignore());

        // Update Request to Entity
        CreateMap<UpdateCaseRequest, Case>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CaseChecklists, opt => opt.Ignore())
            .ForMember(dest => dest.CaseComments, opt => opt.Ignore())
            .ForMember(dest => dest.CaseCpts, opt => opt.Ignore())
            .ForMember(dest => dest.CaseExclusions, opt => opt.Ignore())
            .ForMember(dest => dest.CaseFacilityTypes, opt => opt.Ignore())
            .ForMember(dest => dest.CaseIcds, opt => opt.Ignore())
            .ForMember(dest => dest.CaseNotes, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.ReferFrom, opt => opt.Ignore())
            .ForMember(dest => dest.ReferTo, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.AuthType, opt => opt.Ignore());
    }
}
