using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class LanguageController : ReferenceDataController<LanguageDto, CreateLanguageDto, UpdateLanguageDto>
{
    public LanguageController(IReferenceDataService<LanguageDto, CreateLanguageDto, UpdateLanguageDto> service)
        : base(service, "Language") { }

    protected override int GetIdFromDto(LanguageDto dto) => dto.LanguageId;
}
