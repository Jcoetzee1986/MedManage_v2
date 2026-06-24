using AutoMapper;
using MedManage.Core.DTOs.CaseNappi;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseNappiProfile : Profile
{
    public CaseNappiProfile()
    {
        CreateMap<CaseNappiCode, CaseNappiDto>();

        CreateMap<CreateCaseNappiDto, CaseNappiCode>()
            .ForMember(dest => dest.CaseIdNappiId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseId, opt => opt.Ignore());

        CreateMap<UpdateCaseNappiDto, CaseNappiCode>()
            .ForMember(dest => dest.CaseIdNappiId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseId, opt => opt.Ignore());
    }
}
