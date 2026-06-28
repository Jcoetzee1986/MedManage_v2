using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.CaseCpt;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseCptService : ICaseCptService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseCptService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseCptDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseCpts.GetByCaseIdAsync(caseId);
        return items.Select(e => e.ToDto());
    }

    public async Task<CaseCptDto?> GetByIdAsync(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseCpts.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            return null;

        return entity.ToDto();
    }

    public async Task<CaseCptDto> CreateAsync(int caseId, CreateCaseCptDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        entity.CaseId = caseId;
        entity.DateInserted = DateTime.UtcNow;

        await _unitOfWork.CaseCpts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes to get CPT code info
        var items = await _unitOfWork.CaseCpts.GetByCaseIdAsync(caseId);
        var created = items.FirstOrDefault(x => x.CaseIdCptid == entity.CaseIdCptid);
        return (created ?? entity).ToDto();
    }

    public async Task<CaseCptDto> UpdateAsync(int caseId, int id, UpdateCaseCptDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseCpts.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            throw new KeyNotFoundException($"Case CPT with ID {id} not found for case {caseId}");

        dto.ApplyTo(entity);
        entity.CaseId = caseId;
        entity.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.CaseCpts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes
        var items = await _unitOfWork.CaseCpts.GetByCaseIdAsync(caseId);
        var updated = items.FirstOrDefault(x => x.CaseIdCptid == id);
        return (updated ?? entity).ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseCpts.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            return false;

        // Soft delete
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseCpts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
