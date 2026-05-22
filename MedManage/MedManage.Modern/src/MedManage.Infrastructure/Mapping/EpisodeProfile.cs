using AutoMapper;
using MedManage.Core.DTOs.Episode;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class EpisodeProfile : Profile
{
    public EpisodeProfile()
    {
        // Episode -> EpisodeDto
        CreateMap<Episode, EpisodeDto>()
            .ForMember(dest => dest.DateInserted, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));

        // CreateEpisodeDto -> Episode
        CreateMap<CreateEpisodeDto, Episode>()
            .ForMember(dest => dest.EpisodeId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // UpdateEpisodeDto -> Episode
        CreateMap<UpdateEpisodeDto, Episode>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
    }
}
