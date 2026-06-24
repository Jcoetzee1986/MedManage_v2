using AutoMapper;
using MedManage.Core.DTOs.CaseFacilityType;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseFacilityTypeService : ICaseFacilityTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseFacilityTypeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseFacilityTypeDto>> GetByCaseIdAsync(int caseId)
    {
        var facilityTypes = await _unitOfWork.CaseFacilityTypes.GetByCaseIdAsync(caseId);
        return _mapper.Map<IEnumerable<CaseFacilityTypeDto>>(facilityTypes);
    }

    public async Task<CaseFacilityTypeDto?> GetByIdAsync(int id)
    {
        var facilityType = await _unitOfWork.CaseFacilityTypes.GetByIdAsync(id);
        if (facilityType == null || facilityType.DateDeleted != null)
            return null;

        return _mapper.Map<CaseFacilityTypeDto>(facilityType);
    }

    public async Task<CaseFacilityTypeDto> CreateAsync(CreateCaseFacilityTypeRequest request)
    {
        var entity = _mapper.Map<CaseFacilityType>(request);
        entity.DateInserted = DateTime.Now;

        // Calculate LOS from dates if not explicitly provided
        if (!entity.Los.HasValue && entity.DateDischarged.HasValue)
        {
            var days = (entity.DateDischarged.Value - entity.DateAdmitted).TotalDays;
            entity.Los = (decimal)days;
        }

        await _unitOfWork.CaseFacilityTypes.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseFacilityTypeDto>(entity);
    }

    public async Task<CaseFacilityTypeDto> UpdateAsync(int id, UpdateCaseFacilityTypeRequest request)
    {
        var entity = await _unitOfWork.CaseFacilityTypes.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
            throw new KeyNotFoundException($"CaseFacilityType with ID {id} not found");

        _mapper.Map(request, entity);
        entity.DateUpdated = DateTime.Now;

        // Recalculate LOS from dates if not explicitly provided
        if (!entity.Los.HasValue && entity.DateDischarged.HasValue)
        {
            var days = (entity.DateDischarged.Value - entity.DateAdmitted).TotalDays;
            entity.Los = (decimal)days;
        }

        await _unitOfWork.CaseFacilityTypes.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseFacilityTypeDto>(entity);
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
