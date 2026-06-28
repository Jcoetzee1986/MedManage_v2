using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.CaseNappi;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class CaseNappiService : ICaseNappiService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MedManageDbContext _dbContext;

    public CaseNappiService(IUnitOfWork unitOfWork, MedManageDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CaseNappiDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var items = await _dbContext.CaseNappiCodes
            .Where(cn => cn.CaseId == caseId && cn.DateDeleted == null)
            .Join(_dbContext.NappiCodes,
                cn => cn.NappiId,
                n => n.NappiId,
                (cn, n) => new CaseNappiDto
                {
                    CaseIdNappiId = cn.CaseIdNappiId,
                    CaseId = cn.CaseId,
                    NappiId = cn.NappiId,
                    NappiCode = n.Code,
                    NappiDescription = n.Description,
                    Price1 = n.Price1,
                    Value = cn.Value,
                    Quantity = cn.Quantity,
                    Measure = n.Measure,
                    Units = n.Units,
                    Dispensary = cn.Dispensary,
                    Ward = cn.Ward,
                    Theater = cn.Theater,
                    Tto = cn.Tto,
                    _0201 = cn._0201,
                    Date = cn.Date,
                    DateInserted = cn.DateInserted,
                    DateModified = cn.DateUpdated
                })
            .ToListAsync(cancellationToken);

        return items;
    }

    public async Task<CaseNappiDto?> GetByIdAsync(int caseId, int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseNappiCodes.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            return null;

        return entity.ToDto();
    }

    public async Task<CaseNappiDto> CreateAsync(int caseId, CreateCaseNappiDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        entity.CaseId = caseId;
        entity.DateInserted = DateTime.UtcNow;

        await _unitOfWork.CaseNappiCodes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }

    public async Task<CaseNappiDto> UpdateAsync(int caseId, int id, UpdateCaseNappiDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CaseNappiCodes.GetByIdAsync(id);
        if (entity == null || entity.CaseId != caseId || entity.DateDeleted != null)
            throw new KeyNotFoundException($"Case NAPPI code with ID {id} not found for case {caseId}");

        dto.ApplyTo(entity);
        entity.CaseId = caseId;
        entity.DateUpdated = DateTime.UtcNow;

        await _unitOfWork.CaseNappiCodes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
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
