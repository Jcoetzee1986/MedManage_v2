using AutoMapper;
using MedManage.Core.DTOs.CaseIcd;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseIcdService : ICaseIcdService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseIcdService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseIcdDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        return _mapper.Map<IEnumerable<CaseIcdDto>>(items);
    }

    public async Task<CaseIcdDto?> GetByIdAsync(int caseId, int icdId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var entity = items.FirstOrDefault(x => x.Icdid == icdId);
        if (entity == null)
            return null;

        return _mapper.Map<CaseIcdDto>(entity);
    }

    public async Task<CaseIcdDto> CreateAsync(int caseId, CreateCaseIcdDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CaseIcd>(dto);
        entity.CaseId = caseId;
        entity.DateInserted = DateTime.UtcNow;

        await _unitOfWork.CaseIcds.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes to get ICD code info
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var created = items.FirstOrDefault(x => x.Icdid == entity.Icdid);
        return _mapper.Map<CaseIcdDto>(created ?? entity);
    }

    public async Task<CaseIcdDto> UpdateAsync(int caseId, int icdId, UpdateCaseIcdDto dto, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var entity = items.FirstOrDefault(x => x.Icdid == icdId);
        if (entity == null)
            throw new KeyNotFoundException($"Case ICD with CaseId {caseId} and ICDID {icdId} not found");

        _mapper.Map(dto, entity);
        entity.CaseId = caseId;
        entity.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.CaseIcds.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with includes
        var reloaded = await _unitOfWork.CaseIcds.GetByCaseIdAsync(caseId);
        var updated = reloaded.FirstOrDefault(x => x.Icdid == icdId);
        return _mapper.Map<CaseIcdDto>(updated ?? entity);
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
