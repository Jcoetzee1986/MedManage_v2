using AutoMapper;
using MedManage.Core.DTOs.ServiceProvider;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class ServiceProviderProfile : Profile
{
    public ServiceProviderProfile()
    {
        // Entity to DTO
        CreateMap<ServiceProvider, ServiceProviderDto>();

        // Entity to Autocomplete DTO
        CreateMap<ServiceProvider, ServiceProviderAutocompleteDto>();

        // Create Request to Entity
        CreateMap<CreateServiceProviderRequest, ServiceProvider>()
            .ForMember(dest => dest.ServiceProviderId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CaseReferFroms, opt => opt.Ignore())
            .ForMember(dest => dest.CaseReferTos, opt => opt.Ignore())
            .ForMember(dest => dest.Speciality, opt => opt.Ignore());

        // Update Request to Entity
        CreateMap<UpdateServiceProviderRequest, ServiceProvider>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CaseReferFroms, opt => opt.Ignore())
            .ForMember(dest => dest.CaseReferTos, opt => opt.Ignore())
            .ForMember(dest => dest.Speciality, opt => opt.Ignore());

        // ServiceProviderTariff mappings
        CreateMap<ServiceProviderTariff, ServiceProviderTariffDto>();
        CreateMap<CreateServiceProviderTariffRequest, ServiceProviderTariff>()
            .ForMember(dest => dest.ServiceProviderTariffId, opt => opt.Ignore())
            .ForMember(dest => dest.ServiceProviderId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // ServiceProviderTariffCustom mappings
        CreateMap<ServiceProviderTariffCustom, ServiceProviderTariffCustomDto>();
        CreateMap<CreateServiceProviderTariffCustomRequest, ServiceProviderTariffCustom>()
            .ForMember(dest => dest.ServiceProviderTariffCustomId, opt => opt.Ignore())
            .ForMember(dest => dest.ServiceProviderId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // ServiceProviderMainClientDiscount mappings
        CreateMap<ServiceProviderMainClientDiscount, ServiceProviderDiscountDto>();
    }
}
