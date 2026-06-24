using AutoMapper;
using MedManage.Core.DTOs.MemberChronicIllness;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class MemberChronicIllnessService : IMemberChronicIllnessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemberChronicIllnessService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberChronicIllnessDto>> GetByMemberIdAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.MemberChronicIllnesses.GetByMemberIdAsync(memberId);
        return _mapper.Map<IEnumerable<MemberChronicIllnessDto>>(items);
    }

    public async Task<MemberChronicIllnessDto> CreateAsync(int memberId, CreateMemberChronicIllnessDto dto, CancellationToken cancellationToken = default)
    {
        // Check if the association already exists
        var existing = await _unitOfWork.MemberChronicIllnesses.FindAsync(
            x => x.MemberId == memberId && x.ChronicIllnessId == dto.ChronicIllnessId);

        if (existing.Any())
        {
            var existingItem = existing.First();
            // If it was soft-deleted, restore it
            if (existingItem.DateDeleted != null)
            {
                existingItem.DateDeleted = null;
                await _unitOfWork.MemberChronicIllnesses.UpdateAsync(existingItem);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _mapper.Map<MemberChronicIllnessDto>(existingItem);
            }
            throw new InvalidOperationException("This chronic illness is already assigned to the member.");
        }

        var entity = new MemberChronicIllness
        {
            MemberId = memberId,
            ChronicIllnessId = dto.ChronicIllnessId
        };

        await _unitOfWork.MemberChronicIllnesses.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with include to get ChronicIllness name
        var items = await _unitOfWork.MemberChronicIllnesses.GetByMemberIdAsync(memberId);
        var created = items.FirstOrDefault(x => x.ChronicIllnessId == dto.ChronicIllnessId);
        return _mapper.Map<MemberChronicIllnessDto>(created ?? entity);
    }

    public async Task<bool> DeleteAsync(int memberId, int chronicIllnessId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.MemberChronicIllnesses.FindAsync(
            x => x.MemberId == memberId && x.ChronicIllnessId == chronicIllnessId);

        var entity = items.FirstOrDefault();
        if (entity == null)
        {
            return false;
        }

        // Soft delete
        entity.DateDeleted = DateTime.Now;
        await _unitOfWork.MemberChronicIllnesses.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
