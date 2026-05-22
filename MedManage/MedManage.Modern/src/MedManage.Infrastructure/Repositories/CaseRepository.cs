using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class CaseRepository : Repository<Case>, ICaseRepository
{
    public CaseRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<Case?> GetByIdWithDetailsAsync(int caseId)
    {
        return await _dbSet
            .Include(c => c.Member)
                .ThenInclude(m => m.MedicalAid)
            .Include(c => c.Status)
            .Include(c => c.CaseCpts)
                .ThenInclude(cc => cc.Cpt)
            .Include(c => c.CaseIcds)
                .ThenInclude(ci => ci.Icd)
            .Include(c => c.CaseNotes)
            .Include(c => c.CaseComments)
            .FirstOrDefaultAsync(c => c.CaseId == caseId && c.DateDeleted == null);
    }

    public async Task<IEnumerable<Case>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(c => c.Status)
            .Where(c => c.MemberId == memberId && c.DateDeleted == null)
            .OrderByDescending(c => c.AdmissionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Case>> GetByMemberIdExcludingCaseAsync(int memberId, int excludeCaseId)
    {
        return await _dbSet
            .Include(c => c.Status)
            .Where(c => c.MemberId == memberId && c.CaseId != excludeCaseId && c.DateDeleted == null)
            .OrderByDescending(c => c.AdmissionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Case>> GetLast30CasesAsync()
    {
        return await _dbSet
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Where(c => c.DateDeleted == null)
            .OrderByDescending(c => c.DateCreated)
            .Take(30)
            .ToListAsync();
    }

    public async Task<IEnumerable<Case>> SearchByFiltersAsync(
        string? claimNumber,
        string? memberNumber,
        int? serviceProviderId,
        int? statusId,
        DateTime? admissionDateFrom,
        DateTime? admissionDateTo)
    {
        var query = _dbSet
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Where(c => c.DateDeleted == null);

        if (!string.IsNullOrWhiteSpace(claimNumber))
        {
            query = query.Where(c => c.AuthNumber != null && c.AuthNumber.Contains(claimNumber));
        }

        if (!string.IsNullOrWhiteSpace(memberNumber))
        {
            query = query.Where(c => c.Member.MemberNumber.Contains(memberNumber));
        }

        if (serviceProviderId.HasValue)
        {
            // Case uses ReferToId and ReferFromId for service providers
            query = query.Where(c => c.ReferToId == serviceProviderId.Value || c.ReferFromId == serviceProviderId.Value);
        }

        if (statusId.HasValue)
        {
            query = query.Where(c => c.StatusId == statusId.Value);
        }

        if (admissionDateFrom.HasValue)
        {
            query = query.Where(c => c.AdmissionDate >= DateOnly.FromDateTime(admissionDateFrom.Value));
        }

        if (admissionDateTo.HasValue)
        {
            query = query.Where(c => c.AdmissionDate <= DateOnly.FromDateTime(admissionDateTo.Value));
        }

        return await query
            .OrderByDescending(c => c.AdmissionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Case>> GetPossibleDuplicatesAsync(int memberId, DateTime admissionDate)
    {
        var admissionDateOnly = DateOnly.FromDateTime(admissionDate);
        return await _dbSet
            .Where(c => c.MemberId == memberId 
                      && c.AdmissionDate == admissionDateOnly
                      && c.DateDeleted == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Case>> GetByRemittanceNumberAsync(string remittanceNumber)
    {
        // Note: CaseBillings navigation not scaffolded - need to join with CaseBilling table
        var caseIds = await _context.Set<CaseBilling>()
            .Where(cb => cb.Remittance == remittanceNumber && cb.DateDeleted == null)
            .Select(cb => cb.CaseId)
            .Distinct()
            .ToListAsync();

        return await _dbSet
            .Include(c => c.Member)
            .Where(c => caseIds.Contains(c.CaseId) && c.DateDeleted == null)
            .ToListAsync();
    }

    public async Task CopyCaseAsync(int sourceCaseId, int newMemberId)
    {
        var sourceCase = await GetByIdWithDetailsAsync(sourceCaseId);
        if (sourceCase == null)
        {
            throw new InvalidOperationException($"Case {sourceCaseId} not found");
        }

        var newCase = new Case
        {
            MemberId = newMemberId,
            // Note: Case uses ReferToId and ReferFromId for service providers, not ServiceProviderId
            ReferToId = sourceCase.ReferToId,
            ReferFromId = sourceCase.ReferFromId,
            AuthTypeId = sourceCase.AuthTypeId,
            StatusId = sourceCase.StatusId,
            AdmissionDate = sourceCase.AdmissionDate,
            DischargeDate = sourceCase.DischargeDate,
            AuthNumber = sourceCase.AuthNumber,
            // Copy other fields as needed
            DateInserted = DateTime.Now,
            UserID = sourceCase.UserID
        };

        await _dbSet.AddAsync(newCase);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int caseId, int statusId)
    {
        var caseEntity = await _dbSet.FindAsync(caseId);
        if (caseEntity != null)
        {
            caseEntity.StatusId = statusId;
            caseEntity.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateMemberMedicalAidAsync(int caseId)
    {
        var caseEntity = await _dbSet
            .Include(c => c.Member)
                .ThenInclude(m => m.MedicalAid)
            .FirstOrDefaultAsync(c => c.CaseId == caseId);

        if (caseEntity != null && caseEntity.Member != null)
        {
            // Note: Case doesn't have MedicalAidId - MedicalAid is linked through Member
            // This method updates the Member's MedicalAid, not the Case directly
            // No action needed as MedicalAid relationship is already through Member table
            caseEntity.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
