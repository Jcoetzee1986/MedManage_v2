using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/case-status")]
public class CaseStatusController : ReferenceDataController<CaseStatusDto, CreateCaseStatusDto, UpdateCaseStatusDto>
{
    public CaseStatusController(IReferenceDataService<CaseStatusDto, CreateCaseStatusDto, UpdateCaseStatusDto> service)
        : base(service, "Case Status") { }

    protected override int GetIdFromDto(CaseStatusDto dto) => dto.CaseStatusId;
}
