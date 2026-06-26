using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.CaseChecklist;
using MedManage.Core.DTOs.CaseComment;
using MedManage.Core.DTOs.CaseCpt;
using MedManage.Core.DTOs.CaseExclusion;
using MedManage.Core.DTOs.CaseFacilityType;
using MedManage.Core.DTOs.CaseIcd;
using MedManage.Core.DTOs.CaseLetterNote;
using MedManage.Core.DTOs.CaseLink;
using MedManage.Core.DTOs.CaseLinkedFile;
using MedManage.Core.DTOs.CaseNappi;
using MedManage.Core.DTOs.CaseNote;
using MedManage.Core.DTOs.CaseTariff;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping.Manual;

// ─── CaseBilling ─────────────────────────────────────────────
public static class CaseBillingMappers
{
    public static CaseBillingDto ToDto(this CaseBilling entity) => new()
    {
        CaseBillingId = entity.CaseBillingId,
        CaseId = entity.CaseId,
        ServiceProviderId = entity.ServiceProviderId,
        AccountDate = entity.AccountDate,
        AccountToDate = entity.AccountToDate,
        AccountNumber = entity.AccountNumber,
        InvoiceNumber = entity.InvoiceNumber,
        DateReceived = entity.DateReceived,
        Submitted = entity.Submitted,
        DateSubmitted = entity.DateSubmitted,
        Reported = entity.Reported,
        DateReported = entity.DateReported,
        ReceivedByName = entity.ReceivedByName,
        AmountDue = entity.AmountDue,
        Discount = entity.Discount,
        Penalty = entity.Penalty,
        Rejected = entity.Rejected,
        Paid = entity.Paid,
        DatePaid = entity.DatePaid,
        Remittance = entity.Remittance,
        FinalInvoiceAmountDue = entity.FinalInvoiceAmountDue,
        BillingStatusId = entity.BillingStatusId,
        PatientName = entity.PatientName,
        PatientInitials = entity.PatientInitials,
        PatientSurname = entity.PatientSurname,
        Comment = entity.Comment,
    };

    public static CaseBilling ToEntity(this CreateCaseBillingDto dto) => new()
    {
        CaseId = dto.CaseId,
        ServiceProviderId = dto.ServiceProviderId,
        AccountNumber = dto.AccountNumber,
        InvoiceNumber = dto.InvoiceNumber,
        AmountDue = dto.AmountDue,
        Discount = dto.Discount,
        Penalty = dto.Penalty,
        Rejected = dto.Rejected,
        FinalInvoiceAmountDue = dto.FinalInvoiceAmountDue,
        DateReceived = dto.DateReceived,
        DateSubmitted = dto.DateSubmitted,
        DatePaid = dto.DatePaid,
        Submitted = dto.Submitted,
        Paid = dto.Paid,
        Comment = dto.Comment,
        Remittance = dto.Remittance,
        BillingStatusId = dto.BillingStatusId,
    };

    public static void ApplyTo(this UpdateCaseBillingDto dto, CaseBilling entity)
    {
        entity.CaseId = dto.CaseId;
        entity.ServiceProviderId = dto.ServiceProviderId;
        entity.AccountNumber = dto.AccountNumber;
        entity.InvoiceNumber = dto.InvoiceNumber;
        entity.AmountDue = dto.AmountDue;
        entity.Discount = dto.Discount;
        entity.Penalty = dto.Penalty;
        entity.Rejected = dto.Rejected;
        entity.FinalInvoiceAmountDue = dto.FinalInvoiceAmountDue;
        entity.DateReceived = dto.DateReceived;
        entity.DateSubmitted = dto.DateSubmitted;
        entity.DatePaid = dto.DatePaid;
        entity.Submitted = dto.Submitted;
        entity.Paid = dto.Paid;
        entity.Comment = dto.Comment;
        entity.Remittance = dto.Remittance;
        entity.BillingStatusId = dto.BillingStatusId;
    }

    public static List<CaseBillingDto> ToDtoList(this IEnumerable<CaseBilling> entities)
        => entities.Select(e => e.ToDto()).ToList();

