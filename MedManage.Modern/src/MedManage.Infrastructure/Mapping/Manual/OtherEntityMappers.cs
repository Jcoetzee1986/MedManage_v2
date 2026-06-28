using MedManage.Core.DTOs.Booking;
using MedManage.Core.DTOs.Episode;
using MedManage.Core.DTOs.EpisodeCase;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.DTOs.MedicalAid;
using MedManage.Core.DTOs.MemberChronicIllness;
using MedManage.Core.DTOs.MemberMedicalAidProduct;
using MedManage.Core.DTOs.MemberNote;
using MedManage.Core.DTOs.Tariff;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping.Manual;

// ─── Booking ─────────────────────────────────────────────────
public static class BookingMappers
{
    public static BookingDto ToDto(this Booking entity) => new()
    {
        BookingId = entity.BookingId,
        TravelDate = entity.TravelDate,
        TravelTime = entity.TravelTime,
        AppointmentDate = entity.AppointmentDate,
        ReferringPracticeId = entity.ReferringPracticeId,
        MemberId = entity.MemberId,
        CaseId = entity.CaseId,
        Discipline = entity.Discipline,
        Consultation = entity.Consultation,
        Admission = entity.Admission,
        CurrentPracticeId = entity.CurrentPracticeId,
        Hospital = entity.Hospital,
        Arrived = entity.Arrived,
        Tisch = entity.Tisch,
        Comments = entity.Comments,
        DateCreated = entity.DateInserted ?? DateTime.MinValue,
        DateModified = entity.DateUpdated,
        DateDeleted = entity.DateDeleted,
    };

    public static Booking ToEntity(this CreateBookingDto dto) => new()
    {
        TravelDate = dto.TravelDate,
        TravelTime = dto.TravelTime,
        AppointmentDate = dto.AppointmentDate,
        ReferringPracticeId = dto.ReferringPracticeId,
        MemberId = dto.MemberId,
        CaseId = dto.CaseId,
        Discipline = dto.Discipline,
        Consultation = dto.Consultation,
        Admission = dto.Admission,
        CurrentPracticeId = dto.CurrentPracticeId,
        Hospital = dto.Hospital,
        Arrived = dto.Arrived,
        Tisch = dto.Tisch,
        Comments = dto.Comments,
    };

    public static void ApplyTo(this UpdateBookingDto dto, Booking entity)
    {
        entity.TravelDate = dto.TravelDate;
        entity.TravelTime = dto.TravelTime;
        entity.AppointmentDate = dto.AppointmentDate;
        entity.ReferringPracticeId = dto.ReferringPracticeId;
        entity.MemberId = dto.MemberId;
        entity.CaseId = dto.CaseId;
        entity.Discipline = dto.Discipline;
        entity.Consultation = dto.Consultation;
        entity.Admission = dto.Admission;
        entity.CurrentPracticeId = dto.CurrentPracticeId;
        entity.Hospital = dto.Hospital;
        entity.Arrived = dto.Arrived;
        entity.Tisch = dto.Tisch;
        entity.Comments = dto.Comments;
    }

