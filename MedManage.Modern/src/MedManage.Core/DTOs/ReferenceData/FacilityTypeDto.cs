namespace MedManage.Core.DTOs.ReferenceData;

public class FacilityTypeDto
{
    public int FacilityTypeId { get; set; }
    public string? FacilityType { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateFacilityTypeDto
{
    public string? FacilityType { get; set; }
}

public class UpdateFacilityTypeDto
{
    public int FacilityTypeId { get; set; }
    public string? FacilityType { get; set; }
}
