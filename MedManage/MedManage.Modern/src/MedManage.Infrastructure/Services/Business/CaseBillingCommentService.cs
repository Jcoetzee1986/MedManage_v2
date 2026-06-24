using AutoMapper;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class CaseBillingCommentService : ICaseBillingCommentService
{
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CaseBillingCommentService(MedManageDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CaseBillingCommentDto>> GetByBillingIdAsync(int billingId)
    {
        var comments = await _context.CaseBillingComments
            .Where(c => c.CaseBillingId == billingId && c.DateDeleted == null)
            .OrderByDescending(c => c.DateInserted)
            .Select(c => new CaseBillingCommentDto
            {
                CaseBillingCommentId = c.CaseBillingCommentId,
                CaseBillingId = c.CaseBillingId,
                Comment = c.Comment,
                DateInserted = c.DateInserted,
                UserID = c.UserID
            })
            .ToListAsync();

        return comments;
    }

    public async Task<CaseBillingCommentDto> CreateAsync(int billingId, CreateCaseBillingCommentDto dto)
    {
        var entity = new CaseBillingComment
        {
            CaseBillingId = billingId,
            Comment = dto.Comment,
            DateInserted = DateTime.UtcNow,
            UserID = _currentUserService.UserId
        };

        _context.CaseBillingComments.Add(entity);
        await _context.SaveChangesAsync();

        return new CaseBillingCommentDto
        {
            CaseBillingCommentId = entity.CaseBillingCommentId,
            CaseBillingId = entity.CaseBillingId,
            Comment = entity.Comment,
            DateInserted = entity.DateInserted,
            UserID = entity.UserID
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.CaseBillingComments
            .FirstOrDefaultAsync(c => c.CaseBillingCommentId == id && c.DateDeleted == null);

        if (entity == null)
            return false;

        entity.DateDeleted = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
