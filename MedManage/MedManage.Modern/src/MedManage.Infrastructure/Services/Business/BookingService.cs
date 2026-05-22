using AutoMapper;
using MedManage.Core.DTOs.Booking;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public BookingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
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

    public async Task<IEnumerable<BookingDto>> SearchAsync(BookingSearchFilters filters, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Bookings.SearchByFiltersAsync(
            filters.DateFrom,
            filters.DateTo,
            filters.ServiceProviderId,
            filters.MemberNumber);
        
        return _mapper.Map<IEnumerable<BookingDto>>(entities);
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Booking>(dto);
        entity.DateInserted = DateTime.UtcNow;
        entity.UserID = _currentUserService.UserId ?? string.Empty;
        
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
        entity.DateUpdated = DateTime.UtcNow;
        entity.UpdatedUserID = _currentUserService.UserId;
        
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
}
