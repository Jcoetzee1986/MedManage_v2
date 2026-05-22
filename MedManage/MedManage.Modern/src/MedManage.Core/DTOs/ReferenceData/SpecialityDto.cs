namespace MedManage.Core.DTOs.ReferenceData;

public class SpecialityDto
{
    public int SpecialityId { get; set; }
    public string? Speciality { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateSpecialityDto
{
    public string? Speciality { get; set; }
}

public class UpdateSpecialityDto
{
    public int SpecialityId { get; set; }
    public string? Speciality { get; set; }
}
