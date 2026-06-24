using AutoMapper;
using MedManage.Core.DTOs.MemberMedicalAidProduct;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class MemberMedicalAidProductProfile : Profile
{
    public MemberMedicalAidProductProfile()
    {
        CreateMap<MemberMedicalAidProduct, MemberMedicalAidProductDto>();

        CreateMap<CreateMemberMedicalAidProductDto, MemberMedicalAidProduct>()
            .ForMember(dest => dest.MedAidProductIdMemberId, opt => opt.Ignore())
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        CreateMap<UpdateMemberMedicalAidProductDto, MemberMedicalAidProduct>()
            .ForMember(dest => dest.MedAidProductIdMemberId, opt => opt.Ignore())
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
