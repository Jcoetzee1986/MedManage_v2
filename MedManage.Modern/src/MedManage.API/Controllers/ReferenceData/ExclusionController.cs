using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class ExclusionController : ReferenceDataController<ExclusionDto, CreateExclusionDto, UpdateExclusionDto>
{
    public ExclusionController(IReferenceDataService<ExclusionDto, CreateExclusionDto, UpdateExclusionDto> service)
        : base(service, "Exclusion") { }

    protected override int GetIdFromDto(ExclusionDto dto) => dto.ExclusionId;
}
