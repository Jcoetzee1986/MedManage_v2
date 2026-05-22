namespace MedManage.Core.DTOs.Booking;

public class BookingDto
{
    public int BookingId { get; set; }
    public DateOnly? TravelDate { get; set; }
    public TimeOnly? TravelTime { get; set; }
    public DateOnly? AppointmentDate { get; set; }
    public int? ReferringPracticeId { get; set; }
    public int? MemberId { get; set; }
    public int? CaseId { get; set; }
    public string? Discipline { get; set; }
    public bool? Consultation { get; set; }
    public bool? Admission { get; set; }
    public int? CurrentPracticeId { get; set; }
    public string? Hospital { get; set; }
    public bool? Arrived { get; set; }
    public string? Tisch { get; set; }
    public string? Comments { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateBookingDto
{
    public DateOnly? TravelDate { get; set; }
    public TimeOnly? TravelTime { get; set; }
    public DateOnly? AppointmentDate { get; set; }
    public int? ReferringPracticeId { get; set; }
    public int? MemberId { get; set; }
    public int? CaseId { get; set; }
    public string? Discipline { get; set; }
    public bool? Consultation { get; set; }
    public bool? Admission { get; set; }
    public int? CurrentPracticeId { get; set; }
    public string? Hospital { get; set; }
    public bool? Arrived { get; set; }
    public string? Tisch { get; set; }
    public string? Comments { get; set; }
}

public class UpdateBookingDto
{
    public int BookingId { get; set; }
    public DateOnly? TravelDate { get; set; }
    public TimeOnly? TravelTime { get; set; }
    public DateOnly? AppointmentDate { get; set; }
    public int? ReferringPracticeId { get; set; }
    public int? MemberId { get; set; }
    public int? CaseId { get; set; }
    public string? Discipline { get; set; }
    public bool? Consultation { get; set; }
    public bool? Admission { get; set; }
    public int? CurrentPracticeId { get; set; }
    public string? Hospital { get; set; }
    public bool? Arrived { get; set; }
    public string? Tisch { get; set; }
    public string? Comments { get; set; }
}

public class BookingSearchFilters
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? ServiceProviderId { get; set; }
    public string? MemberNumber { get; set; }
    public bool IncludeDeleted { get; set; } = false;
}
