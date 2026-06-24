using AutoMapper;
using MedManage.Core.DTOs.Tariff;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class TariffProfile : Profile
{
    public TariffProfile()
    {
        // Base Tariff mappings
        CreateMap<BaseTariff, BaseTariffDto>();
        CreateMap<CreateBaseTariffDto, BaseTariff>();
        CreateMap<UpdateBaseTariffDto, BaseTariff>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Tariff Rate mappings
        CreateMap<Tariff, TariffRateDto>();
        CreateMap<CreateTariffRateDto, Tariff>();
        CreateMap<UpdateTariffRateDto, Tariff>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Tariff Name mappings
        CreateMap<TariffName, TariffNameDto>()
            .ForMember(dest => dest.TariffName, opt => opt.MapFrom(src => src.TariffName1));
        CreateMap<CreateTariffNameDto, TariffName>()
            .ForMember(dest => dest.TariffName1, opt => opt.MapFrom(src => src.TariffName));
        CreateMap<UpdateTariffNameDto, TariffName>()
            .ForMember(dest => dest.TariffName1, opt => opt.MapFrom(src => src.TariffName))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
