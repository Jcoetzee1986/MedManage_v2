using AutoMapper;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        // Entity to DTO
        CreateMap<Case, CaseDto>();

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
            .ForMember(dest => dest.Status, opt => opt.Ignore());

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
            .ForMember(dest => dest.Status, opt => opt.Ignore());
    }
}
