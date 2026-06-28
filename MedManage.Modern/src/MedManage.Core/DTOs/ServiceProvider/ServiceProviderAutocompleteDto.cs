namespace MedManage.Core.DTOs.ServiceProvider;

/// <summary>
/// Lightweight DTO for autocomplete/typeahead results
/// </summary>
public class ServiceProviderAutocompleteDto
{
    public int ServiceProviderId { get; set; }
    public string? ServiceProviderName { get; set; }
    public string? ServiceProviderSurname { get; set; }
    public string? PracticeName { get; set; }
    public string? PracticeNr { get; set; }
    public string DisplayText => BuildDisplayText();

    private string BuildDisplayText()
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(ServiceProviderSurname))
            parts.Add(ServiceProviderSurname);
        if (!string.IsNullOrWhiteSpace(ServiceProviderName))
            parts.Add(ServiceProviderName);
        if (!string.IsNullOrWhiteSpace(PracticeName))
            parts.Add($"({PracticeName})");
        if (!string.IsNullOrWhiteSpace(PracticeNr))
            parts.Add($"[{PracticeNr}]");

        return string.Join(" ", parts);
    }
}
