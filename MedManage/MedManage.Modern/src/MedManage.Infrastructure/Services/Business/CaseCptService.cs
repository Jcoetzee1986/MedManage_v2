using AutoMapper;
using MedManage.Core.DTOs.CaseCpt;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseCptService : ICaseCptService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseCptService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseCptDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseCpts.GetByCaseIdAsync(caseId);
        return _mapper.Map<IEnumerable<CaseCptDto>>(items);
    }

    public async Task<CaseCptDto?> GetByIdAsync(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseCpts.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            return null;

        return _mapper.Map<CaseCptDto>(entity);
    }

    public async Task<CaseCptDto> CreateAsync(int caseId, CreateCaseCptDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CaseCpt>(dto);
        entity.CaseId = caseId;
        entity.DateInserted = DateTime.UtcNow;

        await _unitOfWork.CaseCpts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes to get CPT code info
        var items = await _unitOfWork.CaseCpts.GetByCaseIdAsync(caseId);
        var created = items.FirstOrDefault(x => x.CaseIdCptid == entity.CaseIdCptid);
        return _mapper.Map<CaseCptDto>(created ?? entity);
    }

    public async Task<CaseCptDto> UpdateAsync(int caseId, int id, UpdateCaseCptDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseCpts.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            throw new KeyNotFoundException($"Case CPT with ID {id} not found for case {caseId}");

        _mapper.Map(dto, entity);
        entity.CaseId = caseId;
        entity.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.CaseCpts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes
        var items = await _unitOfWork.CaseCpts.GetByCaseIdAsync(caseId);
        var updated = items.FirstOrDefault(x => x.CaseIdCptid == id);
        return _mapper.Map<CaseCptDto>(updated ?? entity);
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
