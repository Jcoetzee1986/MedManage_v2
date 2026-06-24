using AutoMapper;
using MedManage.Core.DTOs.MemberMedicalAidProduct;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MemberMedicalAidProductService : IMemberMedicalAidProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemberMedicalAidProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberMedicalAidProductDto>> GetByMemberIdAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.MemberMedicalAidProducts.GetByMemberIdAsync(memberId);
        return _mapper.Map<IEnumerable<MemberMedicalAidProductDto>>(items);
    }

    public async Task<MemberMedicalAidProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberMedicalAidProducts.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            return null;
        }
        return _mapper.Map<MemberMedicalAidProductDto>(entity);
    }

    public async Task<MemberMedicalAidProductDto> CreateAsync(int memberId, CreateMemberMedicalAidProductDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<MemberMedicalAidProduct>(dto);
        entity.MemberId = memberId;

        await _unitOfWork.MemberMedicalAidProducts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MemberMedicalAidProductDto>(entity);
    }

    public async Task<MemberMedicalAidProductDto> UpdateAsync(int id, UpdateMemberMedicalAidProductDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.MemberMedicalAidProducts.GetByIdAsync(id);
        if (entity == null || entity.DateDeleted != null)
        {
            throw new KeyNotFoundException($"MemberMedicalAidProduct with ID {id} not found");
        }

        _mapper.Map(dto, entity);

        await _unitOfWork.MemberMedicalAidProducts.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MemberMedicalAidProductDto>(entity);
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
