using AutoMapper;
using MedManage.Core.DTOs.CaseLink;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseLinkProfile : Profile
{
    public CaseLinkProfile()
    {
        CreateMap<CaseLink, CaseLinkDto>();
    }
}
