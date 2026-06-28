using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.CaseLink;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class CaseLinkService : ICaseLinkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MedManageDbContext _context;

    public CaseLinkService(IUnitOfWork unitOfWork, MedManageDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<LinkedCaseDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var links = await _unitOfWork.CaseLinks.GetByCaseIdAsync(caseId);
        
        var result = new List<LinkedCaseDto>();
        foreach (var link in links)
        {
            int linkedCaseId;
            string relationship;

            if (link.ParentCase == caseId)
            {
                linkedCaseId = link.ChildCase;
                relationship = "child";
            }
            else
            {
                linkedCaseId = link.ParentCase;
                relationship = "parent";
            }

            // Enrich with case details
            var caseDetails = await _context.Cases
                .Where(c => c.CaseId == linkedCaseId && c.DateDeleted == null)
                .Select(c => new
                {
                    c.AuthNumber,
                    c.AdmissionDate,
                    CaseStatus = c.Status != null ? c.Status.CaseStatus1 : null,
                    CaseType = c.AuthType != null ? c.AuthType.CaseType1 : null,
                    CaseCategory = c.CaseCategoryId != null
                        ? _context.CaseCategories.Where(cc => cc.CaseCategoryId == c.CaseCategoryId).Select(cc => cc.CaseCategory1).FirstOrDefault()
                        : null,
                    ReferToPractice = c.ReferTo != null ? c.ReferTo.PracticeName : null,
                    MemberName = c.Member != null ? (c.Member.Name ?? "") + " " + (c.Member.Surname ?? "") : null
                })
                .FirstOrDefaultAsync(cancellationToken);

            result.Add(new LinkedCaseDto
            {
                CaseId = linkedCaseId,
                Relationship = relationship,
                AuthNumber = caseDetails?.AuthNumber,
                AdmissionDate = caseDetails?.AdmissionDate?.ToString("yyyy-MM-dd"),
                CaseStatus = caseDetails?.CaseStatus,
                CaseType = caseDetails?.CaseType,
                CaseCategory = caseDetails?.CaseCategory,
                ReferToPractice = caseDetails?.ReferToPractice,
                MemberName = caseDetails?.MemberName?.Trim()
            });
        }
        return result;
    }

    public async Task<CaseLinkDto> CreateAsync(int caseId, CreateCaseLinkDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new CaseLink
        {
            ParentCase = caseId,
            ChildCase = dto.ChildCase,
            DateInserted = DateTime.UtcNow
        };

        await _unitOfWork.CaseLinks.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, int linkedCaseId, CancellationToken cancellationToken = default)
    {
        // Find the link in either direction
        var links = await _unitOfWork.CaseLinks.GetByCaseIdAsync(caseId);
        var link = links.FirstOrDefault(l =>
            (l.ParentCase == caseId && l.ChildCase == linkedCaseId) ||
            (l.ChildCase == caseId && l.ParentCase == linkedCaseId));

        if (link == null)
            return false;

        // Soft delete
        link.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseLinks.UpdateAsync(link);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
