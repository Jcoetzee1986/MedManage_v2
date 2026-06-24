using AutoMapper;
using MedManage.Core.DTOs.MedicalAid;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class MedicalAidProfile : Profile
{
    public MedicalAidProfile()
    {
        // MedicalAid -> MedicalAidDto
        CreateMap<MedicalAid, MedicalAidDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));

        // CreateMedicalAidDto -> MedicalAid
        CreateMap<CreateMedicalAidDto, MedicalAid>()
            .ForMember(dest => dest.MedicalAidId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.MedicalAidExclusions, opt => opt.Ignore())
            .ForMember(dest => dest.MedicalAidTariffs, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // UpdateMedicalAidDto -> MedicalAid
        CreateMap<UpdateMedicalAidDto, MedicalAid>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.MedicalAidExclusions, opt => opt.Ignore())
            .ForMember(dest => dest.MedicalAidTariffs, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());

        // MedicalAidProduct -> MedicalAidProductDto
        CreateMap<MedicalAidProduct, MedicalAidProductDto>();
    }
}
