using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.CaseIcd;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseIcdService : ICaseIcdService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseIcdService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseIcdDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        return items.Select(e => e.ToDto());
    }

    public async Task<CaseIcdDto?> GetByIdAsync(int caseId, int icdId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var entity = items.FirstOrDefault(x => x.Icdid == icdId);
        if (entity == null)
            return null;

        return entity.ToDto();
    }

    public async Task<CaseIcdDto> CreateAsync(int caseId, CreateCaseIcdDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        entity.CaseId = caseId;
        entity.DateInserted = DateTime.UtcNow;

        await _unitOfWork.CaseIcds.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes to get ICD code info
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var created = items.FirstOrDefault(x => x.Icdid == entity.Icdid);
        return (created ?? entity).ToDto();
    }

    public async Task<CaseIcdDto> UpdateAsync(int caseId, int icdId, UpdateCaseIcdDto dto, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var entity = items.FirstOrDefault(x => x.Icdid == icdId);
        if (entity == null)
            throw new KeyNotFoundException($"Case ICD with CaseId {caseId} and ICDID {icdId} not found");

        dto.ApplyTo(entity);
        entity.CaseId = caseId;
        entity.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.CaseIcds.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes
        var reloaded = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var updated = reloaded.FirstOrDefault(x => x.Icdid == icdId);
        return (updated ?? entity).ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, int icdId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var entity = items.FirstOrDefault(x => x.Icdid == icdId);
        if (entity == null)
            return false;

        // Soft delete
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseIcds.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
