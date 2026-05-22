using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedManage.Core.DTOs.CaseExclusion;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseExclusionService : ICaseExclusionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseExclusionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseExclusionDto>> GetAllAsync()
    {
        var exclusions = await _unitOfWork.CaseExclusions
            .FindAsync(c => c.DateDeleted == null);
        return _mapper.Map<IEnumerable<CaseExclusionDto>>(exclusions.OrderByDescending(c => c.DateInserted));
    }

    public async Task<CaseExclusionDto?> GetByIdAsync(int caseId, int exclusionId)
    {
        var exclusion = (await _unitOfWork.CaseExclusions
            .FindAsync(c => c.CaseId == caseId && c.ExclusionId == exclusionId && c.DateDeleted == null))
            .FirstOrDefault();

        return exclusion == null ? null : _mapper.Map<CaseExclusionDto>(exclusion);
    }

    public async Task<IEnumerable<CaseExclusionDto>> GetByCaseIdAsync(int caseId)
    {
        var exclusions = await _unitOfWork.CaseExclusions
            .FindAsync(c => c.CaseId == caseId && c.DateDeleted == null);
        return _mapper.Map<IEnumerable<CaseExclusionDto>>(exclusions.OrderByDescending(c => c.DateInserted));
    }

    public async Task<CaseExclusionDto> CreateAsync(CreateCaseExclusionDto dto)
    {
        var exclusion = _mapper.Map<CaseExclusion>(dto);
        exclusion.DateInserted = DateTime.Now;

        await _unitOfWork.CaseExclusions.AddAsync(exclusion);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseExclusionDto>(exclusion);
    }

    public async Task<CaseExclusionDto> UpdateAsync(int caseId, int exclusionId, UpdateCaseExclusionDto dto)
    {
        var exclusion = (await _unitOfWork.CaseExclusions
            .FindAsync(c => c.CaseId == caseId && c.ExclusionId == exclusionId && c.DateDeleted == null))
            .FirstOrDefault();

        if (exclusion == null)
            throw new KeyNotFoundException($"CaseExclusion with CaseId {caseId} and ExclusionId {exclusionId} not found");

        _mapper.Map(dto, exclusion);
        exclusion.DateUpdated = DateTime.Now;

        await _unitOfWork.CaseExclusions.UpdateAsync(exclusion);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseExclusionDto>(exclusion);
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
