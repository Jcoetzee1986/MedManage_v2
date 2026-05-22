using AutoMapper;
using MedManage.Core.DTOs.CaseNote;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseNoteProfile : Profile
{
    public CaseNoteProfile()
    {
        // Map between CaseNote entity and DTOs
        CreateMap<CaseNote, CaseNoteDto>()
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.CaseNote1))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated))
            .ReverseMap()
            .ForMember(dest => dest.CaseNote1, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => src.DateModified))
            .ForMember(dest => dest.Case, opt => opt.Ignore());

        CreateMap<CreateCaseNoteDto, CaseNote>()
            .ForMember(dest => dest.CaseNote1, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.CaseNoteId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Case, opt => opt.Ignore());

        CreateMap<UpdateCaseNoteDto, CaseNote>()
            .ForMember(dest => dest.CaseNote1, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.CaseNoteId, opt => opt.Ignore())
            .ForMember(dest => dest.CaseId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Case, opt => opt.Ignore());
    }
}
