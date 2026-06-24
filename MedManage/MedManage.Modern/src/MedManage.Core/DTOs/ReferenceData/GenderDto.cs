namespace MedManage.Core.DTOs.ReferenceData;

public class GenderDto
{
    public int GenderId { get; set; }
    public string? GenderCode { get; set; }
    public string? GenderDescription { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateGenderDto
{
    public string? GenderCode { get; set; }
    public string? GenderDescription { get; set; }
}

public class UpdateGenderDto
{
    public string? GenderCode { get; set; }
    public string? GenderDescription { get; set; }
}
