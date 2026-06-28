using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.CaseFacilityType;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseFacilityTypeService : ICaseFacilityTypeService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseFacilityTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseFacilityTypeDto>> GetByCaseIdAsync(int caseId)
    {
        var facilityTypes = await _unitOfWork.CaseFacilityTypes.GetByCaseIdAsync(caseId);
        return facilityTypes.Select(e => e.ToDto());
    }

    public async Task<CaseFacilityTypeDto?> GetByIdAsync(int id)
    {
        var facilityType = await _unitOfWork.CaseFacilityTypes.GetByIdAsync(id);
        if (facilityType == null || facilityType.DateDeleted != null)
            return null;

        return facilityType.ToDto();
    }

    public async Task<CaseFacilityTypeDto> CreateAsync(CreateCaseFacilityTypeRequest request)
    {
        var entity = request.ToEntity();
        entity.DateInserted = DateTime.Now;

        // Calculate LOS from dates if not explicitly provided
        if (!entity.Los.HasValue && entity.DateDischarged.HasValue)
        {
            var days = (entity.DateDischarged.Value - entity.DateAdmitted).TotalDays;
            entity.Los = (decimal)days;
        }

        await _unitOfWork.CaseFacilityTypes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<CaseFacilityTypeDto> UpdateAsync(int id, UpdateCaseFacilityTypeRequest request)
    {
        var entity = await _unitOfWork.CaseFacilityTypes.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
            throw new KeyNotFoundException($"CaseFacilityType with ID {id} not found");

        request.ApplyTo(entity);
        entity.DateUpdated = DateTime.Now;

        // Recalculate LOS from dates if not explicitly provided
        if (!entity.Los.HasValue && entity.DateDischarged.HasValue)
        {
            var days = (entity.DateDischarged.Value - entity.DateAdmitted).TotalDays;
            entity.Los = (decimal)days;
        }

        await _unitOfWork.CaseFacilityTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.CaseFacilityTypes.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
            return false;

        entity.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseFacilityTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
