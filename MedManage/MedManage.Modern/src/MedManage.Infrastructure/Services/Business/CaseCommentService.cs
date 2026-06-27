using MedManage.Infrastructure.Mapping.Manual;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseComment;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class CaseCommentService : ICaseCommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MedManageDbContext _dbContext;

    public CaseCommentService(IUnitOfWork unitOfWork, MedManageDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
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
            .FindAsync(cc => cc.CaseId == caseId && cc.DateDeleted == null);
        
        var sortedComments = caseComments.OrderByDescending(cc => cc.DateCreated ?? cc.DateInserted);
        var dtos = sortedComments.Select(e => e.ToDto()).ToList();

        // Resolve user IDs to usernames
        var userIds = dtos
            .Where(d => !string.IsNullOrEmpty(d.UserID))
            .Select(d => d.UserID!)
            .Distinct()
            .ToList();

        if (userIds.Any())
        {
            // Try parsing as GUIDs for AspnetUsers lookup
            var guids = userIds
                .Select(id => Guid.TryParse(id, out var g) ? g : (Guid?)null)
                .Where(g => g.HasValue)
                .Select(g => g!.Value)
                .ToList();

            var userLookup = await _dbContext.AspnetUsers
                .Where(u => guids.Contains(u.UserId))
                .ToDictionaryAsync(u => u.UserId.ToString(), u => u.UserName);

            foreach (var dto in dtos)
            {
                if (!string.IsNullOrEmpty(dto.UserID) && userLookup.TryGetValue(dto.UserID, out var name))
                {
                    dto.UserName = name;
                }
            }
        }

        return dtos;
    }

    public async Task<CaseCommentDto> CreateAsync(CreateCaseCommentDto dto)
    {
        var caseComment = dto.ToEntity();
        caseComment.DateCreated = DateTime.Now;
        caseComment.DateInserted = DateTime.Now;
        
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
