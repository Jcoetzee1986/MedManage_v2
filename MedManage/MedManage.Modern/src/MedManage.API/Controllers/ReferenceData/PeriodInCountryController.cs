using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/period-in-country")]
public class PeriodInCountryController : ReferenceDataController<PeriodInCountryDto, CreatePeriodInCountryDto, UpdatePeriodInCountryDto>
{
    public PeriodInCountryController(IReferenceDataService<PeriodInCountryDto, CreatePeriodInCountryDto, UpdatePeriodInCountryDto> service)
        : base(service, "Period in Country") { }

    protected override int GetIdFromDto(PeriodInCountryDto dto) => dto.PeriodInCountryId;
}
