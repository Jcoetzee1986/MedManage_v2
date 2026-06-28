# Graylog Server Setup for MedManage API

## Overview
MedManage API is configured to send structured logs directly to Graylog at `http://192.168.101.75/` using the GELF (Graylog Extended Log Format) protocol over UDP.

## Graylog Server Configuration

### 1. Create a GELF UDP Input
Graylog receives logs through inputs. You need to create a GELF UDP input to receive logs from the MedManage API.

**Steps:**
1. Log in to Graylog web interface: `http://192.168.101.75/`
2. Navigate to **System** → **Inputs**
3. Select **GELF UDP** from the dropdown
4. Click **Launch new input**
5. Configure the input:
   - **Title**: `MedManage API Logs`
   - **Bind address**: `0.0.0.0` (listens on all interfaces)
   - **Port**: `12201` (default GELF UDP port)
   - **Recv buffer size**: `262144` (256KB - recommended)
   - Click **Save**
6. Verify the input is running (green "RUNNING" status)

### 2. Create a Stream for MedManage
Streams help organize logs from different sources.

**Steps:**
1. Navigate to **Streams** → **Create Stream**
2. Configure:
   - **Title**: `MedManage API`
   - **Description**: `Logs from MedManage API application`
   - **Index Set**: Default (or create a dedicated index)
   - Click **Save**
3. Add Stream Rules:
   - Click **Manage Rules** on your new stream
   - Click **Add stream rule**
   - Rule Configuration:
     - **Field**: `facility`
     - **Type**: `match exactly`
     - **Value**: `MedManage.API`
   - Click **Save**
4. Enable the stream by clicking **Start stream**

### 3. Firewall Configuration
Ensure UDP port 12201 is open on the Graylog server.

**Windows Firewall (if Graylog is on Windows):**
```powershell
New-NetFirewallRule -DisplayName "Graylog GELF UDP" -Direction Inbound -Protocol UDP -LocalPort 12201 -Action Allow
```

**Linux Firewall (if Graylog is on Linux):**
```bash
# UFW
sudo ufw allow 12201/udp

# iptables
sudo iptables -A INPUT -p udp --dport 12201 -j ACCEPT
```

### 4. Verify Connectivity
From the MedManage API server, test connectivity to Graylog:

**PowerShell Test:**
```powershell
Test-NetConnection -ComputerName 192.168.101.75 -Port 12201 -InformationLevel Detailed
```

**Note:** UDP connectivity tests may not always work since UDP is connectionless. The best test is to start the API and check if logs appear in Graylog.

## Application Configuration

### Current Configuration
The MedManage API is configured in `appsettings.json`:

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Graylog",
        "Args": {
          "hostnameOrAddress": "192.168.101.75",
          "port": "12201",
          "transportType": "Udp",
          "facility": "MedManage.API"
        }
      }
    ]
  }
}
```

### Configuration Options

#### Transport Types
- **Udp** (Current): Fast, no guaranteed delivery, suitable for most scenarios
- **Tcp**: Guaranteed delivery, slightly slower, use for critical logs
- **Http**: Uses REST API, most reliable but slowest

To change to TCP:
```json
{
  "Name": "Graylog",
  "Args": {
    "hostnameOrAddress": "192.168.101.75",
    "port": "12201",
    "transportType": "Tcp",
    "facility": "MedManage.API"
  }
}
```

#### Additional Configuration Options
```json
{
  "Name": "Graylog",
  "Args": {
    "hostnameOrAddress": "192.168.101.75",
    "port": "12201",
    "transportType": "Udp",
    "facility": "MedManage.API",
    "minimumLogEventLevel": "Information",
    "messageIdGeneratorType": "Timestamp",
    "shortMessageMaxLength": 500,
    "stackTraceDepth": 10,
    "includeMessageTemplate": true
  }
}
```

**Parameter Descriptions:**
- **hostnameOrAddress**: Graylog server IP or hostname
- **port**: GELF input port (default: 12201)
- **transportType**: Udp, Tcp, or Http
- **facility**: Application identifier in Graylog (used for filtering/routing)
- **minimumLogEventLevel**: Minimum log level to send (Verbose, Debug, Information, Warning, Error, Fatal)
- **messageIdGeneratorType**: How to generate message IDs (Timestamp, Md5)
- **shortMessageMaxLength**: Maximum length of short message field
- **stackTraceDepth**: How many stack trace frames to include
- **includeMessageTemplate**: Include Serilog message template

## Environment-Specific Configuration

### Development (appsettings.Development.json)
For local development, you may want to disable Graylog:

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Console"
      }
    ]
  }
}
```

