namespace MedManage.Core.DTOs.ReferenceData;

public class ChronicIllnessDto
{
    public double? ChronicIllnessId { get; set; }
    public string? ChronicIllnessName { get; set; }
    public string? ChronicIllnessDescription { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateChronicIllnessDto
{
    public string? ChronicIllnessName { get; set; }
    public string? ChronicIllnessDescription { get; set; }
}

public class UpdateChronicIllnessDto
{
    public double? ChronicIllnessId { get; set; }
    public string? ChronicIllnessName { get; set; }
    public string? ChronicIllnessDescription { get; set; }
}
