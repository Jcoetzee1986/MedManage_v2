using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/checklist-template")]
public class ChecklistTemplateController : ReferenceDataController<ChecklistTemplateDto, CreateChecklistTemplateDto, UpdateChecklistTemplateDto>
{
    public ChecklistTemplateController(IReferenceDataService<ChecklistTemplateDto, CreateChecklistTemplateDto, UpdateChecklistTemplateDto> service)
        : base(service, "Checklist Template") { }

    protected override int GetIdFromDto(ChecklistTemplateDto dto) => dto.ChecklistTemplateId;
}
