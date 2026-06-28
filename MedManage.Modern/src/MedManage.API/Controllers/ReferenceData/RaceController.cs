using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class RaceController : ReferenceDataController<RaceDto, CreateRaceDto, UpdateRaceDto>
{
    public RaceController(IReferenceDataService<RaceDto, CreateRaceDto, UpdateRaceDto> service)
        : base(service, "Race") { }

    protected override int GetIdFromDto(RaceDto dto) => dto.RaceId;
}
