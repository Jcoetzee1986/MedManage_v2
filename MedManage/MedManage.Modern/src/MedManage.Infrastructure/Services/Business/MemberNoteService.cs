using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.MemberNote;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MemberNoteService : IMemberNoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public MemberNoteService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<MemberNoteDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _unitOfWork.MemberNotes.GetAllAsync();
        
        if (!includeDeleted)
        {
            notes = notes.Where(n => n.DateDeleted == null);
        }
        
        return notes.Select(e => e.ToDto());
    }

    public async Task<MemberNoteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.MemberNotes.GetByIdAsync(id);
        
        if (note == null || note.DateDeleted != null)
        {
            return null;
        }
        
        return note.ToDto();
    }

    public async Task<IEnumerable<MemberNoteDto>> GetByMemberIdAsync(int memberId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _unitOfWork.MemberNotes.GetAllAsync();
        
        notes = notes.Where(n => n.MemberId == memberId);
        
        if (!includeDeleted)
        {
            notes = notes.Where(n => n.DateDeleted == null);
        }
        
        return notes.OrderByDescending(n => n.DateCreated ?? n.DateInserted).Select(e => e.ToDto());
    }

    public async Task<MemberNoteDto> CreateAsync(CreateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        var note = dto.ToEntity();
        
        // Set DateCreated to now if not provided
        if (!note.DateCreated.HasValue)
        {
            note.DateCreated = DateTime.Now;
        }
        
        await _unitOfWork.MemberNotes.AddAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return note.ToDto();
    }

    public async Task<MemberNoteDto> UpdateAsync(int id, UpdateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.MemberNotes.GetByIdAsync(id);
        
        if (note == null || note.DateDeleted != null)
        {
            throw new KeyNotFoundException($"MemberNote with ID {id} not found");
        }
        
        dto.ApplyTo(note);
        
        await _unitOfWork.MemberNotes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return note.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.MemberNotes.GetByIdAsync(id);
        
        if (note == null)
        {
            return false;
        }
        
        // Soft delete
        note.DateDeleted = DateTime.Now;
        await _unitOfWork.MemberNotes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
