using AutoMapper;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class ExclusionProfile : Profile
{
    public ExclusionProfile()
    {
        // Exclusion -> ExclusionDto
        CreateMap<Exclusion, ExclusionDto>()
            .ForMember(dest => dest.Exclusion, opt => opt.MapFrom(src => src.Exclusion1))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));

        // CreateExclusionDto -> Exclusion
        CreateMap<CreateExclusionDto, Exclusion>()
            .ForMember(dest => dest.Exclusion1, opt => opt.MapFrom(src => src.Exclusion))
            .ForMember(dest => dest.ExclusionId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CaseExclusions, opt => opt.Ignore());

        // UpdateExclusionDto -> Exclusion
        CreateMap<UpdateExclusionDto, Exclusion>()
            .ForMember(dest => dest.Exclusion1, opt => opt.MapFrom(src => src.Exclusion))
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CaseExclusions, opt => opt.Ignore());
    }
}
