using AutoMapper;
using MedManage.Core.DTOs.CaseCpt;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseCptProfile : Profile
{
    public CaseCptProfile()
    {
        CreateMap<CaseCpt, CaseCptDto>()
            .ForMember(dest => dest.CptCode, opt => opt.MapFrom(src => src.Cpt != null ? src.Cpt.Code : null))
            .ForMember(dest => dest.CptShortDescription, opt => opt.MapFrom(src => src.Cpt != null ? src.Cpt.ShortDescr : null))
            .ForMember(dest => dest.CptMediumDescription, opt => opt.MapFrom(src => src.Cpt != null ? src.Cpt.MediumDescr : null));

        CreateMap<CreateCaseCptDto, CaseCpt>()
            .ForMember(dest => dest.CaseIdCptid, opt => opt.Ignore())
            .ForMember(dest => dest.CaseId, opt => opt.Ignore())
            .ForMember(dest => dest.Case, opt => opt.Ignore())
            .ForMember(dest => dest.Cpt, opt => opt.Ignore());

        CreateMap<UpdateCaseCptDto, CaseCpt>()
            .ForMember(dest => dest.CaseIdCptid, opt => opt.Ignore())
            .ForMember(dest => dest.CaseId, opt => opt.Ignore())
            .ForMember(dest => dest.Case, opt => opt.Ignore())
            .ForMember(dest => dest.Cpt, opt => opt.Ignore());
    }
}
