using AutoMapper;
using MedManage.Core.DTOs.Member;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        // Entity to DTO
        CreateMap<Member, MemberDto>();

        // Create Request to Entity
        CreateMap<CreateMemberRequest, Member>()
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // Update Request to Entity
        CreateMap<UpdateMemberRequest, Member>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
    }
}
