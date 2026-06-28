using MedManage.Infrastructure.Mapping.Manual;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseLetterNote;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseLetterNoteService : ICaseLetterNoteService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseLetterNoteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseLetterNoteDto>> GetAllAsync()
    {
        var caseLetterNotes = await _unitOfWork.CaseLetterNotes.GetAllAsync();
        return caseLetterNotes.Select(e => e.ToDto());
    }

    public async Task<CaseLetterNoteDto?> GetByCaseIdAsync(int caseId)
    {
        var caseLetterNote = await _unitOfWork.CaseLetterNotes.GetByIdAsync(caseId);
        return caseLetterNote == null ? null : caseLetterNote.ToDto();
    }

    public async Task<CaseLetterNoteDto> CreateAsync(CreateCaseLetterNoteDto dto)
    {
        // Upsert — update if exists, insert if not
        var existing = await _unitOfWork.CaseLetterNotes.GetByIdAsync(dto.CaseId);
        if (existing != null)
        {
            existing.Note = dto.Note;
            existing.IncludeDischargeForm = dto.IncludeDischargeForm;
            existing.IncludeReferralLetter = dto.IncludeReferralLetter;
            existing.DateUpdated = DateTime.UtcNow;
            await _unitOfWork.CaseLetterNotes.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing.ToDto();
        }

        var caseLetterNote = dto.ToEntity();
        caseLetterNote.DateInserted = DateTime.UtcNow;
        
        await _unitOfWork.CaseLetterNotes.AddAsync(caseLetterNote);
        await _unitOfWork.SaveChangesAsync();
        
        return caseLetterNote.ToDto();
    }

    public async Task<CaseLetterNoteDto> UpdateAsync(int caseId, UpdateCaseLetterNoteDto dto)
    {
        var caseLetterNote = await _unitOfWork.CaseLetterNotes.GetByIdAsync(caseId);
        if (caseLetterNote == null)
            throw new KeyNotFoundException($"CaseLetterNote for Case ID {caseId} not found");

        dto.ApplyTo(caseLetterNote);
        caseLetterNote.DateUpdated = DateTime.UtcNow;
        
        await _unitOfWork.CaseLetterNotes.UpdateAsync(caseLetterNote);
        await _unitOfWork.SaveChangesAsync();
        
        return caseLetterNote.ToDto();
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
