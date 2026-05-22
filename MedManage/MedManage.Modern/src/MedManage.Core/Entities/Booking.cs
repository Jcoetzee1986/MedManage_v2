using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Entities;

namespace MedManage.Core.Entities;

[Table("Bookings", Schema = "shared")]
public partial class Booking : BaseEntity
{
    [Key]
    [Column("BookingID")]
    public int BookingId { get; set; }

    public DateOnly? TravelDate { get; set; }

    public TimeOnly? TravelTime { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    [Column("ReferringPracticeID")]
    public int? ReferringPracticeId { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    [Column("CaseID")]
    public int? CaseId { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Discipline { get; set; }

    public bool? Consultation { get; set; }

    public bool? Admission { get; set; }

    [Column("CurrentPracticeID")]
    public int? CurrentPracticeId { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Hospital { get; set; }

    public bool? Arrived { get; set; }

    [Column("TISCH")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Tisch { get; set; }

    [Unicode(false)]
    public string? Comments { get; set; }
}
