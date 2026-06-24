using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers.ReferenceData;

[Route("api/[controller]")]
public class MemberStatusController : ReferenceDataController<MemberStatusDto, CreateMemberStatusDto, UpdateMemberStatusDto>
{
    public MemberStatusController(IReferenceDataService<MemberStatusDto, CreateMemberStatusDto, UpdateMemberStatusDto> service)
        : base(service, "Member Status") { }

    protected override int GetIdFromDto(MemberStatusDto dto) => dto.MemberStatusId;
}
