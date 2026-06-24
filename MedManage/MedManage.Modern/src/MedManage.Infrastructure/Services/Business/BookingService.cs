using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Booking;
using MedManage.Core.DTOs.Case;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICaseWorkflowService _caseWorkflowService;

    public BookingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        ICaseWorkflowService caseWorkflowService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _caseWorkflowService = caseWorkflowService;
    }

    public async Task<IEnumerable<BookingDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Bookings.GetAllAsync();
        
        if (!includeDeleted)
        {
            entities = entities.Where(x => x.DateDeleted == null);
        }
        
        return _mapper.Map<IEnumerable<BookingDto>>(entities);
    }

    public async Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Bookings.GetByBookingIdAsync(id);
        return entity == null ? null : _mapper.Map<BookingDto>(entity);
    }

    public async Task<IEnumerable<BookingDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Bookings.GetByCaseIdAsync(caseId);
        return _mapper.Map<IEnumerable<BookingDto>>(entities);
    }

    public async Task<IEnumerable<BookingDto>> GetByMemberNumberAsync(string memberNumber, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Bookings.GetByMemberNumberAsync(memberNumber);
        return _mapper.Map<IEnumerable<BookingDto>>(entities);
    }

    public async Task<PagedResult<BookingDto>> SearchAsync(BookingSearchFilters filters, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Bookings.SearchByFiltersAsync(
            filters.DateFrom,
            filters.DateTo,
            filters.ServiceProviderId,
            filters.MemberNumber);

        var query = entities.AsQueryable();

        if (!filters.IncludeDeleted)
        {
            query = query.Where(x => x.DateDeleted == null);
        }

        var totalCount = query.Count();

        var paged = query
            .Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToList();

        return new PagedResult<BookingDto>
        {
            Items = _mapper.Map<IEnumerable<BookingDto>>(paged),
            TotalCount = totalCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize
        };
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Booking>(dto);
        
        await _unitOfWork.Bookings.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<BookingDto>(entity);
    }

    public async Task<BookingDto> UpdateAsync(UpdateBookingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Bookings.GetByBookingIdAsync(dto.BookingId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Booking with ID {dto.BookingId} not found");
        }
        
        _mapper.Map(dto, entity);
        
        await _unitOfWork.Bookings.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<BookingDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Bookings.GetByBookingIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.Bookings.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<CaseDto> ConvertToCaseAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByBookingIdAsync(bookingId);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {bookingId} not found");
        }

        if (booking.CaseId == null || booking.CaseId == 0)
        {
            throw new InvalidOperationException("Booking is not linked to a case. Link the booking to a case before converting.");
        }

        // Transition the linked case from Booking status to Case status
        var transitionRequest = new CaseStatusTransitionRequest
        {
            TargetStatusName = "Case"
        };

        var updatedCase = await _caseWorkflowService.TransitionStatusAsync(booking.CaseId.Value, transitionRequest, cancellationToken);
        return updatedCase;
    }
}
