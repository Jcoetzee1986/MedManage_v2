# MedManage.Modern — Getting Started

Complete guide for running the application locally or with Podman containers.

---

## Prerequisites

| Tool | Version | Purpose |
|------|---------|---------|
| SQL Server | 2016+ | Database (runs on host, not containerized) |
| .NET SDK | 10.0+ | API build & run |
| Node.js | 20.x+ | Angular build & run |
| Podman | 4.x+ | Container runtime (alternative to local dev) |
| podman-compose | latest | Multi-container orchestration |

---

## Database Setup

The MedManage database runs on your host machine (not in a container). Apply the SQL scripts below in order.

### SQL Scripts — Execution Order

Run these against your `MedManage` database in SQL Server Management Studio (SSMS) or via `sqlcmd`.

Scripts are numbered and must be executed in order:

| # | Script | Purpose | Required |
|---|--------|---------|----------|
| 1 | `001_AddAuditColumns.sql` | Adds audit columns to all tables (BaseEntity pattern) | Yes |
| 2 | `002_StandardizeDateInsertedColumns.sql` | Changes `DateInserted` from DATE to DATETIME | Yes |
| 3 | `003_RemoveOldAuditTriggers.sql` | Drops old/broken audit triggers | Yes (first time) |
| 4 | `004_FixAuditTriggers.sql` | Recreates all 24 audit triggers with SESSION_CONTEXT | Yes |
| 5 | `005_CreateRefreshTokensTable.sql` | Creates `RefreshTokens` table for JWT rotation | Yes |
| 6 | `006_CreatePasswordResetTokensTable.sql` | Creates `PasswordResetTokens` table | Yes |
| 7 | `007_AddIsPermanentlyBlocked.sql` | Adds `IsPermanentlyBlocked` column for user deactivation | Yes |
| 8 | `008_CreateCaseLetterNotesTable.sql` | Creates `CaseLetterNotes` table for letter generation | Yes |
| 9 | `009_SeedLetterTemplates.sql` | Seeds HTML letter templates per main client | Yes |
| 10 | `010_CreateTariffPercentageTable.sql` | Creates `Tariff.TariffPercentage` table + seed data | Yes |

All scripts are located in `Infrastructure/Scripts/`.

### Applying Scripts

```powershell
# From the project root (MedManage.Modern/)
sqlcmd -S . -d MedManage -E -i Infrastructure\Scripts\001_AddAuditColumns.sql
sqlcmd -S . -d MedManage -E -i Infrastructure\Scripts\002_StandardizeDateInsertedColumns.sql
# ... continue for all scripts in order
```

Or open each file in SSMS, connect to your MedManage database, and execute.

> **Note:** All scripts are idempotent — re-running is safe.

### Verify Database Connectivity

```powershell
Test-NetConnection -ComputerName localhost -Port 1433
```

Ensure SQL Server is configured for:
- **TCP/IP enabled** (SQL Server Configuration Manager → Protocols)
- **Integrated Security** or mixed mode authentication
- **Firewall allows port 1433** (for Podman container access)

---

## Option A: Run with Podman (Recommended for deployment)

This runs the API and Angular frontend in containers. Only the database stays on the host.

### 1. Create environment file

```powershell
cd MedManage.Modern
copy .env.example .env
```

Edit `.env` with your values:
```env
DB_PASSWORD=YourSqlServerPassword
JWT_SECRET=a-random-string-at-least-32-characters-long
```

### 2. Start all containers

```powershell
# Using the convenience script
.\scripts\podman-up.ps1 -Build -Detach

# Or directly with podman-compose
podman-compose up -d --build
```

### 3. Access the application

| Service | URL | Notes |
|---------|-----|-------|
| Frontend (HTTP) | http://localhost:8080 | Angular app (nginx proxies /api to the API) |
| Frontend (HTTPS) | https://localhost:8443 | Self-signed cert (accept browser warning) |
| API | http://localhost:5000 | .NET 10 API (Swagger at /swagger) |

### 4. Stop containers

```powershell
.\scripts\podman-down.ps1

# Or remove volumes too
.\scripts\podman-down.ps1 -Volumes
```

### How it works

```
┌─────────────────────────────────────────────────────────┐
│  Podman                                                  │
│                                                          │
│  ┌──────────┐    ┌──────────────┐                        │
│  │  nginx   │───▶│  .NET API    │                        │
│  │  :80/443 │    │  :8080       │                        │
│  └──────────┘    └──────┬───────┘                        │
│  (port 8080/8443)       │                                │
│                         │ host.containers.internal:1433   │
└─────────────────────────┼────────────────────────────────┘
                          │
                          ▼
              ┌──────────────────┐
              │  SQL Server      │  (host machine)
              │  :1433           │
              └──────────────────┘
```

### Troubleshooting Podman

**"Cannot connect to database"**
- Ensure SQL Server allows TCP connections on port 1433
- On Windows with Podman Machine, the host is `host.containers.internal`
- Check your `.env` DB_PASSWORD matches your SQL login

