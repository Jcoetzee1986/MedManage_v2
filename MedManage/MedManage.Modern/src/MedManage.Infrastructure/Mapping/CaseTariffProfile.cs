using AutoMapper;
using MedManage.Core.DTOs.CaseTariff;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseTariffProfile : Profile
{
    public CaseTariffProfile()
    {
        CreateMap<CaseTariff, CaseTariffDto>();
        CreateMap<CreateCaseTariffRequest, CaseTariff>();
        CreateMap<UpdateCaseTariffRequest, CaseTariff>();
    }
}
