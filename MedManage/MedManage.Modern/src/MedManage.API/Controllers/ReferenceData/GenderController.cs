using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class GenderController : ReferenceDataController<GenderDto, CreateGenderDto, UpdateGenderDto>
{
    public GenderController(IReferenceDataService<GenderDto, CreateGenderDto, UpdateGenderDto> service)
        : base(service, "Gender") { }

    protected override int GetIdFromDto(GenderDto dto) => dto.GenderId;
}
