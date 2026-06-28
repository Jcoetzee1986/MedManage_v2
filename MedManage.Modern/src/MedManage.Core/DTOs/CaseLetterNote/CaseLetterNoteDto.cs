namespace MedManage.Core.DTOs.CaseLetterNote;

public class CaseLetterNoteDto
{
    public int CaseId { get; set; }
    public string? Note { get; set; }
    public bool? IncludeDischargeForm { get; set; }
    public bool? IncludeReferralLetter { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateCaseLetterNoteDto
{
    public int CaseId { get; set; }
    public string? Note { get; set; }
    public bool? IncludeDischargeForm { get; set; }
    public bool? IncludeReferralLetter { get; set; }
}

public class UpdateCaseLetterNoteDto
{
    public string? Note { get; set; }
    public bool? IncludeDischargeForm { get; set; }
    public bool? IncludeReferralLetter { get; set; }
}
