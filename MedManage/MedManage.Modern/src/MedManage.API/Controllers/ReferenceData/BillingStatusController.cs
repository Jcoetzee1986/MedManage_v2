using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class BillingStatusController : ReferenceDataController<BillingStatusDto, CreateBillingStatusDto, UpdateBillingStatusDto>
{
    public BillingStatusController(IReferenceDataService<BillingStatusDto, CreateBillingStatusDto, UpdateBillingStatusDto> service)
        : base(service, "Billing Status") { }

    protected override int GetIdFromDto(BillingStatusDto dto) => dto.BillingStatusId;
}
