using AutoMapper;
using MedManage.Core.DTOs.EpisodeCase;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class EpisodeCaseProfile : Profile
{
    public EpisodeCaseProfile()
    {
        CreateMap<EpisodeCase, EpisodeCaseDto>();
        CreateMap<CreateEpisodeCaseDto, EpisodeCase>();
        CreateMap<UpdateEpisodeCaseDto, EpisodeCase>();
    }
}
