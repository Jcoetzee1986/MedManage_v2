using AutoMapper;
using MedManage.Core.DTOs.CaseIcd;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseIcdProfile : Profile
{
    public CaseIcdProfile()
    {
        CreateMap<CaseIcd, CaseIcdDto>()
            .ForMember(dest => dest.DiagnosisCode, opt => opt.MapFrom(src => src.Icd != null ? src.Icd.DiagnosisCode : null))
            .ForMember(dest => dest.DiagnosisDesc, opt => opt.MapFrom(src => src.Icd != null ? src.Icd.DiagnosisDesc : null))
            .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.Icd != null ? src.Icd.GroupCode : null))
            .ForMember(dest => dest.GroupDesc, opt => opt.MapFrom(src => src.Icd != null ? src.Icd.GroupDesc : null));

        CreateMap<CreateCaseIcdDto, CaseIcd>();

        CreateMap<UpdateCaseIcdDto, CaseIcd>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
