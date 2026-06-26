namespace MedManage.Core.DTOs.CaseCpt;

/// <summary>
/// DTO for Case CPT code entity
/// </summary>
public class CaseCptDto
{
    public int CaseIdCptid { get; set; }
    public int CaseId { get; set; }
    public int Cptid { get; set; }
    public DateOnly? DateOfProcedure { get; set; }
    public bool? PrimaryCode { get; set; }
    public bool? SecondaryCode { get; set; }

    // Related CPT code info
    public string? CptCode { get; set; }
    public string? CptShortDescription { get; set; }
    public string? CptMediumDescription { get; set; }
    public string? CptLongDescription { get; set; }

    // Audit fields
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

/// <summary>
/// DTO for creating a CPT code assignment to a case
/// </summary>
public class CreateCaseCptDto
{
    public int Cptid { get; set; }
    public DateOnly? DateOfProcedure { get; set; }
    public bool? PrimaryCode { get; set; }
    public bool? SecondaryCode { get; set; }
}

/// <summary>
/// DTO for updating a CPT code assignment on a case
/// </summary>
public class UpdateCaseCptDto
{
    public int Cptid { get; set; }
    public DateOnly? DateOfProcedure { get; set; }
    public bool? PrimaryCode { get; set; }
    public bool? SecondaryCode { get; set; }
}
