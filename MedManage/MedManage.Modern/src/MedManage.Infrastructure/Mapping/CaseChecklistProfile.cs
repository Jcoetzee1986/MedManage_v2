using AutoMapper;
using MedManage.Core.DTOs.CaseChecklist;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseChecklistProfile : Profile
{
    public CaseChecklistProfile()
    {
        CreateMap<CaseChecklist, CaseChecklistDto>();
        CreateMap<CreateCaseChecklistDto, CaseChecklist>();
        CreateMap<UpdateCaseChecklistDto, CaseChecklist>();
    }
}