    // ─── CaseDiscount ────────────────────────────────────────────
    public static CaseDiscountDto ToDto(this CaseDiscount entity) => new()
    {
        CaseId = entity.CaseId,
        Discount = entity.Discount,
    };

    public static List<CaseDiscountDto> ToDtoList(this IEnumerable<CaseDiscount> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseChecklist ───────────────────────────────────────────
public static class CaseChecklistMappers
{
    public static CaseChecklistDto ToDto(this CaseChecklist entity) => new()
    {
        CaseId = entity.CaseId,
        ChecklistTemplateId = entity.ChecklistTemplateId,
        Checked = entity.Checked,
        Date = entity.Date,
        Comments = entity.Comments,
        NotApplicable = entity.NotApplicable,
    };

    public static CaseChecklist ToEntity(this CreateCaseChecklistDto dto) => new()
    {
        CaseId = dto.CaseId,
        ChecklistTemplateId = dto.ChecklistTemplateId,
        Checked = dto.Checked,
        Date = dto.Date,
        Comments = dto.Comments,
        NotApplicable = dto.NotApplicable,
    };

    public static void ApplyTo(this UpdateCaseChecklistDto dto, CaseChecklist entity)
    {
        entity.Checked = dto.Checked;
        entity.Date = dto.Date;
        entity.Comments = dto.Comments;
        entity.NotApplicable = dto.NotApplicable;
    }

    public static List<CaseChecklistDto> ToDtoList(this IEnumerable<CaseChecklist> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseComment ─────────────────────────────────────────────
public static class CaseCommentMappers
{
    public static CaseCommentDto ToDto(this CaseComment entity) => new()
    {
        CaseCommentId = entity.CaseCommentId,
        CaseId = entity.CaseId,
        Comment = entity.CaseComment1,
        DateCreated = entity.DateCreated,
    };

    public static CaseComment ToEntity(this CreateCaseCommentDto dto) => new()
    {
        CaseId = dto.CaseId,
        CaseComment1 = dto.Comment,
    };

    public static void ApplyTo(this UpdateCaseCommentDto dto, CaseComment entity)
    {
        entity.CaseComment1 = dto.Comment;
    }

    public static List<CaseCommentDto> ToDtoList(this IEnumerable<CaseComment> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseCpt ─────────────────────────────────────────────────
public static class CaseCptMappers
{
    public static CaseCptDto ToDto(this CaseCpt entity) => new()
    {
        CaseIdCptid = entity.CaseIdCptid,
        CaseId = entity.CaseId,
        Cptid = entity.Cptid,
        DateOfProcedure = entity.DateOfProcedure,
        PrimaryCode = entity.PrimaryCode,
        SecondaryCode = entity.SecondaryCode,
        // Flattened CPT navigation properties
        CptCode = entity.Cpt?.Code,
        CptShortDescription = entity.Cpt?.ShortDescr,
        CptMediumDescription = entity.Cpt?.MediumDescr,
        CptLongDescription = entity.Cpt?.LongDescr,
    };

    public static CaseCpt ToEntity(this CreateCaseCptDto dto) => new()
    {
        Cptid = dto.Cptid,
        DateOfProcedure = dto.DateOfProcedure,
        PrimaryCode = dto.PrimaryCode,
        SecondaryCode = dto.SecondaryCode,
    };

    public static void ApplyTo(this UpdateCaseCptDto dto, CaseCpt entity)
    {
        entity.Cptid = dto.Cptid;
        entity.DateOfProcedure = dto.DateOfProcedure;
        entity.PrimaryCode = dto.PrimaryCode;
        entity.SecondaryCode = dto.SecondaryCode;
    }

    public static List<CaseCptDto> ToDtoList(this IEnumerable<CaseCpt> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseExclusion ───────────────────────────────────────────
public static class CaseExclusionMappers
{
    public static CaseExclusionDto ToDto(this CaseExclusion entity) => new()
    {
        CaseId = entity.CaseId,
        ExclusionId = entity.ExclusionId,
        Comment = entity.Comment,
    };

    public static CaseExclusion ToEntity(this CreateCaseExclusionDto dto) => new()
    {
        CaseId = dto.CaseId,
        ExclusionId = dto.ExclusionId,
        Comment = dto.Comment,
    };

    public static void ApplyTo(this UpdateCaseExclusionDto dto, CaseExclusion entity)
    {
        entity.Comment = dto.Comment;
    }

    public static List<CaseExclusionDto> ToDtoList(this IEnumerable<CaseExclusion> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseFacilityType ────────────────────────────────────────
public static class CaseFacilityTypeMappers
{
    public static CaseFacilityTypeDto ToDto(this CaseFacilityType entity) => new()
    {
        CaseIdFacilityTypeId = entity.CaseIdFacilityTypeId,
        CaseId = entity.CaseId,
        FacilityTypeId = entity.FacilityTypeId,
        DateAdmitted = entity.DateAdmitted,
        DateDischarged = entity.DateDischarged,
        Los = entity.Los,
        FacilityTypeCode = entity.FacilityTypeCode,
        MinutesOnVentilator = entity.MinutesOnVentilator,
        Comments = entity.Comments,
    };

    public static CaseFacilityType ToEntity(this CreateCaseFacilityTypeRequest request) => new()
    {
        CaseId = request.CaseId,
        FacilityTypeId = request.FacilityTypeId,
        DateAdmitted = request.DateAdmitted,
        DateDischarged = request.DateDischarged,
        Los = request.Los,
        FacilityTypeCode = request.FacilityTypeCode,
        MinutesOnVentilator = request.MinutesOnVentilator,
        Comments = request.Comments,
    };

    public static void ApplyTo(this UpdateCaseFacilityTypeRequest request, CaseFacilityType entity)
    {
        entity.FacilityTypeId = request.FacilityTypeId;
        entity.DateAdmitted = request.DateAdmitted;
        entity.DateDischarged = request.DateDischarged;
        entity.Los = request.Los;
        entity.FacilityTypeCode = request.FacilityTypeCode;
        entity.MinutesOnVentilator = request.MinutesOnVentilator;
        entity.Comments = request.Comments;
    }

    public static List<CaseFacilityTypeDto> ToDtoList(this IEnumerable<CaseFacilityType> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseIcd ─────────────────────────────────────────────────
public static class CaseIcdMappers
{
    public static CaseIcdDto ToDto(this CaseIcd entity) => new()
    {
        CaseId = entity.CaseId,
        Icdid = entity.Icdid,
        DateOfProcedure = entity.DateOfProcedure,
        PrimaryCode = entity.PrimaryCode,
        SecondaryCode = entity.SecondaryCode,
        CoMorbidityCode = entity.CoMorbidityCode,
        // Flattened ICD navigation properties
        DiagnosisCode = entity.Icd?.DiagnosisCode,
        DiagnosisDesc = entity.Icd?.DiagnosisDesc,
        GroupCode = entity.Icd?.GroupCode,
        GroupDesc = entity.Icd?.GroupDesc,
        DateInserted = entity.DateInserted,
        DateUpdated = entity.DateUpdated,
    };

    public static CaseIcd ToEntity(this CreateCaseIcdDto dto) => new()
    {
        Icdid = dto.Icdid,
        DateOfProcedure = dto.DateOfProcedure,
        PrimaryCode = dto.PrimaryCode,
        SecondaryCode = dto.SecondaryCode,
        CoMorbidityCode = dto.CoMorbidityCode,
    };

    public static void ApplyTo(this UpdateCaseIcdDto dto, CaseIcd entity)
    {
        if (dto.Icdid != default) entity.Icdid = dto.Icdid;
        if (dto.DateOfProcedure != null) entity.DateOfProcedure = dto.DateOfProcedure;
        if (dto.PrimaryCode != null) entity.PrimaryCode = dto.PrimaryCode;
        if (dto.SecondaryCode != null) entity.SecondaryCode = dto.SecondaryCode;
        if (dto.CoMorbidityCode != null) entity.CoMorbidityCode = dto.CoMorbidityCode;
    }

    public static List<CaseIcdDto> ToDtoList(this IEnumerable<CaseIcd> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseLetterNote ──────────────────────────────────────────
public static class CaseLetterNoteMappers
{
    public static CaseLetterNoteDto ToDto(this CaseLetterNote entity) => new()
    {
        CaseId = entity.CaseId,
        Note = entity.Note,
        IncludeDischargeForm = entity.IncludeDischargeForm,
        IncludeReferralLetter = entity.IncludeReferralLetter,
    };

    public static CaseLetterNote ToEntity(this CreateCaseLetterNoteDto dto) => new()
    {
        CaseId = dto.CaseId,
        Note = dto.Note,
        IncludeDischargeForm = dto.IncludeDischargeForm,
        IncludeReferralLetter = dto.IncludeReferralLetter,
    };

    public static void ApplyTo(this UpdateCaseLetterNoteDto dto, CaseLetterNote entity)
    {
        entity.Note = dto.Note;
        entity.IncludeDischargeForm = dto.IncludeDischargeForm;
        entity.IncludeReferralLetter = dto.IncludeReferralLetter;
    }
}

// ─── CaseLink ────────────────────────────────────────────────
public static class CaseLinkMappers
{
    public static CaseLinkDto ToDto(this CaseLink entity) => new()
    {
        ParentCase = entity.ParentCase,
        ChildCase = entity.ChildCase,
        DateInserted = entity.DateInserted,
    };
}

// ─── CaseLinkedFile ──────────────────────────────────────────
public static class CaseLinkedFileMappers
{
    public static CaseLinkedFileDto ToDto(this CaseLinkedFile entity) => new()
    {
        CaseLinkedFileId = entity.CaseLinkedFileId,
        CaseId = entity.CaseId,
        MemberId = entity.MemberId,
        FileType = entity.FileType,
        FilePath = entity.FilePath,
        FileName = entity.FileName,
        DateAdded = entity.DateAdded,
        DateInserted = entity.DateInserted,
        DateModified = entity.DateUpdated,
        DateDeleted = entity.DateDeleted,
    };

    public static CaseLinkedFile ToEntity(this CreateCaseLinkedFileDto dto) => new()
    {
        CaseId = dto.CaseId,
        MemberId = dto.MemberId,
        FileType = dto.FileType,
        FilePath = dto.FilePath,
        FileName = dto.FileName,
        DateAdded = dto.DateAdded,
    };

    public static void ApplyTo(this UpdateCaseLinkedFileDto dto, CaseLinkedFile entity)
    {
        entity.CaseId = dto.CaseId;
        entity.MemberId = dto.MemberId;
        entity.FileType = dto.FileType;
        entity.FilePath = dto.FilePath;
        entity.FileName = dto.FileName;
        entity.DateAdded = dto.DateAdded;
    }

    public static List<CaseLinkedFileDto> ToDtoList(this IEnumerable<CaseLinkedFile> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseNappi ───────────────────────────────────────────────
public static class CaseNappiMappers
{
    public static CaseNappiDto ToDto(this CaseNappiCode entity) => new()
    {
        CaseIdNappiId = entity.CaseIdNappiId,
        CaseId = entity.CaseId,
        NappiId = entity.NappiId,
        Value = entity.Value,
        Quantity = entity.Quantity,
        Dispensary = entity.Dispensary,
        Ward = entity.Ward,
        Theater = entity.Theater,
        Tto = entity.Tto,
        Date = entity.Date,
        DateInserted = entity.DateInserted,
        DateModified = entity.DateUpdated,
    };

    public static CaseNappiCode ToEntity(this CreateCaseNappiDto dto) => new()
    {
        NappiId = dto.NappiId,
        Value = dto.Value,
        Quantity = dto.Quantity,
        Dispensary = dto.Dispensary,
        Ward = dto.Ward,
        Theater = dto.Theater,
        Tto = dto.Tto,
        Date = dto.Date,
    };

    public static void ApplyTo(this UpdateCaseNappiDto dto, CaseNappiCode entity)
    {
        entity.NappiId = dto.NappiId;
        entity.Value = dto.Value;
        entity.Quantity = dto.Quantity;
        entity.Dispensary = dto.Dispensary;
        entity.Ward = dto.Ward;
        entity.Theater = dto.Theater;
        entity.Tto = dto.Tto;
        entity.Date = dto.Date;
    }

    public static List<CaseNappiDto> ToDtoList(this IEnumerable<CaseNappiCode> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseNote ────────────────────────────────────────────────
public static class CaseNoteMappers
{
    public static CaseNoteDto ToDto(this CaseNote entity) => new()
    {
        CaseNoteId = entity.CaseNoteId,
        CaseId = entity.CaseId,
        Note = entity.CaseNote1,
        DateCreated = entity.DateCreated,
        InterimAmount = entity.InterimAmount,
        CaseNumber = entity.CaseNumber,
        InterimHospital = entity.InterimHospital,
        InterimRadiology = entity.InterimRadiology,
        InterimDialysis = entity.InterimDialysis,
        InterimSpecialist = entity.InterimSpecialist,
        InterimPhysio = entity.InterimPhysio,
        InterimTransport = entity.InterimTransport,
        InterimAccomodation = entity.InterimAccomodation,
        InterimScript = entity.InterimScript,
        DateModified = entity.DateUpdated,
    };

    public static CaseNote ToEntity(this CreateCaseNoteDto dto) => new()
    {
        CaseId = dto.CaseId,
        CaseNote1 = dto.Note,
        DateCreated = dto.DateCreated,
        InterimAmount = dto.InterimAmount,
        CaseNumber = dto.CaseNumber,
        InterimHospital = dto.InterimHospital,
        InterimRadiology = dto.InterimRadiology,
        InterimDialysis = dto.InterimDialysis,
        InterimSpecialist = dto.InterimSpecialist,
        InterimPhysio = dto.InterimPhysio,
        InterimTransport = dto.InterimTransport,
        InterimAccomodation = dto.InterimAccomodation,
        InterimScript = dto.InterimScript,
    };

    public static void ApplyTo(this UpdateCaseNoteDto dto, CaseNote entity)
    {
        entity.CaseNote1 = dto.Note;
        entity.InterimAmount = dto.InterimAmount;
        entity.CaseNumber = dto.CaseNumber;
        entity.InterimHospital = dto.InterimHospital;
        entity.InterimRadiology = dto.InterimRadiology;
        entity.InterimDialysis = dto.InterimDialysis;
        entity.InterimSpecialist = dto.InterimSpecialist;
        entity.InterimPhysio = dto.InterimPhysio;
        entity.InterimTransport = dto.InterimTransport;
        entity.InterimAccomodation = dto.InterimAccomodation;
        entity.InterimScript = dto.InterimScript;
    }

    public static List<CaseNoteDto> ToDtoList(this IEnumerable<CaseNote> entities)
        => entities.Select(e => e.ToDto()).ToList();
}

// ─── CaseTariff ──────────────────────────────────────────────
public static class CaseTariffMappers
{
    public static CaseTariffDto ToDto(this CaseTariff entity) => new()
    {
        CaseIdTariffId = entity.CaseIdTariffId,
        CaseId = entity.CaseId,
        TariffId = entity.TariffId,
        Value = entity.Value,
        Qty = entity.Qty,
        AgreedRateOverride = entity.AgreedRateOverride,
        ValueIsTotal = entity.ValueIsTotal,
        Rejected = entity.Rejected,
        DateOfProcedure = entity.DateOfProcedure,
    };

    public static CaseTariff ToEntity(this CreateCaseTariffRequest request) => new()
    {
        TariffId = request.TariffId,
        Value = request.Value,
        Qty = request.Qty,
        AgreedRateOverride = request.AgreedRateOverride,
        ValueIsTotal = request.ValueIsTotal,
        Rejected = request.Rejected,
        DateOfProcedure = request.DateOfProcedure,
    };

    public static void ApplyTo(this UpdateCaseTariffRequest request, CaseTariff entity)
    {
        entity.Value = request.Value;
        entity.Qty = request.Qty;
        entity.AgreedRateOverride = request.AgreedRateOverride;
        entity.ValueIsTotal = request.ValueIsTotal;
        entity.Rejected = request.Rejected;
        entity.DateOfProcedure = request.DateOfProcedure;
    }

    public static List<CaseTariffDto> ToDtoList(this IEnumerable<CaseTariff> entities)
        => entities.Select(e => e.ToDto()).ToList();
}
