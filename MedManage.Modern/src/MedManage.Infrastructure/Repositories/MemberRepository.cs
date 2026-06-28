using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Persistence;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(MedManageDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByIdWithDetailsAsync(int memberId)
    {
        return await _dbSet
            .Include(m => m.Title)
            .Include(m => m.Gender)
            .Include(m => m.MemberLanguage)
            .Include(m => m.MemberRace)
            .Include(m => m.MarritalStatus)
            .Include(m => m.MemberStatus)
            .Include(m => m.MedicalAid)
            .Include(m => m.MemberChronicIllnesses)
                .ThenInclude(mci => mci.ChronicIllness)
            .FirstOrDefaultAsync(m => m.MemberId == memberId);
    }

    public async Task<Member?> GetByMemberNumberAsync(string memberNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.MemberNumber == memberNumber);
    }

    public async Task<IEnumerable<Member>> SearchByFiltersAsync(
        string? memberNumber,
        string? firstName,
        string? lastName,
        string? idNumber,
        int? medicalAidId,
        int? statusId,
        bool includeDeleted = false)
    {
        IQueryable<Member> query = includeDeleted
            ? GetQueryableIncludingDeleted()
            : _dbSet;

        query = query
            .Include(m => m.Title)
            .Include(m => m.MedicalAid);

        if (!string.IsNullOrWhiteSpace(memberNumber))
        {
            query = query.Where(m => m.MemberNumber.Contains(memberNumber));
        }

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(m => m.Name != null && m.Name.Contains(firstName));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(m => m.Surname != null && m.Surname.Contains(lastName));
        }

        if (!string.IsNullOrWhiteSpace(idNumber))
        {
            query = query.Where(m => m.Idnumber != null && m.Idnumber.Contains(idNumber));
        }

        if (medicalAidId.HasValue)
        {
            query = query.Where(m => m.MedicalAidId == medicalAidId.Value);
        }

        if (statusId.HasValue)
        {
            query = query.Where(m => m.MemberStatusId == statusId.Value);
        }

        return await query
            .OrderBy(m => m.Surname)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<bool> MemberNumberExistsAsync(string memberNumber, int? excludeMemberId = null)
    {
        var query = _dbSet.Where(m => m.MemberNumber == memberNumber);

        if (excludeMemberId.HasValue)
        {
            query = query.Where(m => m.MemberId != excludeMemberId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Member>> GetByMedicalAidAsync(int medicalAidId)
    {
        return await _dbSet
            .Where(m => m.MedicalAidId == medicalAidId)
            .OrderBy(m => m.Surname)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }
}
