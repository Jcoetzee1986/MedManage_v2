using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace MedManage.Infrastructure.Services;

/// <summary>
/// Singleton service managing a shared Chromium browser instance for PDF generation.
/// Downloads Chromium to a configurable path (supports PVC mounts in k8s).
/// Used by ReportGenerationService and LetterTemplateService.
/// </summary>
public class BrowserService : IDisposable
{
    private readonly ILogger<BrowserService> _logger;
    private readonly string _cachePath;
    private IBrowser? _browser;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public BrowserService(ILogger<BrowserService> logger)
    {
        _logger = logger;
        // Use PUPPETEER_CACHE_DIR env var if set (for PVC mount), otherwise default
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

            _logger.LogInformation("Chromium cache directory: {CachePath}", _cachePath);

            var fetcherOptions = new BrowserFetcherOptions { Path = _cachePath };
            var browserFetcher = new BrowserFetcher(fetcherOptions);

            // Check if already downloaded
            var installedBrowser = browserFetcher.GetInstalledBrowsers().FirstOrDefault();
            if (installedBrowser != null)
            {
                _logger.LogInformation("Chromium already downloaded at {Path} (revision {Revision})",
                    installedBrowser.GetExecutablePath(), installedBrowser.BuildId);
            }
            else
            {
                _logger.LogInformation("Downloading Chromium to {CachePath}...", _cachePath);
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = await browserFetcher.DownloadAsync();
                sw.Stop();
                _logger.LogInformation("Chromium downloaded successfully in {ElapsedMs}ms. Path: {Path}, BuildId: {BuildId}",
                    sw.ElapsedMilliseconds, result.GetExecutablePath(), result.BuildId);
            }

            _logger.LogInformation("Launching Chromium browser...");
            
            // Get the executable path from the fetcher
            var installedBrowsers = browserFetcher.GetInstalledBrowsers().ToList();
            var executablePath = installedBrowsers.First().GetExecutablePath();
            _logger.LogInformation("Chromium executable: {ExecutablePath}", executablePath);

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
            _logger.LogError(ex, "Failed to initialize Chromium browser. CachePath: {CachePath}", _cachePath);
            throw;
        }
        finally
        {
            _lock.Release();
        }
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
