using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseLetterNote;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseLetterNoteService : ICaseLetterNoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseLetterNoteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseLetterNoteDto>> GetAllAsync()
    {
        var caseLetterNotes = await _unitOfWork.CaseLetterNotes.GetAllAsync();
        return _mapper.Map<IEnumerable<CaseLetterNoteDto>>(caseLetterNotes);
    }

    public async Task<CaseLetterNoteDto?> GetByCaseIdAsync(int caseId)
    {
        var caseLetterNote = await _unitOfWork.CaseLetterNotes.GetByIdAsync(caseId);
        return caseLetterNote == null ? null : _mapper.Map<CaseLetterNoteDto>(caseLetterNote);
    }

    public async Task<CaseLetterNoteDto> CreateAsync(CreateCaseLetterNoteDto dto)
    {
        var caseLetterNote = _mapper.Map<CaseLetterNote>(dto);
        caseLetterNote.DateInserted = DateTime.UtcNow;
        
        await _unitOfWork.CaseLetterNotes.AddAsync(caseLetterNote);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CaseLetterNoteDto>(caseLetterNote);
    }

    public async Task<CaseLetterNoteDto> UpdateAsync(int caseId, UpdateCaseLetterNoteDto dto)
    {
        var caseLetterNote = await _unitOfWork.CaseLetterNotes.GetByIdAsync(caseId);
        if (caseLetterNote == null)
            throw new KeyNotFoundException($"CaseLetterNote for Case ID {caseId} not found");

        _mapper.Map(dto, caseLetterNote);
        caseLetterNote.DateUpdated = DateTime.UtcNow;
        
        await _unitOfWork.CaseLetterNotes.UpdateAsync(caseLetterNote);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CaseLetterNoteDto>(caseLetterNote);
    }

    public async Task<bool> DeleteAsync(int caseId)
    {
        var caseLetterNote = await _unitOfWork.CaseLetterNotes.GetByIdAsync(caseId);
        if (caseLetterNote == null)
            return false;

        caseLetterNote.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseLetterNotes.UpdateAsync(caseLetterNote);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}
