using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MedManage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var healthReport = await _healthCheckService.CheckHealthAsync();
        
        var response = new
        {
            Status = healthReport.Status.ToString(),
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Checks = healthReport.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description,
                Duration = e.Value.Duration.TotalMilliseconds,
                Exception = e.Value.Exception?.Message
            })
        };

        return healthReport.Status == HealthStatus.Healthy 
            ? Ok(response) 
            : StatusCode(503, response);
    }
}
