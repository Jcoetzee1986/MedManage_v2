using AutoMapper;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseBillingProfile : Profile
{
    public CaseBillingProfile()
    {
        CreateMap<CaseBilling, CaseBillingDto>();
        CreateMap<CreateCaseBillingDto, CaseBilling>();
        CreateMap<UpdateCaseBillingDto, CaseBilling>();

        // CaseDiscount mappings
        CreateMap<CaseDiscount, CaseDiscountDto>();
        CreateMap<CreateCaseDiscountDto, CaseDiscount>();
    }
}
