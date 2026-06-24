using AutoMapper;
using MedManage.Core.DTOs.MemberNote;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MemberNoteService : IMemberNoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MemberNoteService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<MemberNoteDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _unitOfWork.MemberNotes.GetAllAsync();
        
        if (!includeDeleted)
        {
            notes = notes.Where(n => n.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<MemberNoteDto>>(notes);
    }

    public async Task<MemberNoteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.MemberNotes.GetByIdAsync(id);
        
        if (note == null || note.DateDeleted != null)
        {
            return null;
        }
        
        return _mapper.Map<MemberNoteDto>(note);
    }

    public async Task<IEnumerable<MemberNoteDto>> GetByMemberIdAsync(int memberId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var notes = await _unitOfWork.MemberNotes.GetAllAsync();
        
        notes = notes.Where(n => n.MemberId == memberId);
        
        if (!includeDeleted)
        {
            notes = notes.Where(n => n.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<MemberNoteDto>>(notes.OrderByDescending(n => n.DateCreated ?? n.DateInserted));
    }

    public async Task<MemberNoteDto> CreateAsync(CreateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        var note = _mapper.Map<MemberNote>(dto);
        
        // Set DateCreated to now if not provided
        if (!note.DateCreated.HasValue)
        {
            note.DateCreated = DateTime.Now;
        }
        
        await _unitOfWork.MemberNotes.AddAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MemberNoteDto>(note);
    }

    public async Task<MemberNoteDto> UpdateAsync(int id, UpdateMemberNoteDto dto, CancellationToken cancellationToken = default)
    {
        var note = await _unitOfWork.MemberNotes.GetByIdAsync(id);
        
        if (note == null || note.DateDeleted != null)
        {
            throw new KeyNotFoundException($"MemberNote with ID {id} not found");
        }
        
        _mapper.Map(dto, note);
        
        await _unitOfWork.MemberNotes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<MemberNoteDto>(note);
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
