using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class SpecialityController : ReferenceDataController<SpecialityDto, CreateSpecialityDto, UpdateSpecialityDto>
{
    public SpecialityController(IReferenceDataService<SpecialityDto, CreateSpecialityDto, UpdateSpecialityDto> service)
        : base(service, "Speciality") { }

    protected override int GetIdFromDto(SpecialityDto dto) => dto.SpecialityId;
}
