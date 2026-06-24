namespace MedManage.Core.DTOs.MedicalAid;

public class MedicalAidDto
{
    public int MedicalAidId { get; set; }
    public int? MainClientId { get; set; }
    public string? MedicalAidName { get; set; }
    public DateOnly? MedicalAidInitiationDate { get; set; }
    public DateOnly? MedicalAidReinstatedDate { get; set; }
    public DateOnly? MedicalAidTerminatedDate { get; set; }
    public string? CasePrefix { get; set; }
    public string? ReportTemplate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateMedicalAidDto
{
    public int? MainClientId { get; set; }
    public string? MedicalAidName { get; set; }
    public DateOnly? MedicalAidInitiationDate { get; set; }
    public DateOnly? MedicalAidReinstatedDate { get; set; }
    public DateOnly? MedicalAidTerminatedDate { get; set; }
    public string? CasePrefix { get; set; }
    public string? ReportTemplate { get; set; }
}

public class UpdateMedicalAidDto
{
    public int MedicalAidId { get; set; }
    public int? MainClientId { get; set; }
    public string? MedicalAidName { get; set; }
    public DateOnly? MedicalAidInitiationDate { get; set; }
    public DateOnly? MedicalAidReinstatedDate { get; set; }
    public DateOnly? MedicalAidTerminatedDate { get; set; }
    public string? CasePrefix { get; set; }
    public string? ReportTemplate { get; set; }
}

public class MedicalAidSearchFilters
{
    public string? MedicalAidName { get; set; }
    public bool? ActiveOnly { get; set; } = true;
    public bool IncludeDeleted { get; set; } = false;
}

// ─── Medical Aid Product DTOs ──────────────────────────────────

public class MedicalAidProductDto
{
    public int MedAidProductId { get; set; }
    public int? MainClientId { get; set; }
    public string? MedAidProductName { get; set; }
    public bool? AllowServices { get; set; }
}

public class CreateMedicalAidProductDto
{
    public string? MedAidProductName { get; set; }
    public bool? AllowServices { get; set; } = true;
}

public class UpdateMedicalAidProductDto
{
    public int MedAidProductId { get; set; }
    public string? MedAidProductName { get; set; }
    public bool? AllowServices { get; set; }
}

// ─── Medical Aid Exclusion DTOs ────────────────────────────────

public class MedicalAidExclusionDto
{
    public int MedicalAidId { get; set; }
    public string BaseTariffId { get; set; } = null!;
    public string? BaseTariffDescription { get; set; }
}

public class CreateMedicalAidExclusionDto
{
    public string BaseTariffId { get; set; } = null!;
}

// ─── Medical Aid Tariff DTOs ───────────────────────────────────

public class MedicalAidTariffDto
{
    public int MedicalAidId { get; set; }
    public int TariffNameId { get; set; }
    public string? TariffName { get; set; }
}

public class CreateMedicalAidTariffDto
{
    public int TariffNameId { get; set; }
}
