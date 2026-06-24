using AutoMapper;
using MedManage.Core.DTOs.MemberChronicIllness;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class MemberChronicIllnessProfile : Profile
{
    public MemberChronicIllnessProfile()
    {
        CreateMap<MemberChronicIllness, MemberChronicIllnessDto>()
            .ForMember(dest => dest.ChronicIllnessName, opt => opt.MapFrom(src => src.ChronicIllness != null ? src.ChronicIllness.ChronicIllnessName : null))
            .ForMember(dest => dest.ChronicIllnessDescription, opt => opt.MapFrom(src => src.ChronicIllness != null ? src.ChronicIllness.ChronicIllnessDescription : null));
    }
}
