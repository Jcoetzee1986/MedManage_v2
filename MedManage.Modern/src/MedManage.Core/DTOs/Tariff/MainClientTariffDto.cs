namespace MedManage.Core.DTOs.Tariff;

public class MainClientTariffDto
{
    public int MainClientId { get; set; }
    public int TariffNameId { get; set; }
    public int? TariffPeriodName { get; set; }
    public DateTime? DateInserted { get; set; }
}

public class CreateMainClientTariffDto
{
    public int MainClientId { get; set; }
    public int TariffNameId { get; set; }
    public int? TariffPeriodName { get; set; }
}
