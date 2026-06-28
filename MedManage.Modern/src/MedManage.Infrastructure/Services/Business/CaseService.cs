using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class CaseService : ICaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CaseService(
        IUnitOfWork unitOfWork,
        MedManageDbContext context,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CaseDto?> GetByIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _context.Cases
            .Include(c => c.Member)
                .ThenInclude(m => m!.MedicalAid)
            .Include(c => c.Member)
                .ThenInclude(m => m!.MemberStatus)
            .Include(c => c.Status)
            .Include(c => c.AuthType)
            .Include(c => c.ReferTo)
                .ThenInclude(p => p!.Speciality)
            .Include(c => c.ReferFrom)
                .ThenInclude(p => p!.Speciality)
            .FirstOrDefaultAsync(c => c.CaseId == caseId && c.DateDeleted == null, cancellationToken);

        return caseEntity?.ToDto();
    }

    public async Task<PagedResult<CaseDto>> SearchAsync(CaseSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Cases
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Include(c => c.AuthType)
            .Include(c => c.ReferTo)
            .Include(c => c.ReferFrom)
            .Where(c => c.DateDeleted == null)
            .AsQueryable();

        // Core case filters
        if (!string.IsNullOrWhiteSpace(request.AuthNumber))
        {
            query = query.Where(c => c.AuthNumber != null && c.AuthNumber.Contains(request.AuthNumber));
        }

        if (request.MemberId.HasValue)
        {
            query = query.Where(c => c.MemberId == request.MemberId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.MemberNumber))
        {
            query = query.Where(c => c.Member != null && c.Member.MemberNumber != null
                && c.Member.MemberNumber.Contains(request.MemberNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.MemberSurname))
        {
            query = query.Where(c => c.Member != null && c.Member.Surname != null
                && c.Member.Surname.Contains(request.MemberSurname));
        }

        if (!string.IsNullOrWhiteSpace(request.MemberName))
        {
            query = query.Where(c => c.Member != null && c.Member.Name != null
                && c.Member.Name.Contains(request.MemberName));
        }

        if (request.StatusId.HasValue)
        {
            query = query.Where(c => c.StatusId == request.StatusId.Value);
        }

        if (request.CaseCategoryId.HasValue)
        {
            query = query.Where(c => c.CaseCategoryId == request.CaseCategoryId.Value);
        }

        if (request.AuthTypeId.HasValue)
        {
            query = query.Where(c => c.AuthTypeId == request.AuthTypeId.Value);
        }

        // Provider filters
        if (request.ReferToId.HasValue)
        {
            query = query.Where(c => c.ReferToId == request.ReferToId.Value);
        }

        if (request.ReferFromId.HasValue)
        {
            query = query.Where(c => c.ReferFromId == request.ReferFromId.Value);
        }

        // Practice name filter (matches both ReferTo and ReferFrom practice names)
        if (!string.IsNullOrWhiteSpace(request.PracticeName))
        {
            query = query.Where(c =>
                (c.ReferTo != null && c.ReferTo.PracticeName != null && c.ReferTo.PracticeName.Contains(request.PracticeName)) ||
                (c.ReferFrom != null && c.ReferFrom.PracticeName != null && c.ReferFrom.PracticeName.Contains(request.PracticeName)));
        }

        // Date filters
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

        if (request.DateCreatedFrom.HasValue)
        {
            query = query.Where(c => c.DateCreated >= request.DateCreatedFrom.Value);
        }

        if (request.DateCreatedTo.HasValue)
        {
            query = query.Where(c => c.DateCreated <= request.DateCreatedTo.Value);
        }

        // Medical aid filter (through member)
        if (request.MedicalAidId.HasValue)
        {
            query = query.Where(c => c.Member != null && c.Member.MedicalAidId == request.MedicalAidId.Value);
        }

        // ICD code filter - find cases that have a matching ICD code
        if (!string.IsNullOrWhiteSpace(request.IcdCode))
        {
            var icdCode = request.IcdCode;
            var caseIdsWithIcd = _context.CaseIcds
                .Include(ci => ci.Icd)
                .Where(ci => ci.Icd.DiagnosisCode != null && ci.Icd.DiagnosisCode.Contains(icdCode))
                .Select(ci => ci.CaseId);

            query = query.Where(c => caseIdsWithIcd.Contains(c.CaseId));
        }

        // CPT code filter - find cases that have a matching CPT code
        if (!string.IsNullOrWhiteSpace(request.CptCode))
        {
            var cptCode = request.CptCode;
            var caseIdsWithCpt = _context.CaseCpts
                .Include(cc => cc.Cpt)
                .Where(cc => cc.Cpt.Code != null && cc.Cpt.Code.Contains(cptCode))
                .Select(cc => cc.CaseId);

            query = query.Where(c => caseIdsWithCpt.Contains(c.CaseId));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = ApplySorting(query, request.SortBy, request.SortDescending);

        // Apply pagination
        var cases = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<CaseDto>
        {
            Items = cases.ToDtoList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<CaseDto> CreateAsync(CreateCaseRequest request, CancellationToken cancellationToken = default)
    {
        var caseEntity = request.ToEntity();

        // Ensure DateCreated is set
        if (!caseEntity.DateCreated.HasValue)
            caseEntity.DateCreated = DateOnly.FromDateTime(DateTime.Now);

        // Map frontend field names to entity fields
        if (request.CaseStatusId.HasValue && !caseEntity.StatusId.HasValue)
            caseEntity.StatusId = request.CaseStatusId;
        if (request.CaseTypeId.HasValue && !caseEntity.AuthTypeId.HasValue)
            caseEntity.AuthTypeId = request.CaseTypeId;

        // Set admission/discharge from request
        if (request.DateAdmitted != null && !caseEntity.AdmissionDate.HasValue)
        {
            if (DateOnly.TryParse(request.DateAdmitted, out var admDate))
                caseEntity.AdmissionDate = admDate;
        }
        if (request.DateDischarged != null && !caseEntity.DischargeDate.HasValue)
        {
            if (DateOnly.TryParse(request.DateDischarged, out var disDate))
                caseEntity.DischargeDate = disDate;
        }

        await _unitOfWork.Cases.AddAsync(caseEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Generate auth number: prefix from MainClient + CaseId
        if (string.IsNullOrEmpty(caseEntity.AuthNumber))
        {
            var prefix = "BWP"; // Default
            if (caseEntity.MemberId.HasValue)
            {
                var memberPrefix = await _context.Members
                    .Where(m => m.MemberId == caseEntity.MemberId.Value)
                    .Join(_context.MedicalAids, m => m.MedicalAidId, ma => ma.MedicalAidId, (m, ma) => ma.MainClientId)
                    .Join(_context.MainClients, mcId => mcId, mc => mc.MainClientId, (mcId, mc) => mc.MainClientName)
                    .FirstOrDefaultAsync(cancellationToken);

                if (memberPrefix != null)
                {
                    // Use first 3 chars of main client name as prefix
                    prefix = memberPrefix.Length >= 3 ? memberPrefix[..3].ToUpper() : memberPrefix.ToUpper();
                }
            }
            caseEntity.AuthNumber = $"{prefix}0{caseEntity.CaseId}";
            await _unitOfWork.Cases.UpdateAsync(caseEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Auto-populate checklist from templates
        var checklistTemplates = await _context.ChecklistTemplates
            .Where(ct => ct.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var template in checklistTemplates)
        {
            _context.CaseChecklists.Add(new Core.Entities.CaseChecklist
            {
                CaseId = caseEntity.CaseId,
                ChecklistTemplateId = template.ChecklistTemplateId,
                Checked = false,
                NotApplicable = false,
                Comments = "",
                Date = DateTime.Now
            });
        }
        if (checklistTemplates.Any())
            await _context.SaveChangesAsync(cancellationToken);

        return caseEntity.ToDto();
    }

    public async Task<CaseDto> UpdateAsync(UpdateCaseRequest request, CancellationToken cancellationToken = default)
    {
        var existingCase = await _unitOfWork.Cases.GetByIdAsync(request.CaseId);
        if (existingCase == null)
        {
            throw new KeyNotFoundException($"Case with ID {request.CaseId} not found");
        }

        request.ApplyTo(existingCase);

        await _unitOfWork.Cases.UpdateAsync(existingCase);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return existingCase.ToDto();
    }

    public async Task<CaseDto> PatchAsync(int caseId, Dictionary<string, object?> fields, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Cases.GetByIdAsync(caseId);
        if (entity == null)
            throw new KeyNotFoundException($"Case with ID {caseId} not found");

        foreach (var (key, value) in fields)
        {
            switch (key.ToLowerInvariant())
            {
                case "admissiondate":
                    entity.AdmissionDate = value == null ? null : DateOnly.Parse(value.ToString()!);
                    break;
                case "dischargedate":
                    entity.DischargeDate = value == null ? null : DateOnly.Parse(value.ToString()!);
                    break;
                case "interimamount":
                case "totalamount":
                    entity.TotalAmount = value == null ? null : Convert.ToDecimal(value);
                    break;
                case "totallengthofstay":
                    entity.TotalLengthOfStay = value == null ? null : Convert.ToDecimal(value);
                    break;
                case "penaltypercentage":
                    entity.PenaltyPercentage = value == null ? null : Convert.ToDecimal(value);
                    break;
                case "statusid":
                    entity.StatusId = value == null ? null : Convert.ToInt32(value);
                    break;
                case "casedescription":
                    entity.CaseDescription = value?.ToString();
                    break;
            }
        }

        entity.DateUpdated = DateTime.UtcNow;
        await _unitOfWork.Cases.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
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
        return await _context.Cases
            .AnyAsync(c => c.CaseId == caseId && c.DateDeleted == null, cancellationToken);
    }

    public async Task<DuplicateCheckResult> CheckDuplicateAsync(DuplicateCheckRequest request, CancellationToken cancellationToken = default)
    {
        var admissionDate = request.AdmissionDate;

        var query = _context.Cases
            .Include(c => c.Member)
            .Include(c => c.ReferTo)
            .Include(c => c.AuthType)
            .Where(c => c.DateDeleted == null
                && c.MemberId == request.MemberId
                && c.AdmissionDate == admissionDate);

        // Also match on provider if specified
        if (request.ReferToId.HasValue)
        {
            query = query.Where(c => c.ReferToId == request.ReferToId.Value);
        }

        // Exclude a specific case (for update scenarios)
        if (request.ExcludeCaseId.HasValue)
        {
            query = query.Where(c => c.CaseId != request.ExcludeCaseId.Value);
        }

        var duplicates = await query.ToListAsync(cancellationToken);

        return new DuplicateCheckResult
        {
            HasDuplicates = duplicates.Any(),
            PossibleDuplicates = duplicates.ToDtoList(),
            Message = duplicates.Any()
                ? $"Found {duplicates.Count} possible duplicate case(s) for the same member, provider, and admission date."
                : null
        };
    }

    private static IQueryable<Case> ApplySorting(IQueryable<Case> query, string? sortBy, bool sortDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "authnumber" => sortDescending
                ? query.OrderByDescending(c => c.AuthNumber)
                : query.OrderBy(c => c.AuthNumber),
            "admissiondate" => sortDescending
                ? query.OrderByDescending(c => c.AdmissionDate)
                : query.OrderBy(c => c.AdmissionDate),
            "dischargedate" => sortDescending
                ? query.OrderByDescending(c => c.DischargeDate)
                : query.OrderBy(c => c.DischargeDate),
            "status" or "casestatusname" => sortDescending
                ? query.OrderByDescending(c => c.StatusId)
                : query.OrderBy(c => c.StatusId),
            "member" or "membersurname" => sortDescending
                ? query.OrderByDescending(c => c.Member != null ? c.Member.Surname : "")
                : query.OrderBy(c => c.Member != null ? c.Member.Surname : ""),
            "membername" => sortDescending
                ? query.OrderByDescending(c => c.Member != null ? c.Member.Name : "")
                : query.OrderBy(c => c.Member != null ? c.Member.Name : ""),
            "membernumber" => sortDescending
                ? query.OrderByDescending(c => c.Member != null ? c.Member.MemberNumber : "")
                : query.OrderBy(c => c.Member != null ? c.Member.MemberNumber : ""),
            "refertopracticename" => sortDescending
                ? query.OrderByDescending(c => c.ReferTo != null ? c.ReferTo.PracticeName : "")
                : query.OrderBy(c => c.ReferTo != null ? c.ReferTo.PracticeName : ""),
            "casetypename" => sortDescending
                ? query.OrderByDescending(c => c.AuthTypeId)
                : query.OrderBy(c => c.AuthTypeId),
            "datecreated" => sortDescending
                ? query.OrderByDescending(c => c.DateCreated)
                : query.OrderBy(c => c.DateCreated),
            _ => query.OrderByDescending(c => c.DateCreated).ThenByDescending(c => c.CaseId)
        };
    }

    public async Task<IEnumerable<CaseDto>> GetMyCasesAsync(string userId, CancellationToken cancellationToken = default)
    {
        // Get case IDs that are currently locked by this user
        var lockedCaseIds = await _context.SessionUserCases
            .Where(s => s.UserID == userId)
            .Select(s => s.CaseId)
            .ToListAsync(cancellationToken);

        // Get cases created by or locked by this user
        var cases = await _context.Cases
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Include(c => c.AuthType)
            .Include(c => c.ReferTo)
            .Where(c => c.DateDeleted == null &&
                (c.UserID == userId || lockedCaseIds.Contains(c.CaseId)))
            .OrderByDescending(c => c.DateCreated)
            .Take(100)
            .ToListAsync(cancellationToken);

        return cases.Select(c => c.ToDto());
    }
}
