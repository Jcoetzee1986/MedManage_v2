using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class FacilityTypeController : ReferenceDataController<FacilityTypeDto, CreateFacilityTypeDto, UpdateFacilityTypeDto>
{
    public FacilityTypeController(IReferenceDataService<FacilityTypeDto, CreateFacilityTypeDto, UpdateFacilityTypeDto> service)
        : base(service, "Facility Type") { }

    protected override int GetIdFromDto(FacilityTypeDto dto) => dto.FacilityTypeId;
}
