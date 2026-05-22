using AutoMapper;
using MedManage.Core.DTOs.CaseExclusion;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseExclusionProfile : Profile
{
    public CaseExclusionProfile()
    {
        CreateMap<CaseExclusion, CaseExclusionDto>();
        CreateMap<CreateCaseExclusionDto, CaseExclusion>();
        CreateMap<UpdateCaseExclusionDto, CaseExclusion>();
    }
}
