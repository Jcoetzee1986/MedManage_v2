# Serilog Configuration Guide

## Overview
The MedManage API uses Serilog for structured logging with JSON output, making it compatible with log aggregation systems like Graylog, ELK Stack, and others.

## Configuration Files

- **appsettings.json**: Base configuration for all environments
- **appsettings.Development.json**: Development-specific settings (verbose logging)
- **appsettings.Production.json**: Production settings (minimal logging to console only)

## Configurable Settings

All major application settings can be configured via appsettings.json and overridden with environment variables:

### CORS Settings
- `CorsSettings:AllowedOrigins`: Array of allowed origins
- `CorsSettings:AllowCredentials`: Enable/disable credential support

### JWT Settings
- `JwtSettings:SecretKey`: JWT signing key
- `JwtSettings:Issuer`: Token issuer
- `JwtSettings:Audience`: Token audience
- `JwtSettings:ExpirationMinutes`: Token expiration time

### Connection Strings
- `ConnectionStrings:DefaultConnection`: Database connection string

### Serilog Settings
- See detailed Serilog configuration below

## Log Format

All logs are output in **Compact JSON format** using Serilog's CompactJsonFormatter. This format includes:
- `@t`: Timestamp (ISO 8601)
- `@mt`: Message template
- `@l`: Level (Debug, Information, Warning, Error, Fatal)
- `@x`: Exception details (if applicable)
- Additional properties: MachineName, EnvironmentName, ThreadId, Application name

### Example Log Entry
```json
{
  "@t": "2026-04-16T10:30:15.1234567Z",
  "@mt": "Processing request {RequestPath}",
  "@l": "Information",
  "RequestPath": "/api/health",
  "MachineName": "PROD-WEB-01",
  "EnvironmentName": "Production",
  "Application": "MedManage.API"
}
```

## Environment Variables for Docker/Kubernetes

You can override any Serilog setting using environment variables with the following pattern:

```bash
Serilog__PropertyName__SubProperty=value
```

### Common Examples

#### Change minimum log level:
```bash
Serilog__MinimumLevel__Default=Debug
```

#### Override specific namespace logging:
```bash
Serilog__MinimumLevel__Override__Microsoft=Information
Serilog__MinimumLevel__Override__MedManage=Debug
```

#### Disable file logging (container mode):
```bash
Serilog__WriteTo__1__Name=
```

#### Change file path:
```bash
Serilog__WriteTo__1__Args__path=/var/log/medmanage/app-.json
```

## Docker Compose Example

```yaml
version: '3.8'
services:
  medmanage-api:
    image: medmanage-api:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      # Logging configuration
      - Serilog__MinimumLevel__Default=Information
      - Serilog__MinimumLevel__Override__MedManage=Debug
      # CORS configuration
      - CorsSettings__AllowedOrigins__0=https://app.example.com
      - CorsSettings__AllowedOrigins__1=https://app2.example.com
      - CorsSettings__AllowCredentials=true
      # Database connection
      - ConnectionStrings__DefaultConnection=Server=db;Database=MedManage;User Id=sa;Password=YourPassword;TrustServerCertificate=true
      # JWT configuration
      - JwtSettings__SecretKey=your-production-secret-key-min-32-chars
      - JwtSettings__Issuer=MedManage.API
      - JwtSettings__Audience=MedManage.Frontend
    logging:
      driver: "json-file"
      options:
        max-size: config
data:
  ASPNETCORE_ENVIRONMENT: "Production"
  # Logging
  Serilog__MinimumLevel__Default: "Information"
  Serilog__MinimumLevel__Override__MedManage: "Information"
  Serilog__MinimumLevel__Override__Microsoft: "Warning"
  # CORS
  CorsSettings__AllowedOrigins__0: "https://medmanage.k8s.example.com"
  CorsSettings__AllowCredentials: "true"
  # JWT
  JwtSettings__Issuer: "MedManage.API"
  JwtSettings__Audience: "MedManage.Frontend"

---
apiVersion: v1
kind: Secret
metadata:
  name: medmanage-secrets
type: Opaque
stringData:
  ConnectionStrings__DefaultConnection: "Server=sql-server;Database=MedManage;User Id=sa;Password=SecurePassword123;"
  JwtSettings__SecretKey: "your-super-secure-production-secret-key-minimum-32-characters
```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: medmanage-logging-config
data:
  ASPNETCORE_ENVIRONMENT: "Production"
  Serilog__MinimumLevel__Default: "Information"
  Serilog__MinimumLevel__Override__MedManage: "Information"
  Serilog__MinimumLevel__Override__Microsoft: "Warning"
```

## Graylog Integration

### Option 1: Collect JSON logs from stdout
Configure your container runtime to ship logs to Graylog using a log driver or agent (Fluentd, Filebeat, etc.).

### Option 2: Add GELF sink (optional)
Install `Serilog.Sinks.Graylog` package and add to appsettings.json:

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Graylog",
        "Args": {
          "hostnameOrAddress": "graylog.example.com",
          "port": 12201,
          "transportType": "Udp"
        }
      }
    ]
  }
}
```

Environment variable override:
```bash
Serilog__WriteTo__2__Name=Graylog
Serilog__WriteTo__2__Args__hostnameOrAddress=graylog.example.com
Serilog__WriteTo__2__Args__port=12201
```

## Log Levels

- **Debug**: Detailed diagnostic information (development only)
- **Information**: General operational messages
- **Warning**: Potentially harmful situations
- **Error**: Error events that might still allow the application to continue
- **Fatal**: Very severe error events that might cause the application to abort

## File Logging Configuration

File logs are written to the `logs/` directory with the following settings:

- **Rolling interval**: Daily
- **Retention**: 30 days (7 in development)
- **Max file size**: 100 MB (rolls over when exceeded)
- **Format**: Compact JSON

### Log File Location in Container

Mount a volume to persist logs:
```bash
docker run -v /host/logs:/app/logs medmanage-api:latest
```

Or use environment variable to change path:
```bash
-e Serilog__WriteTo__1__Args__path=/var/log/medmanage/app-.json
```

## Testing Configuration

To verify your logging configuration, check the startup logs for Serilog initialization messages, or call the health endpoint:

```bash
curl http://localhost:5000/api/health
```

The application will log the request and you can verify the JSON format in console or log files.

## Troubleshooting

### Logs not appearing
1. Check ASPNETCORE_ENVIRONMENT is set correctly
2. Verify minimum log level allows your messages
3. Check file permissions if using file logging

### JSON format not working
1. Ensure `Serilog.Formatting.Compact` package is installed
2. Verify formatter is specified in WriteTo configuration
3. Check for configuration syntax errors

### Environment variables not working
1. Ensure double underscore `__` is used (not single `:`)
2. Restart the application after changing environment variables
3. Check for typos in property names (case-sensitive)
