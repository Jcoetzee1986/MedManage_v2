namespace MedManage.Core.DTOs.CaseIcd;

/// <summary>
/// DTO for Case ICD code entity
/// </summary>
public class CaseIcdDto
{
    public int CaseId { get; set; }
    public int Icdid { get; set; }
    public DateOnly? DateOfProcedure { get; set; }
    public bool? PrimaryCode { get; set; }
    public bool? SecondaryCode { get; set; }
    public bool? CoMorbidityCode { get; set; }

    // Related ICD code info
    public string? DiagnosisCode { get; set; }
    public string? DiagnosisDesc { get; set; }
    public string? GroupCode { get; set; }
    public string? GroupDesc { get; set; }

    // Audit fields
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}

/// <summary>
/// DTO for creating an ICD code assignment to a case
/// </summary>
public class CreateCaseIcdDto
{
    public int Icdid { get; set; }
    public DateOnly? DateOfProcedure { get; set; }
    public bool? PrimaryCode { get; set; }
    public bool? SecondaryCode { get; set; }
    public bool? CoMorbidityCode { get; set; }
}

/// <summary>
/// DTO for updating an ICD code assignment on a case
/// </summary>
public class UpdateCaseIcdDto
{
    public int Icdid { get; set; }
    public DateOnly? DateOfProcedure { get; set; }
    public bool? PrimaryCode { get; set; }
    public bool? SecondaryCode { get; set; }
    public bool? CoMorbidityCode { get; set; }
}
