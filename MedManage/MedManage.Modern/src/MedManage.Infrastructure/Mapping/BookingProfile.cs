using AutoMapper;
using MedManage.Core.DTOs.Booking;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        // Booking -> BookingDto
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateInserted))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateUpdated));

        // CreateBookingDto -> Booking
        CreateMap<CreateBookingDto, Booking>()
            .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());

        // UpdateBookingDto -> Booking
        CreateMap<UpdateBookingDto, Booking>()
            .ForMember(dest => dest.DateInserted, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedUserID, opt => opt.Ignore())
            .ForMember(dest => dest.DateDeleted, opt => opt.Ignore());
    }
}
