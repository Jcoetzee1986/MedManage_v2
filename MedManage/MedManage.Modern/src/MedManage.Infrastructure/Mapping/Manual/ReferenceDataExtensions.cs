using MedManage.Core.DTOs.ReferenceData;
using MedManage.Core.DTOs.Exclusion;
using MedManage.Core.Entities;

namespace MedManage.Infrastructure.Mapping.Manual;

/// <summary>
/// Extension methods for reference data entities that allow .ToDto()/.ToEntity()/.ApplyTo() usage.
/// These supplement the IEntityMapper implementations used by the generic ReferenceDataService.
/// </summary>
public static class ReferenceDataExtensions
{
    // ─── MarritalStatus ──────────────────────────────────────
    public static MarritalStatusDto ToDto(this MarritalStatus e) => new()
    {
        MarritalStatusId = e.MarritalStatusId,
        MarritalStatus = e.MarritalStatus1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static MarritalStatus ToEntity(this CreateMarritalStatusDto d) => new() { MarritalStatus1 = d.MarritalStatus };
    public static void ApplyTo(this UpdateMarritalStatusDto d, MarritalStatus e) { e.MarritalStatus1 = d.MarritalStatus; }

    // ─── MemberStatus ────────────────────────────────────────
    public static MemberStatusDto ToDto(this MemberStatus e) => new()
    {
        MemberStatusId = e.MemberStatusId,
        MemberStatus = e.MemberStatus1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static MemberStatus ToEntity(this CreateMemberStatusDto d) => new() { MemberStatus1 = d.MemberStatus };
    public static void ApplyTo(this UpdateMemberStatusDto d, MemberStatus e) { e.MemberStatus1 = d.MemberStatus; }

    // ─── BillingStatus ───────────────────────────────────────
    public static BillingStatusDto ToDto(this BillingStatus e) => new()
    {
        BillingStatusId = e.BillingStatusId,
        BillingStatus = e.BillingStatus1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static BillingStatus ToEntity(this CreateBillingStatusDto d) => new() { BillingStatus1 = d.BillingStatus };
    public static void ApplyTo(this UpdateBillingStatusDto d, BillingStatus e) { e.BillingStatus1 = d.BillingStatus; }

    // ─── CaseStatus ──────────────────────────────────────────
    public static CaseStatusDto ToDto(this CaseStatus e) => new()
    {
        CaseStatusId = e.CaseStatusId,
        CaseStatus = e.CaseStatus1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static CaseStatus ToEntity(this CreateCaseStatusDto d) => new() { CaseStatus1 = d.CaseStatus };
    public static void ApplyTo(this UpdateCaseStatusDto d, CaseStatus e) { e.CaseStatus1 = d.CaseStatus; }

    // ─── CaseType ────────────────────────────────────────────
    public static CaseTypeDto ToDto(this CaseType e) => new()
    {
        CaseTypeId = e.CaseTypeId,
        CaseType = e.CaseType1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static CaseType ToEntity(this CreateCaseTypeDto d) => new() { CaseType1 = d.CaseType };
    public static void ApplyTo(this UpdateCaseTypeDto d, CaseType e) { e.CaseType1 = d.CaseType; }

    // ─── FacilityType ────────────────────────────────────────
    public static FacilityTypeDto ToDto(this FacilityType e) => new()
    {
        FacilityTypeId = e.FacilityTypeId,
        FacilityType = e.FacilityType1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static FacilityType ToEntity(this CreateFacilityTypeDto d) => new() { FacilityType1 = d.FacilityType };
    public static void ApplyTo(this UpdateFacilityTypeDto d, FacilityType e) { e.FacilityType1 = d.FacilityType; }

    // ─── Speciality ──────────────────────────────────────────
    public static SpecialityDto ToDto(this Speciality e) => new()
    {
        SpecialityId = e.SpecialityId,
        Speciality = e.Speciality1,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static Speciality ToEntity(this CreateSpecialityDto d) => new() { Speciality1 = d.Speciality };
    public static void ApplyTo(this UpdateSpecialityDto d, Speciality e) { e.Speciality1 = d.Speciality; }

    // ─── ChronicIllness ──────────────────────────────────────
    public static ChronicIllnessDto ToDto(this ChronicIllness e) => new()
    {
        ChronicIllnessId = e.ChronicIllnessId,
        ChronicIllnessName = e.ChronicIllnessName,
        ChronicIllnessDescription = e.ChronicIllnessDescription,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static ChronicIllness ToEntity(this CreateChronicIllnessDto d) => new() { ChronicIllnessName = d.ChronicIllnessName, ChronicIllnessDescription = d.ChronicIllnessDescription };
    public static void ApplyTo(this UpdateChronicIllnessDto d, ChronicIllness e) { e.ChronicIllnessName = d.ChronicIllnessName; e.ChronicIllnessDescription = d.ChronicIllnessDescription; }

    // ─── ChecklistTemplate ───────────────────────────────────
    public static ChecklistTemplateDto ToDto(this ChecklistTemplate e) => new()
    {
        ChecklistTemplateId = e.ChecklistTemplateId,
        ChecklistPrompt = e.ChecklistPrompt,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static ChecklistTemplate ToEntity(this CreateChecklistTemplateDto d) => new() { ChecklistPrompt = d.ChecklistPrompt };
    public static void ApplyTo(this UpdateChecklistTemplateDto d, ChecklistTemplate e) { e.ChecklistPrompt = d.ChecklistPrompt; }

    // ─── Country ─────────────────────────────────────────────
    public static CountryDto ToDto(this Country e) => new()
    {
        CountryId = e.CountryId,
        CountryName = e.CountryName,
        DateCreated = e.DateInserted ?? default,
        DateModified = e.DateUpdated,
    };
    public static Country ToEntity(this CreateCountryDto d) => new() { CountryName = d.CountryName };
    public static void ApplyTo(this UpdateCountryDto d, Country e) { e.CountryName = d.CountryName; }
}


// ─── IEntityMapper implementations (used by generic ReferenceDataService) ───
// These delegate to the extension methods above.

public class MarritalStatusMapper : IEntityMapper<MarritalStatus, MarritalStatusDto, CreateMarritalStatusDto, UpdateMarritalStatusDto>
{
    public MarritalStatusDto ToDto(MarritalStatus e) => e.ToDto();
    public MarritalStatus ToEntity(CreateMarritalStatusDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateMarritalStatusDto d, MarritalStatus e) => d.ApplyTo(e);
}

public class MemberStatusMapper : IEntityMapper<MemberStatus, MemberStatusDto, CreateMemberStatusDto, UpdateMemberStatusDto>
{
    public MemberStatusDto ToDto(MemberStatus e) => e.ToDto();
    public MemberStatus ToEntity(CreateMemberStatusDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateMemberStatusDto d, MemberStatus e) => d.ApplyTo(e);
}

public class BillingStatusMapper : IEntityMapper<BillingStatus, BillingStatusDto, CreateBillingStatusDto, UpdateBillingStatusDto>
{
    public BillingStatusDto ToDto(BillingStatus e) => e.ToDto();
    public BillingStatus ToEntity(CreateBillingStatusDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateBillingStatusDto d, BillingStatus e) => d.ApplyTo(e);
}

public class CaseStatusMapper : IEntityMapper<CaseStatus, CaseStatusDto, CreateCaseStatusDto, UpdateCaseStatusDto>
{
    public CaseStatusDto ToDto(CaseStatus e) => e.ToDto();
    public CaseStatus ToEntity(CreateCaseStatusDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateCaseStatusDto d, CaseStatus e) => d.ApplyTo(e);
}

public class CaseTypeMapper : IEntityMapper<CaseType, CaseTypeDto, CreateCaseTypeDto, UpdateCaseTypeDto>
{
    public CaseTypeDto ToDto(CaseType e) => e.ToDto();
    public CaseType ToEntity(CreateCaseTypeDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateCaseTypeDto d, CaseType e) => d.ApplyTo(e);
}

public class FacilityTypeMapper : IEntityMapper<FacilityType, FacilityTypeDto, CreateFacilityTypeDto, UpdateFacilityTypeDto>
{
    public FacilityTypeDto ToDto(FacilityType e) => e.ToDto();
    public FacilityType ToEntity(CreateFacilityTypeDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateFacilityTypeDto d, FacilityType e) => d.ApplyTo(e);
}

public class SpecialityMapper : IEntityMapper<Speciality, SpecialityDto, CreateSpecialityDto, UpdateSpecialityDto>
{
    public SpecialityDto ToDto(Speciality e) => e.ToDto();
    public Speciality ToEntity(CreateSpecialityDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateSpecialityDto d, Speciality e) => d.ApplyTo(e);
}

public class ChronicIllnessMapper : IEntityMapper<ChronicIllness, ChronicIllnessDto, CreateChronicIllnessDto, UpdateChronicIllnessDto>
{
    public ChronicIllnessDto ToDto(ChronicIllness e) => e.ToDto();
    public ChronicIllness ToEntity(CreateChronicIllnessDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateChronicIllnessDto d, ChronicIllness e) => d.ApplyTo(e);
}

public class ChecklistTemplateMapper : IEntityMapper<ChecklistTemplate, ChecklistTemplateDto, CreateChecklistTemplateDto, UpdateChecklistTemplateDto>
{
    public ChecklistTemplateDto ToDto(ChecklistTemplate e) => e.ToDto();
    public ChecklistTemplate ToEntity(CreateChecklistTemplateDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateChecklistTemplateDto d, ChecklistTemplate e) => d.ApplyTo(e);
}

public class CountryMapper : IEntityMapper<Country, CountryDto, CreateCountryDto, UpdateCountryDto>
{
    public CountryDto ToDto(Country e) => e.ToDto();
    public Country ToEntity(CreateCountryDto d) => d.ToEntity();
    public void ApplyUpdate(UpdateCountryDto d, Country e) => d.ApplyTo(e);
}

public class GenderMapper : IEntityMapper<Gender, GenderDto, CreateGenderDto, UpdateGenderDto>
{
    public GenderDto ToDto(Gender e) => new() { GenderId = e.GenderId, GenderDescription = e.GenderDescription, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public Gender ToEntity(CreateGenderDto d) => new() { GenderDescription = d.GenderDescription };
    public void ApplyUpdate(UpdateGenderDto d, Gender e) { e.GenderDescription = d.GenderDescription; }
}

public class LanguageMapper : IEntityMapper<Language, LanguageDto, CreateLanguageDto, UpdateLanguageDto>
{
    public LanguageDto ToDto(Language e) => new() { LanguageId = e.LanguageId, Language = e.Language1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public Language ToEntity(CreateLanguageDto d) => new() { Language1 = d.Language };
    public void ApplyUpdate(UpdateLanguageDto d, Language e) { e.Language1 = d.Language; }
}

public class RaceMapper : IEntityMapper<Race, RaceDto, CreateRaceDto, UpdateRaceDto>
{
    public RaceDto ToDto(Race e) => new() { RaceId = e.RaceId, Race = e.Race1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public Race ToEntity(CreateRaceDto d) => new() { Race1 = d.Race };
    public void ApplyUpdate(UpdateRaceDto d, Race e) { e.Race1 = d.Race; }
}

public class TitleMapper : IEntityMapper<Title, TitleDto, CreateTitleDto, UpdateTitleDto>
{
    public TitleDto ToDto(Title e) => new() { TitleId = e.TitleId, Title = e.Title1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public Title ToEntity(CreateTitleDto d) => new() { Title1 = d.Title };
    public void ApplyUpdate(UpdateTitleDto d, Title e) { e.Title1 = d.Title; }
}

public class PeriodInCountryMapper : IEntityMapper<PeriodInCountry, PeriodInCountryDto, CreatePeriodInCountryDto, UpdatePeriodInCountryDto>
{
    public PeriodInCountryDto ToDto(PeriodInCountry e) => new() { PeriodInCountryId = e.PeriodInCountryId, PeriodInCountry = e.PeriodInCountry1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public PeriodInCountry ToEntity(CreatePeriodInCountryDto d) => new() { PeriodInCountry1 = d.PeriodInCountry };
    public void ApplyUpdate(UpdatePeriodInCountryDto d, PeriodInCountry e) { e.PeriodInCountry1 = d.PeriodInCountry; }
}

public class CaseCategoryMapper : IEntityMapper<CaseCategory, CaseCategoryDto, CreateCaseCategoryDto, UpdateCaseCategoryDto>
{
    public CaseCategoryDto ToDto(CaseCategory e) => new() { CaseCategoryId = e.CaseCategoryId, CaseCategory = e.CaseCategory1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public CaseCategory ToEntity(CreateCaseCategoryDto d) => new() { CaseCategory1 = d.CaseCategory };
    public void ApplyUpdate(UpdateCaseCategoryDto d, CaseCategory e) { e.CaseCategory1 = d.CaseCategory; }
}

public class SuspendedReasonMapper : IEntityMapper<SuspendedReason, SuspendedReasonDto, CreateSuspendedReasonDto, UpdateSuspendedReasonDto>
{
    public SuspendedReasonDto ToDto(SuspendedReason e) => new() { SuspendedReasonId = e.SuspendedReasonId, SuspendedReason = e.SuspendedReason1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public SuspendedReason ToEntity(CreateSuspendedReasonDto d) => new() { SuspendedReason1 = d.SuspendedReason };
    public void ApplyUpdate(UpdateSuspendedReasonDto d, SuspendedReason e) { e.SuspendedReason1 = d.SuspendedReason; }
}

public class ExclusionMapper : IEntityMapper<Exclusion, ExclusionDto, CreateExclusionDto, UpdateExclusionDto>
{
    public ExclusionDto ToDto(Exclusion e) => new() { ExclusionId = e.ExclusionId, Exclusion = e.Exclusion1, DateCreated = e.DateInserted ?? default, DateModified = e.DateUpdated };
    public Exclusion ToEntity(CreateExclusionDto d) => new() { Exclusion1 = d.Exclusion };
    public void ApplyUpdate(UpdateExclusionDto d, Exclusion e) { e.Exclusion1 = d.Exclusion; }
}
