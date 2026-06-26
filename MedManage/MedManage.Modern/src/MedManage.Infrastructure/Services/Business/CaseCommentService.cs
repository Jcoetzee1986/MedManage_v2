using MedManage.Infrastructure.Mapping.Manual;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseComment;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseCommentService : ICaseCommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseCommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseCommentDto>> GetAllAsync()
    {
        var caseComments = await _unitOfWork.CaseComments.GetAllAsync();
        return caseComments.Select(e => e.ToDto());
    }

    public async Task<CaseCommentDto?> GetByIdAsync(int id)
    {
        var caseComment = await _unitOfWork.CaseComments.GetByIdAsync(id);
        return caseComment == null ? null : caseComment.ToDto();
    }

    public async Task<IEnumerable<CaseCommentDto>> GetByCaseIdAsync(int caseId)
    {
        var caseComments = await _unitOfWork.CaseComments
            .FindAsync(cc => cc.CaseId == caseId);
        
        var sortedComments = caseComments.OrderByDescending(cc => cc.DateCreated);
        return sortedComments.Select(e => e.ToDto());
    }

    public async Task<CaseCommentDto> CreateAsync(CreateCaseCommentDto dto)
    {
        var caseComment = dto.ToEntity();
        caseComment.DateInserted = DateTime.UtcNow;
        
        await _unitOfWork.CaseComments.AddAsync(caseComment);
        await _unitOfWork.SaveChangesAsync();
        
        return caseComment.ToDto();
    }

    public async Task<CaseCommentDto> UpdateAsync(int id, UpdateCaseCommentDto dto)
    {
        var caseComment = await _unitOfWork.CaseComments.GetByIdAsync(id);
        if (caseComment == null)
            throw new KeyNotFoundException($"CaseComment with ID {id} not found");

        dto.ApplyTo(caseComment);
        caseComment.DateUpdated = DateTime.UtcNow;
        
        await _unitOfWork.CaseComments.UpdateAsync(caseComment);
        await _unitOfWork.SaveChangesAsync();
        
        return caseComment.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var caseComment = await _unitOfWork.CaseComments.GetByIdAsync(id);
        if (caseComment == null)
            return false;

        caseComment.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseComments.UpdateAsync(caseComment);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}
