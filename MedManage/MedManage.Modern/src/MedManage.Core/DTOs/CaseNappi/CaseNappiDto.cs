namespace MedManage.Core.DTOs.CaseNappi;

public class CaseNappiDto
{
    public int CaseIdNappiId { get; set; }
    public int? CaseId { get; set; }
    public int? NappiId { get; set; }
    public decimal? Value { get; set; }
    public decimal? Quantity { get; set; }
    public bool? Dispensary { get; set; }
    public bool? Ward { get; set; }
    public bool? Theater { get; set; }
    public bool? Tto { get; set; }
    public DateOnly? Date { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
}

public class CreateCaseNappiDto
{
    public int? NappiId { get; set; }
    public decimal? Value { get; set; }
    public decimal? Quantity { get; set; }
    public bool? Dispensary { get; set; }
    public bool? Ward { get; set; }
    public bool? Theater { get; set; }
    public bool? Tto { get; set; }
    public DateOnly? Date { get; set; }
}

public class UpdateCaseNappiDto
{
    public int? NappiId { get; set; }
    public decimal? Value { get; set; }
    public decimal? Quantity { get; set; }
    public bool? Dispensary { get; set; }
    public bool? Ward { get; set; }
    public bool? Theater { get; set; }
    public bool? Tto { get; set; }
    public DateOnly? Date { get; set; }
}
