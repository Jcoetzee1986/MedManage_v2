namespace MedManage.Core.Configuration;

/// <summary>
/// jsreport server connection settings
/// </summary>
public class JsReportSettings
{
    public const string SectionName = "JsReport";

    /// <summary>
    /// Base URL of the jsreport server (e.g., http://localhost:5488)
    /// </summary>
    public string ServerUrl { get; set; } = "http://localhost:5488";

    /// <summary>
    /// Username for jsreport authentication (if enabled)
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for jsreport authentication (if enabled)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// HTTP request timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 60;
}