### Production (appsettings.Production.json)
Recommended production configuration:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      {
        "Name": "Graylog",
        "Args": {
          "hostnameOrAddress": "192.168.101.75",
          "port": "12201",
          "transportType": "Tcp",
          "facility": "MedManage.API",
          "minimumLogEventLevel": "Information"
        }
      }
    ]
  }
}
```

## Graylog Dashboard Setup

### Create Dashboards for MedManage

1. **Navigate to Dashboards** → **Create dashboard**
2. **Title**: `MedManage API Overview`
3. Add widgets:

#### Widget 1: Log Messages Over Time
- **Widget Type**: Search result count
- **Query**: `facility:MedManage.API`
- **Timeframe**: Last 24 hours
- **Chart Type**: Line chart

#### Widget 2: Log Levels Distribution
- **Widget Type**: Quick values
- **Field**: `level`
- **Query**: `facility:MedManage.API`
- **Limit**: 10

#### Widget 3: Top Error Messages
- **Widget Type**: Message table
- **Query**: `facility:MedManage.API AND level:Error`
- **Fields**: timestamp, message, exception
- **Timeframe**: Last 24 hours

#### Widget 4: Application Health
- **Widget Type**: Message table
- **Query**: `facility:MedManage.API AND (message:"Health check" OR message:"API Starting")`
- **Fields**: timestamp, message

### Create Alerts

#### Alert 1: High Error Rate
1. Navigate to **Alerts** → **Event Definitions** → **Create Event Definition**
2. Configure:
   - **Title**: `MedManage - High Error Rate`
   - **Description**: `Alert when error rate exceeds threshold`
   - **Condition**: Filter & Aggregation
   - **Search Query**: `facility:MedManage.API AND level:Error`
   - **Aggregation**: count() > 10
   - **Time Range**: Last 5 minutes
   - **Execute search every**: 1 minute
3. Add Notification:
   - **Email** or **Slack** notification
   - Include message: "High error rate detected in MedManage API"

#### Alert 2: Application Down
1. **Title**: `MedManage - Application Down`
2. **Condition**: No activity detected
3. **Search Query**: `facility:MedManage.API`
4. **Aggregation**: count() < 1
5. **Time Range**: Last 10 minutes

## Viewing Logs in Graylog

### Basic Search
Navigate to **Search** and use these queries:

**All MedManage logs:**
```
facility:MedManage.API
```

**Errors only:**
```
facility:MedManage.API AND level:Error
```

**Specific time range:**
```
facility:MedManage.API AND timestamp:[2026-04-18 00:00:00 TO 2026-04-18 23:59:59]
```

**Authentication logs:**
```
facility:MedManage.API AND (message:login OR message:authentication)
```

**Database operations:**
```
facility:MedManage.API AND (message:database OR SourceContext:MedManageDbContext)
```

**Health checks:**
```
facility:MedManage.API AND message:health
```

### Useful Fields to Display
Add these fields to your search results:
- `timestamp` - When the log occurred
- `level` - Log level (Information, Warning, Error, etc.)
- `message` - Log message
- `SourceContext` - Class/component that generated the log
- `RequestPath` - HTTP request path
- `RequestId` - Unique request identifier
- `MachineName` - Server that generated the log
- `EnvironmentName` - Development, Staging, Production
- `exception` - Exception details (for errors)

## Testing the Integration

### 1. Restore NuGet Packages
```powershell
cd c:\Code\MedManage\MedManage\MedManage.Modern
dotnet restore
```

### 2. Start the API
```powershell
cd src\MedManage.API
dotnet run
```

### 3. Generate Test Logs
The API will automatically log:
- Application startup: `"MedManage API Starting..."`
- HTTP requests (via ASP.NET Core logging)
- Database queries (EF Core logging)
- Authentication attempts
- Errors and exceptions

### 4. Verify in Graylog
1. Open Graylog: `http://192.168.101.75/`
2. Navigate to **Search**
3. Search for: `facility:MedManage.API`
4. You should see logs appearing within seconds

