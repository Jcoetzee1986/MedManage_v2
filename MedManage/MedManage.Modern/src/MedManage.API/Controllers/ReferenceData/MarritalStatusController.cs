using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class MarritalStatusController : ReferenceDataController<MarritalStatusDto, CreateMarritalStatusDto, UpdateMarritalStatusDto>
{
    public MarritalStatusController(IReferenceDataService<MarritalStatusDto, CreateMarritalStatusDto, UpdateMarritalStatusDto> service)
        : base(service, "Marital Status") { }

    protected override int GetIdFromDto(MarritalStatusDto dto) => dto.MarritalStatusId;
}