**"Podman machine not running"**
```powershell
podman machine start
```

**"Port already in use"**
```powershell
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

---

## Option B: Run Locally (for development)

Run the API and Angular directly on your machine for hot-reload development.

### 1. Configure the API

Edit `src/MedManage.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

### 2. Start the API

```powershell
dotnet watch run --project src\MedManage.API
```

The API starts at:
- `https://localhost:58764` (HTTPS)
- `http://localhost:58765` (HTTP)
- Swagger UI: `https://localhost:58764/swagger`

### 3. Start the Angular frontend

```powershell
cd client\medmanage-angular
npm install    # first time only
npm start
```

Angular dev server starts at `http://localhost:4200`.

The dev server proxies `/api` requests to the .NET API:
- **`npm start`** → proxies to `https://localhost:58764` (default)
- **`npm run start:http`** → proxies to `http://localhost:58765`

Proxy configs: `proxy.conf.json` (HTTPS) and `proxy.conf.http.json` (HTTP).

---

## Reports

Reports are generated entirely in-process — no external containers needed:

| Format | Technology | Library |
|--------|-----------|---------|
| Excel (.xlsx) | ClosedXML | NuGet: ClosedXML |
| PDF | HTML → PDF | NuGet: PuppeteerSharp |
| Case Letters | Handlebars templates → PDF | NuGet: Handlebars.Net + PuppeteerSharp |

Available reports:
- Cases Between Dates (PDF/Excel)
- Billing Summary (PDF/Excel)
- WIP Extract (PDF/Excel)
- Case Tariff Detail (PDF/Excel)
- My Cases (Excel)
- Case Comments Export (Excel)
- Linked Cases (PDF)
- Case Letter (PDF) — per-client Handlebars HTML templates

---

## Running Tests

```powershell
# All .NET tests
dotnet test

# Exclude integration tests (require running DB)
dotnet test --filter "Category!=Integration"

# Angular build check
cd client\medmanage-angular
npx ng build --configuration production
```

Current test status: **61 tests passing** (Infrastructure.Tests)

---

## Project Structure

```
MedManage.Modern/
├── src/
│   ├── MedManage.API/              # ASP.NET Core Web API (controllers, middleware)
│   ├── MedManage.Core/             # Domain entities, interfaces, DTOs, configuration
│   ├── MedManage.Infrastructure/   # EF Core, repositories, services, background processors
│   └── MedManage.Shared/           # Shared utilities
├── client/
│   └── medmanage-angular/          # Angular 19 SPA (standalone components)
├── tests/
│   ├── MedManage.Core.Tests/
│   ├── MedManage.Infrastructure.Tests/
│   └── MedManage.API.Tests/
├── Infrastructure/
│   ├── Scripts/                    # SQL migration scripts (numbered)
│   └── Templates/                  # HTML letter templates, base64 images
├── scripts/                        # podman-up.ps1, podman-down.ps1, test-report-endpoints.ps1
├── docs/                           # Documentation
├── podman-compose.yml              # API + Angular container orchestration
├── Dockerfile.api                  # API container build
├── Dockerfile.angular              # Angular container build (nginx)
├── nginx.conf                      # nginx reverse proxy config
└── .env.example                    # Environment variable template
```

---

## Environment Variables Reference

| Variable | Default | Description |
|----------|---------|-------------|
| `DB_PASSWORD` | *required* | SQL Server password for the connection string |
| `JWT_SECRET` | *required* | JWT signing key (32+ chars) |
| `ASPNETCORE_ENVIRONMENT` | `Production` | .NET environment (Production/Development) |

---

## Key Configuration (appsettings.json)

| Section | Purpose |
|---------|---------|
| `ConnectionStrings.DefaultConnection` | SQL Server connection |
| `JwtSettings` | Token signing key, issuer, audience, expiry (480 min) |
| `CorsSettings` | Allowed origins for CORS |
| `CaseLock` | Case lock timeout (5h) and cleanup interval (15m) |
| `Serilog` | Logging sinks (Console, File, Graylog) |

---

## Useful Commands

```powershell
# Build solution
dotnet build MedManage.Modern.sln

# Run API with hot-reload
dotnet watch run --project src\MedManage.API

# Build Angular for production
cd client\medmanage-angular
npx ng build --configuration production

# Run Podman stack
podman-compose up -d --build

# View API logs
podman logs -f medmanage-api

# Run report integration tests
.\scripts\test-report-endpoints.ps1 -Username "YourUser" -Password "YourPass"

# Apply a SQL script
sqlcmd -S . -d MedManage -E -i Infrastructure\Scripts\010_CreateTariffPercentageTable.sql
```

---

## Roles

The system uses these role names (must match database):

| Role | Access |
|------|--------|
| Case Manager | Case CRUD, letters, notes, bookings |
| System Administrator | All features + admin panel |
| Billing Auditing | Finance, billing, reports |
| Imports | Data import module |
| Metadata Administrator | Reference data, tariff admin |
