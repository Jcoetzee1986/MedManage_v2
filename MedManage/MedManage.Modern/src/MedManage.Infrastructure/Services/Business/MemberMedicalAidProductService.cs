using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Core.DTOs.MemberMedicalAidProduct;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MemberMedicalAidProductService : IMemberMedicalAidProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public MemberMedicalAidProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MemberMedicalAidProductDto>> GetByMemberIdAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.MemberMedicalAidProducts.GetByMemberIdAsync(memberId);
        return items.Select(e => e.ToDto());
    }

    public async Task<MemberMedicalAidProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberMedicalAidProducts.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            return null;
        }
        return entity.ToDto();
    }

    public async Task<MemberMedicalAidProductDto> CreateAsync(int memberId, CreateMemberMedicalAidProductDto dto, CancellationToken cancellationToken = default)
    {
        var entity = dto.ToEntity();
        entity.MemberId = memberId;

        await _unitOfWork.MemberMedicalAidProducts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }

    public async Task<MemberMedicalAidProductDto> UpdateAsync(int id, UpdateMemberMedicalAidProductDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberMedicalAidProducts.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            throw new KeyNotFoundException($"MemberMedicalAidProduct with ID {id} not found");
        }

        dto.ApplyTo(entity);

        await _unitOfWork.MemberMedicalAidProducts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberMedicalAidProducts.GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }

        // Soft delete
        entity.DateDeleted = DateTime.Now;
        await _unitOfWork.MemberMedicalAidProducts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
