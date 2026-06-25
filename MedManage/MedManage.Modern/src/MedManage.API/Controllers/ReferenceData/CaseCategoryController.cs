using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/case-category")]
public class CaseCategoryController : ReferenceDataController<CaseCategoryDto, CreateCaseCategoryDto, UpdateCaseCategoryDto>
{
    public CaseCategoryController(IReferenceDataService<CaseCategoryDto, CreateCaseCategoryDto, UpdateCaseCategoryDto> service)
        : base(service, "Case Category") { }

    protected override int GetIdFromDto(CaseCategoryDto dto) => dto.CaseCategoryId;
}
