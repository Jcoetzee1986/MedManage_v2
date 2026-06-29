using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace MedManage.Infrastructure.Services;

/// <summary>
/// Singleton service managing a shared Chromium browser instance for PDF generation.
/// 
/// Resolution order for Chromium executable:
/// 1. PUPPETEER_EXECUTABLE_PATH env var (system-installed Chromium, e.g. /usr/bin/chromium)
/// 2. PUPPETEER_CACHE_DIR env var (PVC-cached download)
/// 3. OS temp directory (local dev fallback — downloads on first use)
/// 
/// Used by ReportGenerationService and LetterTemplateService.
/// </summary>
public class BrowserService : IDisposable
{
    private readonly ILogger<BrowserService> _logger;
    private readonly string? _executablePath;
    private readonly string _cachePath;
    private IBrowser? _browser;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public BrowserService(ILogger<BrowserService> logger)
    {
        _logger = logger;
        _executablePath = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");
        _cachePath = Environment.GetEnvironmentVariable("PUPPETEER_CACHE_DIR")
            ?? Path.Combine(Path.GetTempPath(), "puppeteer-cache");
    }

    public async Task<IBrowser> GetBrowserAsync()
    {
        if (_browser != null && _browser.IsConnected)
            return _browser;

        await _lock.WaitAsync();
        try
        {
            if (_browser != null && _browser.IsConnected)
                return _browser;

            string executablePath;

            // Option 1: Use system-installed Chromium (Docker image with apt chromium)
            if (!string.IsNullOrEmpty(_executablePath))
            {
                executablePath = _executablePath;
                _logger.LogInformation("Using system Chromium at: {ExecutablePath}", executablePath);
            }
            else
            {
                // Option 2/3: Download via BrowserFetcher (PVC cache or temp dir)
                executablePath = await EnsureChromiumDownloadedAsync();
            }

            _logger.LogInformation("Launching Chromium browser...");
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = executablePath,
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--single-process",
                    "--disable-extensions",
                    "--disable-background-networking"
                }
            });

            _logger.LogInformation("Chromium browser launched successfully. PID: {ProcessId}", _browser.Process?.Id);
            return _browser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Chromium browser. ExecutablePath: {ExecutablePath}, CachePath: {CachePath}",
                _executablePath ?? "(auto)", _cachePath);
            throw;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<string> EnsureChromiumDownloadedAsync()
    {
        _logger.LogInformation("Chromium cache directory: {CachePath}", _cachePath);

        var fetcherOptions = new BrowserFetcherOptions { Path = _cachePath };
        var browserFetcher = new BrowserFetcher(fetcherOptions);

        var installedBrowser = browserFetcher.GetInstalledBrowsers().FirstOrDefault();
        if (installedBrowser != null)
        {
            var path = installedBrowser.GetExecutablePath();
            _logger.LogInformation("Chromium already cached at {Path} (build {BuildId})",
                path, installedBrowser.BuildId);
            return path;
        }

        _logger.LogInformation("Downloading Chromium to {CachePath}...", _cachePath);
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var result = await browserFetcher.DownloadAsync();
        sw.Stop();

        var execPath = result.GetExecutablePath();
        _logger.LogInformation("Chromium downloaded in {ElapsedMs}ms. Path: {Path}, BuildId: {BuildId}",
            sw.ElapsedMilliseconds, execPath, result.BuildId);
        return execPath;
    }

    public void Dispose()
    {
        if (_browser != null)
        {
            _browser.Dispose();
            _browser = null;
        }
        _lock.Dispose();
    }
}