### 5. Force an Error (for testing)
Access a non-existent endpoint to generate an error log:
```powershell
Invoke-WebRequest -Uri "https://localhost:5001/api/nonexistent" -SkipCertificateCheck
```

This will create a 404 error in Graylog.

## Troubleshooting

### No Logs Appearing in Graylog

**Check 1: Input is Running**
- Go to **System** → **Inputs**
- Verify GELF UDP input shows "RUNNING" status
- Check "Network IO" shows activity

**Check 2: Firewall**
```powershell
# Test from API server
Test-NetConnection -ComputerName 192.168.101.75 -Port 12201
```

**Check 3: Application Logs**
Check local file logs in `src/MedManage.API/logs/` to verify logging is working.

**Check 4: Serilog Configuration**
Verify in `appsettings.json`:
- `Using` array includes `"Serilog.Sinks.Graylog"`
- `WriteTo` array includes Graylog configuration
- Port and hostname are correct

**Check 5: Network Connectivity**
```powershell
# Ping Graylog server
ping 192.168.101.75

# Check route
tracert 192.168.101.75
```

### Logs Delayed or Missing

**Issue**: UDP can drop packets under high load
**Solution**: Switch to TCP transport:
```json
"transportType": "Tcp"
```

**Issue**: Minimum log level filtering
**Solution**: Check `minimumLogEventLevel` in configuration matches what you expect

### Too Many Logs

**Issue**: EF Core generates verbose query logs
**Solution**: Increase minimum level for EF Core:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Override": {
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    }
  }
}
```

## Performance Considerations

### UDP Transport (Current)
- **Pros**: Fast, non-blocking, doesn't slow down application
- **Cons**: No delivery guarantee, can lose logs under heavy load
- **Use for**: Most production scenarios, high-volume logging

### TCP Transport
- **Pros**: Guaranteed delivery, reliable
- **Cons**: Blocks if Graylog is slow/down, can impact performance
- **Use for**: Critical logs, lower-volume applications

### Async Wrapper (Recommended for Production)
To prevent logging from blocking the application, wrap Graylog sink in async wrapper:

```json
{
  "Name": "Async",
  "Args": {
    "configure": [
      {
        "Name": "Graylog",
        "Args": {
          "hostnameOrAddress": "192.168.101.75",
          "port": "12201",
          "transportType": "Tcp",
          "facility": "MedManage.API"
        }
      }
    ]
  }
}
```

Requires: `Serilog.Sinks.Async` package

## Security Considerations

### 1. Network Security
- Consider using VPN or private network for Graylog communication
- Restrict Graylog input to specific IP ranges
- Use TLS/SSL for TCP transport in production

### 2. Sensitive Data
Avoid logging:
- Passwords
- Credit card numbers
- Personal health information (PHI)
- API keys/tokens

Use Serilog destructuring to sanitize:
```csharp
Log.Information("User {UserID} logged in", userId); // Good
Log.Information("Login: {Username} / {Password}", user, pass); // BAD!
```

### 3. Log Retention
Configure appropriate retention policies in Graylog:
- System → Indices → Manage Index Sets
- Set rotation/retention based on compliance requirements
- HIPAA: Consider 6+ year retention for audit logs

## References

- [Serilog.Sinks.Graylog Documentation](https://github.com/whir1/serilog-sinks-graylog)
- [Graylog GELF Documentation](https://docs.graylog.org/docs/gelf)
- [Serilog Best Practices](https://github.com/serilog/serilog/wiki/Configuration-Basics)
- [ASP.NET Core Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
