using MedManage.Infrastructure.Mapping.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseExclusion;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class CaseExclusionService : ICaseExclusionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MedManageDbContext _dbContext;

    public CaseExclusionService(IUnitOfWork unitOfWork, MedManageDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CaseExclusionDto>> GetAllAsync()
    {
        var exclusions = await _unitOfWork.CaseExclusions
            .FindAsync(c => c.DateDeleted == null);
        return exclusions.OrderByDescending(c => c.DateInserted).Select(e => e.ToDto());
    }

    public async Task<CaseExclusionDto?> GetByIdAsync(int caseId, int exclusionId)
    {
        var exclusion = await _dbContext.Set<CaseExclusion>()
            .Include(c => c.Exclusion)
            .FirstOrDefaultAsync(c => c.CaseId == caseId && c.ExclusionId == exclusionId && c.DateDeleted == null);

        return exclusion == null ? null : exclusion.ToDto();
    }

    public async Task<IEnumerable<CaseExclusionDto>> GetByCaseIdAsync(int caseId)
    {
        var exclusions = await _dbContext.Set<CaseExclusion>()
            .Include(c => c.Exclusion)
            .Where(c => c.CaseId == caseId && c.DateDeleted == null)
            .OrderByDescending(c => c.DateInserted)
            .ToListAsync();

        return exclusions.Select(e => e.ToDto());
    }

    public async Task<CaseExclusionDto> CreateAsync(CreateCaseExclusionDto dto)
    {
        var exclusion = dto.ToEntity();
        exclusion.DateInserted = DateTime.Now;

        await _unitOfWork.CaseExclusions.AddAsync(exclusion);
        await _unitOfWork.SaveChangesAsync();

        return exclusion.ToDto();
    }

    public async Task<CaseExclusionDto> UpdateAsync(int caseId, int exclusionId, UpdateCaseExclusionDto dto)
    {
        var exclusion = (await _unitOfWork.CaseExclusions
            .FindAsync(c => c.CaseId == caseId && c.ExclusionId == exclusionId && c.DateDeleted == null))
            .FirstOrDefault();

        if (exclusion == null)
            throw new KeyNotFoundException($"CaseExclusion with CaseId {caseId} and ExclusionId {exclusionId} not found");

        dto.ApplyTo(exclusion);
        exclusion.DateUpdated = DateTime.Now;

        await _unitOfWork.CaseExclusions.UpdateAsync(exclusion);
        await _unitOfWork.SaveChangesAsync();

        return exclusion.ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, int exclusionId)
    {
        var exclusion = (await _unitOfWork.CaseExclusions
            .FindAsync(c => c.CaseId == caseId && c.ExclusionId == exclusionId && c.DateDeleted == null))
            .FirstOrDefault();

        if (exclusion == null)
            return false;

        exclusion.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseExclusions.UpdateAsync(exclusion);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
