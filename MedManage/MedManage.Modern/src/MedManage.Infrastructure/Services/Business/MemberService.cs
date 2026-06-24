using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.Member;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Core.Interfaces.ICurrentUserService _currentUserService;

    public MemberService(IUnitOfWork unitOfWork, IMapper mapper, Core.Interfaces.ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<MemberDto?> GetByIdAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(memberId);
        return member == null ? null : _mapper.Map<MemberDto>(member);
    }

    public async Task<PagedResult<MemberDto>> SearchAsync(MemberSearchRequest request, CancellationToken cancellationToken = default)
    {
        // Build query directly on DbSet for database-level filtering
        var query = (await _unitOfWork.Members.GetAllAsync()).AsQueryable();

        // Apply soft delete filter first
        if (!(request.IncludeDeleted ?? false))
        {
            query = query.Where(m => m.DateDeleted == null);
        }

        // Apply search filters
        if (!string.IsNullOrWhiteSpace(request.MemberNumber))
        {
            query = query.Where(m => m.MemberNumber != null && m.MemberNumber.Contains(request.MemberNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.Surname))
        {
            query = query.Where(m => m.Surname != null && m.Surname.Contains(request.Surname));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(m => m.Name != null && m.Name.Contains(request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.Idnumber))
        {
            query = query.Where(m => m.Idnumber == request.Idnumber);
        }

        if (request.MedicalAidId.HasValue)
        {
            query = query.Where(m => m.MedicalAidId == request.MedicalAidId.Value);
        }

        if (request.MemberStatusId.HasValue)
        {
            query = query.Where(m => m.MemberStatusId == request.MemberStatusId.Value);
        }

        if (request.HasMedicalAid.HasValue)
        {
            query = query.Where(m => m.HasMedicalAid == request.HasMedicalAid.Value);
        }

        if (request.Suspended.HasValue)
        {
            query = query.Where(m => m.Suspended == request.Suspended.Value);
        }

        if (request.Deceased.HasValue)
        {
            query = query.Where(m => m.Deceased == request.Deceased.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply ordering and pagination, then execute query
        var members = await query
            .OrderBy(m => m.Surname)
            .ThenBy(m => m.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<MemberDto>
        {
            Items = _mapper.Map<List<MemberDto>>(members),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<MemberDto> CreateAsync(CreateMemberRequest request, CancellationToken cancellationToken = default)
    {
        var member = _mapper.Map<Member>(request);
        
        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MemberDto>(member);
    }

    public async Task<MemberDto> UpdateAsync(UpdateMemberRequest request, CancellationToken cancellationToken = default)
    {
        var existingMember = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (existingMember == null)
        {
            throw new KeyNotFoundException($"Member with ID {request.MemberId} not found");
        }

        _mapper.Map(request, existingMember);
        
        await _unitOfWork.Members.UpdateAsync(existingMember);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MemberDto>(existingMember);
    }

    public async Task<bool> DeleteAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(memberId);
        if (member == null)
        {
            return false;
        }

        await _unitOfWork.Members.DeleteAsync(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Members.ExistsAsync(m => m.MemberId == memberId);
    }

    public async Task<bool> IsMemberNumberUniqueAsync(string memberNumber, int? excludeMemberId = null, CancellationToken cancellationToken = default)
    {
        var exists = await _unitOfWork.Members.MemberNumberExistsAsync(memberNumber, excludeMemberId);
        return !exists;
    }
}
