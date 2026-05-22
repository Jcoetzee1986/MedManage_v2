# Health Check Implementation

## Overview
The MedManage API includes comprehensive health monitoring with database connectivity verification. Two endpoints are available:

1. **`GET /health`** - ASP.NET Core built-in health checks (recommended for monitoring tools)
2. **`GET /api/health`** - Controller-based endpoint (backward compatible)

## Endpoints

### 1. Built-in Health Check: `/health`
**Purpose:** Standard health check endpoint for load balancers and monitoring systems.

**Response Format:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-01-15T10:30:00.000Z",
  "version": "1.0.0",
  "checks": [
    {
      "name": "database",
      "status": "Healthy",
      "description": null,
      "duration": 45.3,
      "exception": null
    }
  ]
}
```

**HTTP Status Codes:**
- `200 OK` - All checks passed (status: Healthy)
- `503 Service Unavailable` - One or more checks failed (status: Unhealthy)

### 2. Controller Endpoint: `/api/health`
**Purpose:** Backward-compatible health check with same functionality.

**Response Format:** Same as `/health` endpoint above.

**HTTP Status Codes:**
- `200 OK` - All checks passed (status: Healthy)
- `503 Service Unavailable` - One or more checks failed (status: Unhealthy)

## Database Connectivity Check

The health check verifies database connectivity by:
1. Opening a connection to SQL Server
2. Executing a simple query against the DbContext
3. Measuring response time in milliseconds
4. Capturing any connection exceptions

**What it checks:**
- ✅ Database server is reachable
- ✅ Connection string is valid
- ✅ Database exists and is accessible
- ✅ User has necessary permissions
- ✅ Connection pool is healthy

**When it fails:**
- ❌ SQL Server is down
- ❌ Network connectivity issues
- ❌ Database does not exist
- ❌ Invalid credentials
- ❌ Connection timeout

## Implementation Details

### Program.cs Configuration
```csharp
// Register health checks with database check
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MedManageDbContext>(
        name: "database",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql", "sqlserver" }
    );

// Map health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds,
                exception = e.Value.Exception?.Message
            })
        });
        await context.Response.WriteAsync(result);
    }
});
```

### HealthController Implementation
```csharp
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
```

## Usage Scenarios

### 1. Kubernetes Liveness/Readiness Probes
```yaml
livenessProbe:
  httpGet:
    path: /health
    port: 5000
  initialDelaySeconds: 30
  periodSeconds: 10

readinessProbe:
  httpGet:
    path: /health
    port: 5000
  initialDelaySeconds: 5
  periodSeconds: 5
```

### 2. Azure App Service Health Check
Configure in Azure Portal:
- Health check path: `/health`
- Interval: 30 seconds
- Unhealthy threshold: 3 failures

### 3. Load Balancer Health Check
Configure your load balancer to:
- Poll `/health` every 10 seconds
- Remove instance if 3 consecutive failures
- Expect HTTP 200 response

### 4. Monitoring Dashboard (PowerShell)
```powershell
# Check API health
$response = Invoke-RestMethod -Uri "https://api.medmanage.com/health"

if ($response.status -eq "Healthy") {
    Write-Host "✅ API is healthy" -ForegroundColor Green
    Write-Host "Database response time: $($response.checks[0].duration)ms"
} else {
    Write-Host "❌ API is unhealthy" -ForegroundColor Red
    $response.checks | ForEach-Object {
        Write-Host "  $($_.name): $($_.status)"
        if ($_.exception) {
            Write-Host "  Error: $($_.exception)" -ForegroundColor Yellow
        }
    }
}
```

## Future Enhancements

Consider adding additional health checks:
- **Redis Cache** (if implemented): Check cache connectivity
- **External APIs**: Verify third-party service dependencies
- **Disk Space**: Alert when low on storage
- **Memory Usage**: Monitor application memory consumption
- **Queue Depth**: Check message queue backlogs

Example:
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MedManageDbContext>("database")
    .AddRedis(redisConnectionString, "redis")
    .AddUrlGroup(new Uri("https://external-api.com/health"), "external-api")
    .AddDiskStorageHealthCheck(s => s.AddDrive("C:\\", 5120)) // 5GB minimum
    .AddProcessAllocatedMemoryHealthCheck(512); // 512MB threshold
```

## Troubleshooting

### Health check always returns "Healthy" even when DB is down
- Ensure you're calling the correct endpoint (`/health` or `/api/health`)
- Verify `AddHealthChecks()` and `MapHealthChecks()` are configured in Program.cs
- Check that `AddDbContextCheck<MedManageDbContext>()` is registered

### Timeout errors in health check
- Database connectivity might be slow
- Consider increasing connection timeout in connection string
- Review database performance and query execution plans

### 503 errors from health check
- Database is genuinely unavailable
- Connection string configuration issues
- Network connectivity problems
- SQL Server authentication failures

### Health check missing from Swagger
- Health check endpoints don't appear in Swagger by default
- This is intentional - health checks are infrastructure, not business APIs
- Access directly via `/health` or `/api/health`

## References

- [ASP.NET Core Health Checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)
- [EF Core Health Checks](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health)
- [Health Check Response Format](https://tools.ietf.org/id/draft-inadarei-api-health-check-01.html)
