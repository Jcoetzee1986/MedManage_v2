using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class CaseService : ICaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CaseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CaseDto?> GetByIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _unitOfWork.Cases.GetByIdAsync(caseId);
        return caseEntity == null ? null : _mapper.Map<CaseDto>(caseEntity);
    }

    public async Task<PagedResult<CaseDto>> SearchAsync(CaseSearchRequest request, CancellationToken cancellationToken = default)
    {
        var allCases = await _unitOfWork.Cases.GetAllAsync();
        var query = allCases.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.AuthNumber))
        {
            query = query.Where(c => c.AuthNumber != null && c.AuthNumber.Contains(request.AuthNumber));
        }

        if (request.MemberId.HasValue)
        {
            query = query.Where(c => c.MemberId == request.MemberId.Value);
        }

        if (request.StatusId.HasValue)
        {
            query = query.Where(c => c.StatusId == request.StatusId.Value);
        }

        if (request.ReferToId.HasValue)
        {
            query = query.Where(c => c.ReferToId == request.ReferToId.Value);
        }

        if (request.ReferFromId.HasValue)
        {
            query = query.Where(c => c.ReferFromId == request.ReferFromId.Value);
        }

        if (request.AdmissionDateFrom.HasValue)
        {
            query = query.Where(c => c.AdmissionDate >= request.AdmissionDateFrom.Value);
        }

        if (request.AdmissionDateTo.HasValue)
        {
            query = query.Where(c => c.AdmissionDate <= request.AdmissionDateTo.Value);
        }

        if (request.DischargeDateFrom.HasValue)
        {
            query = query.Where(c => c.DischargeDate >= request.DischargeDateFrom.Value);
        }

        if (request.DischargeDateTo.HasValue)
        {
            query = query.Where(c => c.DischargeDate <= request.DischargeDateTo.Value);
        }

        if (request.CaseCategoryId.HasValue)
        {
            query = query.Where(c => c.CaseCategoryId == request.CaseCategoryId.Value);
        }

        // Apply soft delete filter
        query = query.Where(c => c.DateDeleted == null);

        // Get total count
        var totalCount = query.Count();

        // Apply pagination
        var cases = query
            .OrderByDescending(c => c.DateCreated)
            .ThenByDescending(c => c.CaseId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<CaseDto>
        {
            Items = _mapper.Map<List<CaseDto>>(cases),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<CaseDto> CreateAsync(CreateCaseRequest request, CancellationToken cancellationToken = default)
    {
        var caseEntity = _mapper.Map<Case>(request);
        
        // Set audit fields
        caseEntity.UserID = _currentUserService.UserId ?? "SYSTEM";
        
        await _unitOfWork.Cases.AddAsync(caseEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CaseDto>(caseEntity);
    }

    public async Task<CaseDto> UpdateAsync(UpdateCaseRequest request, CancellationToken cancellationToken = default)
    {
        var existingCase = await _unitOfWork.Cases.GetByIdAsync(request.CaseId);
        if (existingCase == null)
        {
            throw new KeyNotFoundException($"Case with ID {request.CaseId} not found");
        }

        _mapper.Map(request, existingCase);
        
        // Set update audit fields
        existingCase.DateUpdated = DateTime.UtcNow;
        existingCase.UpdatedUserID = _currentUserService.UserId ?? "SYSTEM";
        
        await _unitOfWork.Cases.UpdateAsync(existingCase);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CaseDto>(existingCase);
    }

    public async Task<bool> DeleteAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _unitOfWork.Cases.GetByIdAsync(caseId);
        if (caseEntity == null)
        {
            return false;
        }

        await _unitOfWork.Cases.DeleteAsync(caseEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _unitOfWork.Cases.GetByIdAsync(caseId);
        return caseEntity != null;
    }
}