    public static List<BookingDto> ToDtoList(this IEnumerable<Booking> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── Episode ─────────────────────────────────────────────────
public static class EpisodeMappers
{
    public static EpisodeDto ToDto(this Episode entity) => new()
    {
        EpisodeId = entity.EpisodeId,
        EpisodeDescription = entity.EpisodeDescription,
        MemberId = entity.MemberId,
        DateCreated = entity.DateCreated,
        DateInserted = entity.DateInserted ?? DateTime.MinValue,
        DateModified = entity.DateUpdated,
        DateDeleted = entity.DateDeleted,
    };

    public static Episode ToEntity(this CreateEpisodeDto dto) => new()
    {
        EpisodeDescription = dto.EpisodeDescription,
        MemberId = dto.MemberId,
        DateCreated = dto.DateCreated,
    };

    public static void ApplyTo(this UpdateEpisodeDto dto, Episode entity)
    {
        entity.EpisodeDescription = dto.EpisodeDescription;
        entity.MemberId = dto.MemberId;
        entity.DateCreated = dto.DateCreated;
    }

    public static List<EpisodeDto> ToDtoList(this IEnumerable<Episode> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── EpisodeCase ─────────────────────────────────────────────
public static class EpisodeCaseMappers
{
    public static EpisodeCaseDto ToDto(this EpisodeCase entity) => new()
    {
        EpisodeId = entity.EpisodeId,
        CaseId = entity.CaseId,
    };

    public static EpisodeCase ToEntity(this CreateEpisodeCaseDto dto) => new()
    {
        EpisodeId = dto.EpisodeId,
        CaseId = dto.CaseId,
    };

    public static void ApplyTo(this UpdateEpisodeCaseDto dto, EpisodeCase entity)
    {
        entity.DateCreated = dto.DateCreated;
    }

    public static List<EpisodeCaseDto> ToDtoList(this IEnumerable<EpisodeCase> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── Exclusion (standalone entity) ──────────────────────────
public static class ExclusionMappers
{
    public static ExclusionDto ToDto(this Exclusion entity) => new()
    {
        ExclusionId = entity.ExclusionId,
        Exclusion = entity.Exclusion1,
        DateCreated = entity.DateInserted ?? DateTime.MinValue,
        DateModified = entity.DateUpdated,
        DateDeleted = entity.DateDeleted,
    };

    public static Exclusion ToEntity(this CreateExclusionDto dto) => new()
    {
        Exclusion1 = dto.Exclusion,
    };

    public static void ApplyTo(this UpdateExclusionDto dto, Exclusion entity)
    {
        entity.Exclusion1 = dto.Exclusion;
    }

    public static List<ExclusionDto> ToDtoList(this IEnumerable<Exclusion> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── MedicalAid ──────────────────────────────────────────────
public static class MedicalAidMappers
{
    public static MedicalAidDto ToDto(this MedicalAid entity) => new()
    {
        MedicalAidId = entity.MedicalAidId,
        MedicalAidName = entity.MedicalAidName,
        MainClientId = entity.MainClientId,
        MedicalAidInitiationDate = entity.MedicalAidInitiationDate,
        MedicalAidReinstatedDate = entity.MedicalAidReinstatedDate,
        MedicalAidTerminatedDate = entity.MedicalAidTerminatedDate,
        CasePrefix = entity.CasePrefix,
        ReportTemplate = entity.ReportTemplate,
        DateCreated = entity.DateInserted ?? DateTime.MinValue,
        DateModified = entity.DateUpdated,
        DateDeleted = entity.DateDeleted,
    };

    public static MedicalAid ToEntity(this CreateMedicalAidDto dto) => new()
    {
        MedicalAidName = dto.MedicalAidName,
        MainClientId = dto.MainClientId,
        MedicalAidInitiationDate = dto.MedicalAidInitiationDate,
        MedicalAidReinstatedDate = dto.MedicalAidReinstatedDate,
        MedicalAidTerminatedDate = dto.MedicalAidTerminatedDate,
        CasePrefix = dto.CasePrefix,
        ReportTemplate = dto.ReportTemplate,
    };

    public static void ApplyTo(this UpdateMedicalAidDto dto, MedicalAid entity)
    {
        entity.MedicalAidName = dto.MedicalAidName;
        entity.MainClientId = dto.MainClientId;
        entity.MedicalAidInitiationDate = dto.MedicalAidInitiationDate;
        entity.MedicalAidReinstatedDate = dto.MedicalAidReinstatedDate;
        entity.MedicalAidTerminatedDate = dto.MedicalAidTerminatedDate;
        entity.CasePrefix = dto.CasePrefix;
        entity.ReportTemplate = dto.ReportTemplate;
    }

    public static List<MedicalAidDto> ToDtoList(this IEnumerable<MedicalAid> entities)
        => entities.Select(e => e.ToDto()).ToList();

    // MedicalAidProduct
    public static MedicalAidProductDto ToDto(this MedicalAidProduct entity) => new()
    {
        MedAidProductId = entity.MedAidProductId,
        MainClientId = entity.MainClientId,
        MedAidProductName = entity.MedAidProductName,
        AllowServices = entity.AllowServices,
    };

    public static List<MedicalAidProductDto> ToDtoList(this IEnumerable<MedicalAidProduct> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── MemberChronicIllness ────────────────────────────────────
public static class MemberChronicIllnessMappers
{
    public static MemberChronicIllnessDto ToDto(this MemberChronicIllness entity) => new()
    {
        MemberId = entity.MemberId,
        ChronicIllnessId = entity.ChronicIllnessId,
        ChronicIllnessName = entity.ChronicIllness?.ChronicIllnessName,
        ChronicIllnessDescription = entity.ChronicIllness?.ChronicIllnessDescription,
    };

    public static List<MemberChronicIllnessDto> ToDtoList(this IEnumerable<MemberChronicIllness> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── MemberMedicalAidProduct ─────────────────────────────────
public static class MemberMedicalAidProductMappers
{
    public static MemberMedicalAidProductDto ToDto(this MemberMedicalAidProduct entity) => new()
    {
        MedAidProductIdMemberId = entity.MedAidProductIdMemberId,
        MemberId = entity.MemberId,
        MedAidProductId = entity.MedAidProductId,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
    };

    public static MemberMedicalAidProduct ToEntity(this CreateMemberMedicalAidProductDto dto) => new()
    {
        MedAidProductId = dto.MedAidProductId,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
    };

    public static void ApplyTo(this UpdateMemberMedicalAidProductDto dto, MemberMedicalAidProduct entity)
    {
        if (dto.MedAidProductId != null) entity.MedAidProductId = dto.MedAidProductId;
        if (dto.StartDate != null) entity.StartDate = dto.StartDate;
        if (dto.EndDate != null) entity.EndDate = dto.EndDate;
    }

    public static List<MemberMedicalAidProductDto> ToDtoList(this IEnumerable<MemberMedicalAidProduct> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── MemberNote ──────────────────────────────────────────────
public static class MemberNoteMappers
{
    public static MemberNoteDto ToDto(this MemberNote entity) => new()
    {
        MemberNoteId = entity.MemberNoteId,
        MemberId = entity.MemberId,
        Note = entity.MemberNote1,
        DateCreated = entity.DateCreated,
        DateModified = entity.DateUpdated,
    };

    public static MemberNote ToEntity(this CreateMemberNoteDto dto) => new()
    {
        MemberId = dto.MemberId,
        MemberNote1 = dto.Note,
        DateCreated = dto.DateCreated,
    };

    public static void ApplyTo(this UpdateMemberNoteDto dto, MemberNote entity)
    {
        entity.MemberNote1 = dto.Note;
    }

    public static List<MemberNoteDto> ToDtoList(this IEnumerable<MemberNote> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── Tariff ──────────────────────────────────────────────────
public static class TariffMappers
{
    // BaseTariff
    public static BaseTariffDto ToDto(this BaseTariff entity) => new()
    {
        BaseTariffId = entity.BaseTariffId,
        TariffDescription = entity.TariffDescription,
    };

    public static BaseTariff ToEntity(this CreateBaseTariffDto dto) => new()
    {
        BaseTariffId = dto.BaseTariffId,
        TariffDescription = dto.TariffDescription,
    };

    public static void ApplyTo(this UpdateBaseTariffDto dto, BaseTariff entity)
    {
        if (dto.TariffDescription != null) entity.TariffDescription = dto.TariffDescription;
    }

    public static List<BaseTariffDto> ToDtoList(this IEnumerable<BaseTariff> entities)
        => entities.Select(e => e.ToDto()).ToList();

    // TariffRate (Tariff entity)
    public static TariffRateDto ToDto(this Tariff entity) => new()
    {
        TariffId = entity.TariffId,
        BaseTariffId = entity.BaseTariffId,
        TariffNameId = entity.TariffNameId,
        TariffAmount = entity.TariffAmount,
        Metric = entity.Metric,
        Quantity = entity.Quantity,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        TariffPeriodName = entity.TariffPeriodName,
        DateInserted = entity.DateInserted,
        DateUpdated = entity.DateUpdated,
    };

    public static Tariff ToEntity(this CreateTariffRateDto dto) => new()
    {
        BaseTariffId = dto.BaseTariffId,
        TariffNameId = dto.TariffNameId,
        TariffAmount = dto.TariffAmount,
        Metric = dto.Metric,
        Quantity = dto.Quantity,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        TariffPeriodName = dto.TariffPeriodName,
    };

    public static void ApplyTo(this UpdateTariffRateDto dto, Tariff entity)
    {
        if (dto.BaseTariffId != null) entity.BaseTariffId = dto.BaseTariffId;
        if (dto.TariffNameId != null) entity.TariffNameId = dto.TariffNameId;
        if (dto.TariffAmount != null) entity.TariffAmount = dto.TariffAmount;
        if (dto.Metric != null) entity.Metric = dto.Metric;
        if (dto.Quantity != null) entity.Quantity = dto.Quantity;
        if (dto.StartDate != null) entity.StartDate = dto.StartDate;
        if (dto.EndDate != null) entity.EndDate = dto.EndDate;
        if (dto.TariffPeriodName != null) entity.TariffPeriodName = dto.TariffPeriodName;
    }

    public static List<TariffRateDto> ToDtoList(this IEnumerable<Tariff> entities)
        => entities.Select(e => e.ToDto()).ToList();

    // TariffName
    public static TariffNameDto ToDto(this TariffName entity) => new()
    {
        TariffNameId = entity.TariffNameId,
        TariffName = entity.TariffName1,
    };

    public static TariffName ToEntity(this CreateTariffNameDto dto) => new()
    {
        TariffName1 = dto.TariffName,
    };

    public static void ApplyTo(this UpdateTariffNameDto dto, TariffName entity)
    {
        if (dto.TariffName != null) entity.TariffName1 = dto.TariffName;
    }

    public static List<TariffNameDto> ToDtoList(this IEnumerable<TariffName> entities)
        => entities.Select(e => e.ToDto()).ToList();
}
