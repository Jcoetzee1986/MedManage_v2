using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.CaseLink;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseLinkService : ICaseLinkService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseLinkService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<LinkedCaseDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var links = await _unitOfWork.CaseLinks.GetByCaseIdAsync(caseId);
        
        var result = new List<LinkedCaseDto>();
        foreach (var link in links)
        {
            if (link.ParentCase == caseId)
            {
                result.Add(new LinkedCaseDto { CaseId = link.ChildCase, Relationship = "child" });
            }
            else
            {
                result.Add(new LinkedCaseDto { CaseId = link.ParentCase, Relationship = "parent" });
            }
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
