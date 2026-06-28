using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseRepository : IRepository<Case>
{
    Task<Case?> GetByIdWithDetailsAsync(int caseId);
    Task<IEnumerable<Case>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Case>> GetByMemberIdExcludingCaseAsync(int memberId, int excludeCaseId);
    Task<IEnumerable<Case>> GetLast30CasesAsync();
    Task<IEnumerable<Case>> SearchByFiltersAsync(
        string? claimNumber,
        string? memberNumber,
        int? serviceProviderId,
        int? statusId,
        DateTime? admissionDateFrom,
        DateTime? admissionDateTo,
        bool includeDeleted = false);
    Task<IEnumerable<Case>> GetPossibleDuplicatesAsync(int memberId, DateTime admissionDate);
    Task<IEnumerable<Case>> GetByRemittanceNumberAsync(string remittanceNumber);
    Task CopyCaseAsync(int sourceCaseId, int newMemberId);
    Task UpdateStatusAsync(int caseId, int statusId);
    Task UpdateMemberMedicalAidAsync(int caseId);
}
