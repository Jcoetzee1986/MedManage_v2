using AutoMapper;
using MedManage.Core.DTOs.CaseFacilityType;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseFacilityTypeProfile : Profile
{
    public CaseFacilityTypeProfile()
    {
        CreateMap<CaseFacilityType, CaseFacilityTypeDto>();

        CreateMap<CreateCaseFacilityTypeRequest, CaseFacilityType>()
            .ForMember(dest => dest.CaseIdFacilityTypeId, opt => opt.Ignore());

        CreateMap<UpdateCaseFacilityTypeRequest, CaseFacilityType>()
            .ForMember(dest => dest.CaseIdFacilityTypeId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseId, opt => opt.Ignore());
    }
}
