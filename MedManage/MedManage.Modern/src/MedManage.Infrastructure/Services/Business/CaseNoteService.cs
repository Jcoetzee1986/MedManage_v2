using AutoMapper;
using MedManage.Core.DTOs.CaseNote;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseNoteService : ICaseNoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CaseNoteService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CaseNoteDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _unitOfWork.CaseNotes.GetAllAsync();
        
        if (!includeDeleted)
        {
            notes = notes.Where(n => n.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<CaseNoteDto>>(notes);
    }

    public async Task<CaseNoteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.CaseNotes.GetByIdAsync(id);
        
        if (note == null || note.DateDeleted != null)
        {
            return null;
        }
        
        return _mapper.Map<CaseNoteDto>(note);
    }

    public async Task<IEnumerable<CaseNoteDto>> GetByCaseIdAsync(int caseId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _unitOfWork.CaseNotes.GetAllAsync();
        
        notes = notes.Where(n => n.CaseId == caseId);
        
        if (!includeDeleted)
        {
            notes = notes.Where(n => n.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<CaseNoteDto>>(notes.OrderByDescending(n => n.DateCreated ?? n.DateInserted));
    }

    public async Task<CaseNoteDto> CreateAsync(CreateCaseNoteDto dto, CancellationToken cancellationToken = default)
    {
        var note = _mapper.Map<CaseNote>(dto);
        
        // Set DateCreated to now if not provided
        if (!note.DateCreated.HasValue)
        {
            note.DateCreated = DateTime.Now;
        }
        
        await _unitOfWork.CaseNotes.AddAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CaseNoteDto>(note);
    }

    public async Task<CaseNoteDto> UpdateAsync(int id, UpdateCaseNoteDto dto, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.CaseNotes.GetByIdAsync(id);
        
        if (note == null || note.DateDeleted != null)
        {
            throw new KeyNotFoundException($"CaseNote with ID {id} not found");
        }
        
        _mapper.Map(dto, note);
        
        await _unitOfWork.CaseNotes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CaseNoteDto>(note);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.CaseNotes.GetByIdAsync(id);
        
        if (note == null)
        {
            return false;
        }
        
        // Soft delete
        note.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseNotes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
