using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/suspended-reason")]
public class SuspendedReasonController : ReferenceDataController<SuspendedReasonDto, CreateSuspendedReasonDto, UpdateSuspendedReasonDto>
{
    public SuspendedReasonController(IReferenceDataService<SuspendedReasonDto, CreateSuspendedReasonDto, UpdateSuspendedReasonDto> service)
        : base(service, "Suspended Reason") { }

    protected override int GetIdFromDto(SuspendedReasonDto dto) => dto.SuspendedReasonId;
}
