using AutoMapper;
using MedManage.Core.DTOs.CaseComment;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseCommentProfile : Profile
{
    public CaseCommentProfile()
    {
        CreateMap<CaseComment, CaseCommentDto>()
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.CaseComment1));

        CreateMap<CreateCaseCommentDto, CaseComment>()
            .ForMember(dest => dest.CaseComment1, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.CaseCommentId, opt => opt.Ignore());

        CreateMap<UpdateCaseCommentDto, CaseComment>()
            .ForMember(dest => dest.CaseComment1, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.CaseCommentId, opt => opt.Ignore());
    }
}
