using AutoMapper;
using MedManage.Core.DTOs.CaseNappi;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseNappiService : ICaseNappiService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseNappiService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseNappiDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.CaseNappiCodes.GetByCaseIdAsync(caseId);
        return _mapper.Map<IEnumerable<CaseNappiDto>>(items);
    }

    public async Task<CaseNappiDto?> GetByIdAsync(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseNappiCodes.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            return null;

        return _mapper.Map<CaseNappiDto>(entity);
    }

    public async Task<CaseNappiDto> CreateAsync(int caseId, CreateCaseNappiDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CaseNappiCode>(dto);
        entity.CaseId = caseId;
        entity.DateInserted = DateTime.UtcNow;

        await _unitOfWork.CaseNappiCodes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CaseNappiDto>(entity);
    }

    public async Task<CaseNappiDto> UpdateAsync(int caseId, int id, UpdateCaseNappiDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseNappiCodes.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            throw new KeyNotFoundException($"Case NAPPI code with ID {id} not found for case {caseId}");

        _mapper.Map(dto, entity);
        entity.CaseId = caseId;
        entity.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.CaseNappiCodes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CaseNappiDto>(entity);
    }

    public async Task<bool> DeleteAsync(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseNappiCodes.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            return false;

        // Soft delete
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseNappiCodes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
