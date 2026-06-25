using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/case-type")]
public class CaseTypeController : ReferenceDataController<CaseTypeDto, CreateCaseTypeDto, UpdateCaseTypeDto>
{
    public CaseTypeController(IReferenceDataService<CaseTypeDto, CreateCaseTypeDto, UpdateCaseTypeDto> service)
        : base(service, "Case Type") { }

    protected override int GetIdFromDto(CaseTypeDto dto) => dto.CaseTypeId;
}
