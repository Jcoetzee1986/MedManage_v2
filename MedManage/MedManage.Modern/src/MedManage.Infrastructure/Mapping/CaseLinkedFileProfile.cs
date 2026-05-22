using AutoMapper;
using MedManage.Core.DTOs.CaseLinkedFile;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseLinkedFileProfile : Profile
{
    public CaseLinkedFileProfile()
    {
        CreateMap<CaseLinkedFile, CaseLinkedFileDto>();

        CreateMap<CreateCaseLinkedFileDto, CaseLinkedFile>()
            .ForMember(dest => dest.CaseLinkedFileId, opt => opt.Ignore());

        CreateMap<UpdateCaseLinkedFileDto, CaseLinkedFile>()
            .ForMember(dest => dest.CaseLinkedFileId, opt => opt.Ignore());
    }
}
