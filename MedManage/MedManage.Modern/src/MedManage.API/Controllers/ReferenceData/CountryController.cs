using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class CountryController : ReferenceDataController<CountryDto, CreateCountryDto, UpdateCountryDto>
{
    public CountryController(IReferenceDataService<CountryDto, CreateCountryDto, UpdateCountryDto> service)
        : base(service, "Country") { }

    protected override int GetIdFromDto(CountryDto dto) => dto.CountryId;
}
