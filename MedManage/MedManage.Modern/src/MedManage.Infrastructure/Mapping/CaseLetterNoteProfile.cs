using AutoMapper;
using MedManage.Core.DTOs.CaseLetterNote;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class CaseLetterNoteProfile : Profile
{
    public CaseLetterNoteProfile()
    {
        CreateMap<CaseLetterNote, CaseLetterNoteDto>();

        CreateMap<CreateCaseLetterNoteDto, CaseLetterNote>();

        CreateMap<UpdateCaseLetterNoteDto, CaseLetterNote>()
            .ForMember(dest => dest.CaseId, opt => opt.Ignore());
    }
}
