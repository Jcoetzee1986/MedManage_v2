using AutoMapper;
using MedManage.Core.DTOs.MemberNote;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class MemberNoteProfile : Profile
{
    public MemberNoteProfile()
    {
        // Map between MemberNote entity and DTOs
        CreateMap<MemberNote, MemberNoteDto>()
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.MemberNote1))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated))
            .ReverseMap()
            .ForMember(dest => dest.MemberNote1, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => src.DateModified))
            .ForMember(dest => dest.Member, opt => opt.Ignore());

        CreateMap<CreateMemberNoteDto, MemberNote>()
            .ForMember(dest => dest.MemberNote1, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.MemberNoteId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore());

        CreateMap<UpdateMemberNoteDto, MemberNote>()
            .ForMember(dest => dest.MemberNote1, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.MemberNoteId, opt => opt.Ignore())
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore());
    }
}
